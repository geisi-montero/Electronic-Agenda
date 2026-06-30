using Oracle.ManagedDataAccess.Client;

namespace Agenda
{
    class Conexion
    {
        private string cadena =

  "User Id=YOUR_USERNAME;" +
  "Password=YOUR_PASSWORD;" +
  "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=YOUR_SERVICE_NAME)));";

        public OracleConnection ObtenerConexion()
        {
            OracleConnection cn = new OracleConnection(cadena);

            return cn;
        }
    }
}
