namespace ConsultorioMedAPP.Utilities
{
    public class SessionHelper
    {
        public static bool EstaAutenticado(ISession session)
        {
            var cedula = session.GetInt32("Cedula");
            return cedula != null && cedula != 0;
        }

        public static int? ObtenerCedula(ISession session)
        {
            return session.GetInt32("Cedula");
        }

        public static string ObtenerRol(ISession session)
        {
            return session.GetString("Rol") ?? "Sin rol";
        }
    }
}