using Artisan.Tools.DbClient;
using System.Data.SqlClient;

namespace Artisan.Tools.DbClients.SqlClient
{
    public class SqlClientDb : Db
    {
        public override System.Data.IDbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        public override System.Data.IDbDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter();
        }
    }

}
