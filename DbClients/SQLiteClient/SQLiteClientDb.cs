using Artisan.Tools.DbClient;
using System.Data.SQLite;
using System.Data;

namespace Artisan.Tools.DbClients.SQLiteClient
{
    public class SQLiteClientDb : Db
    {
        public override CommandType DefaultCommandType { get { return CommandType.Text; } }

        public override System.Data.IDbConnection CreateConnection()
        {
            return new SQLiteConnection();
        }

        public override System.Data.IDbDataAdapter CreateAdapter()
        {
            return new SQLiteDataAdapter();
        }
    }

}
