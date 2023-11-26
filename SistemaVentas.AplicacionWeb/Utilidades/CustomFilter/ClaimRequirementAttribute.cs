using Microsoft.AspNetCore.Mvc;

namespace SistemaVentas.AplicacionWeb.Utilidades.CustomFilter
{
    public class ClaimRequirementAttribute: TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string controlador, string accion): base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { controlador, accion };
        }
    }
}
