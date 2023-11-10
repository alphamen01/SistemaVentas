using SistemaVentas.BLL.Interfaces;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Implements
{
    public class NegocioService : INegocioService
    {
        private readonly IGenericRepository<Negocio> _repositorio;
        private readonly IFirebaseService _firebase;

        public NegocioService(IGenericRepository<Negocio> repositorio, IFirebaseService firebase)
        {
            _repositorio = repositorio;
            _firebase = firebase;
        }

        public async Task<Negocio> Obtener()
        {
            try
            {
                Negocio negocio_encontrado = await _repositorio.Obtener(n => n.IdNegocio == 1);
                return negocio_encontrado;  

            }catch
            {
                throw;
            }
        }

        public async Task<Negocio> GuardarCambios(Negocio entidad, Stream logo = null, string nombreLogo = "")
        {
            try
            {
                Negocio negocio_encontrado = await _repositorio.Obtener(n => n.IdNegocio == 1);

                negocio_encontrado.NumeroDocumento = entidad.NumeroDocumento;
                negocio_encontrado.Nombre = entidad.Nombre;
                negocio_encontrado.Correo = entidad.Correo;
                negocio_encontrado.Direccion = entidad.Direccion;
                negocio_encontrado.Telefono = entidad.Telefono;
                negocio_encontrado.PorcentajeImpuesto = entidad.PorcentajeImpuesto;
                negocio_encontrado.SimboloMoneda = entidad.SimboloMoneda;

                negocio_encontrado.NombreLogo = negocio_encontrado.NombreLogo == "" ? nombreLogo : negocio_encontrado.NombreLogo;

                if(logo != null)
                {
                    string urlFoto = await _firebase.SubirStorage(logo, "carpeta_logo", negocio_encontrado.NombreLogo!);
                    negocio_encontrado.UrlLogo = urlFoto;
                }

                await _repositorio.Editar(negocio_encontrado);
                return negocio_encontrado;

            }catch
            {
                throw;
            }
        }

    }
}
