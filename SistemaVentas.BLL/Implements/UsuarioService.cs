using SistemaVentas.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Net;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity.Models;
using System.Text.Encodings.Web;

namespace SistemaVentas.BLL.Implements
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IFirebaseService _firebase;
        private readonly IUtilidadesService _utilidades;
        private readonly ICorreoService _correo;


        public UsuarioService(IGenericRepository<Usuario> repositorio, IFirebaseService firebase, IUtilidadesService utilidades, ICorreoService correo)
        {
            _repositorio = repositorio;
            _firebase = firebase;
            _utilidades = utilidades;
            _correo = correo;
        }

        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repositorio.Consultar();
            return query.Include(r => r.IdRolNavigation).ToList();
        }

        public async Task<Usuario> Crear(Usuario entidad, Stream foto = null, string nombreFoto = "", string urlPlantillaCorreo = "")
        {
            Usuario usuario_existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo);

            if(usuario_existe != null)
            {
                throw new TaskCanceledException("El correo ya existe");
            }

            try
            {
                string clave_generada = _utilidades.GenerarClave();
                entidad.Clave = _utilidades.ConvertirSha256(clave_generada);
                entidad.NombreFoto = nombreFoto;

                if(foto != null)
                {
                    string urlFoto = await _firebase.SubirStorage(foto, "carpeta_usuario",nombreFoto);
                    entidad.UrlFoto = urlFoto;
                }

                Usuario usuario_creado = await _repositorio.Crear(entidad);

                if(usuario_creado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se puedo crear el usuario");
                }

                if(urlPlantillaCorreo != "")
                {
                    urlPlantillaCorreo = urlPlantillaCorreo.Replace("[correo]", usuario_creado.Correo).Replace("[clave]", clave_generada);

                    string htmlCorreo = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPlantillaCorreo);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader readerStream = null;

                            if(response.CharacterSet == null)
                            {
                                readerStream = new StreamReader(dataStream);
                            }
                            else
                            {
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                            }

                            htmlCorreo = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();
                        }
                    }

                    if(htmlCorreo != "")
                    {
                        await _correo.EnviarCorreo(usuario_creado.Correo, "Cuenta Creada", htmlCorreo);
                    }

                    
                }
                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario == usuario_creado.IdUsuario);
                usuario_creado = query.Include(r => r.IdRolNavigation).First();

                return usuario_creado;
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        public async Task<Usuario> Editar(Usuario entidad, Stream foto = null, string nombreFoto = "")
        {
            Usuario usuario_existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo&&u.IdUsuario !=entidad.IdUsuario);

            if (usuario_existe != null)
            {
                throw new TaskCanceledException("El correo ya existe");
            }

            try
            {
                IQueryable<Usuario> queryUsuario = await _repositorio.Consultar(u=>u.IdUsuario == entidad.IdUsuario);
                Usuario usuario_editar = queryUsuario.First();

                usuario_editar.Nombre =entidad.Nombre;
                usuario_editar.Correo = entidad.Correo;
                usuario_editar.Telefono = entidad.Telefono;
                usuario_editar.IdRol =entidad.IdRol;
                usuario_editar.EsActivo = entidad.EsActivo;

                if(usuario_editar.NombreFoto == "")
                {
                    usuario_editar.NombreFoto = nombreFoto;
                }

                if (foto != null)
                {
                    string urlFoto = await _firebase.SubirStorage(foto, "carpeta_usuario", usuario_editar.NombreFoto);
                    usuario_editar.UrlFoto = urlFoto;
                }

                bool respuesta = await _repositorio.Editar(usuario_editar);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se puedo modificar el usuario");
                }

                Usuario usuario_editado = queryUsuario.Include(r => r.IdRolNavigation).First();

                return usuario_editado;
            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u=> u.IdUsuario == idUsuario);

                if(usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                string nombreFoto = usuario_encontrado.NombreFoto;

                bool respuesta = await _repositorio.Eliminar(usuario_encontrado);

                if (respuesta)
                {
                    await _firebase.EliminarStorage("carpeta_usuario", nombreFoto);
                }
                return true;

            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> ObtnerPorCredenciales(string correo, string clave)
        {
            string clave_encriptada = _utilidades.ConvertirSha256(clave);

            Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Correo.Equals(correo) && u.Clave.Equals(clave_encriptada));

            return usuario_encontrado;
        }

        public async Task<Usuario> ObtnerPorId(int idUsuario)
        {
            IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario == idUsuario);

            Usuario result = query.Include(r => r.IdRolNavigation).FirstOrDefault();

            return result;
        }

        public async Task<bool> GuardarPerfil(Usuario entidad)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u=>u.IdUsuario == entidad.IdUsuario);

                if(usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                usuario_encontrado.Correo = entidad.Correo;
                usuario_encontrado.Telefono = entidad.Telefono;

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CambiarClave(int idUsuario, string claveActual, string claveNueva)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == idUsuario);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                if(usuario_encontrado.Clave != _utilidades.ConvertirSha256(claveActual))
                {
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");
                }

                usuario_encontrado.Clave = _utilidades.ConvertirSha256(claveNueva);

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }       

        public async Task<bool> RestablecerClave(string correo, string urlPlantillaCorreo)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Correo == correo);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("No encontramos ningun usuario asociado al correo");
                }

                string clave_generada = _utilidades.GenerarClave();

                usuario_encontrado.Clave = _utilidades.ConvertirSha256(clave_generada);



                urlPlantillaCorreo = urlPlantillaCorreo.Replace("[clave]", clave_generada);

                string htmlCorreo = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;

                        if (response.CharacterSet == null)
                        {
                            readerStream = new StreamReader(dataStream);
                        }
                        else
                        {
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }

                bool correo_enviado = false;

                if (htmlCorreo != "")
                {
                    correo_enviado =  await _correo.EnviarCorreo(correo, "Contraseña Restablecida", htmlCorreo);
                }

                if (!correo_enviado) {
                    throw new TaskCanceledException("Tenemos problemas. Porfavor intentalo de nuevo o mas tarde");
                }

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;

            }
            catch
            {
                throw;
            }
        }
    }
}
