using Artisan.Tools.DbClient;
using Oracle.ManagedDataAccess.Client;

namespace Artisan.Tools.DbClients.OracleClient
{
    public class OracleClientDb : Db
    {
        public override System.Data.IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        public override System.Data.IDbDataAdapter CreateAdapter()
        {
            return new OracleDataAdapter();
        }
    }

}
