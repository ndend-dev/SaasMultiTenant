using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaasMultiTenant.Utils
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Editor = "Editor";
            public const string Lector = "Lector";
        }

        public static class Status
        {
            public const string Activo = "Activo";
            public const string Pendiente = "Pendiente";
            public const string Completado = "Completado";
        }
    }
}
