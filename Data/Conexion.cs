using Oracle.ManagedDataAccess.Client;

namespace Agenda
{
    class Conexion
    {
        private string cadena =

"User Id=system;" +
"Password=Oracle123;" +
"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));";

        public OracleConnection ObtenerConexion()
        {
            OracleConnection cn = new OracleConnection(cadena);

            return cn;
        }
    }
}