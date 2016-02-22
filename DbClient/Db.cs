using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.DbClient
{
    public abstract class Db : IDisposable
    {
        private static Dictionary<string, Db> databases;
        private IDbConnection connection;
        private IDbTransaction transaction;
        private string connectionString;
        public virtual bool AutoCommit { get; set; }
        public virtual CommandType DefaultCommandType { get { return CommandType.StoredProcedure; } }
        public virtual int DefaultTimeOut { get { return 30; } }

        public static Db Create(string name)
        {
            Db database = null;
            if (databases == null)
            {
                databases = new Dictionary<string, Db>();
                string providerName;
                for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
                {
                    try
                    {
                        var c = ConfigurationManager.ConnectionStrings[i];
                        if (string.IsNullOrEmpty(c.ProviderName))
                        {
                            providerName = "Artisan.Tools.DbClients.SqlClient.SqlClientDb, Artisan.Tools.DbClients.SqlClient";
                        }
                        else
                        {
                            string[] parts = c.ProviderName.Split(new char[] { '.' });
                            string prov = parts[parts.Length - 1];
                            providerName = string.Format("Artisan.Tools.DbClients.{0}.{0}Db, Artisan.Tools.DbClients.{0}", prov); ;
                        }

                        Type databaseType = Type.GetType(providerName);
                        //Assembly assembly = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, providerName));
                        //if (assembly != null)
                        //{

                        //Type databaseType = assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(Db)));

                        if (databaseType != null)
                        {
                            databases[c.Name] = (Db)Activator.CreateInstance(databaseType);
                            databases[c.Name].connectionString = c.ConnectionString;
                        }
                        //}
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            databases.TryGetValue(name, out database);

            return database;
        }

        //public static Db Create(string name)
        //{
        //    Db database = null;
        //    if (databases == null)
        //    {
        //        databases = new Dictionary<string, Db>();
        //        string providerName;
        //        for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
        //        {
        //            try
        //            {
        //                var c = ConfigurationManager.ConnectionStrings[i];
        //                if (string.IsNullOrEmpty(c.ProviderName))
        //                {
        //                    providerName = "DbExec.SqlClient.dll";
        //                }
        //                else
        //                {
        //                    string[] parts = c.ProviderName.Split(new char[] { '.' });
        //                    providerName = "DbExec." + parts[parts.Length - 1] + ".dll";
        //                }

        //                Assembly assembly = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, providerName));
        //                if (assembly != null)
        //                {

        //                    Type databaseType = assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(Db)));

        //                    if (databaseType != null)
        //                    {
        //                        databases[c.Name] = (Db)Activator.CreateInstance(databaseType);
        //                        databases[c.Name].connectionString = c.ConnectionString;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }

        //    databases.TryGetValue(name, out database);

        //    return database;
        //}

        protected Db()
        {

        }

        public abstract IDbConnection CreateConnection();

        public abstract IDbDataAdapter CreateAdapter();

        protected virtual DbCommand CreateDbCommand(IDbCommand cmd)
        {
            return new DbCommand(this, cmd);
        }

        public virtual DbCommand GetCommand(string commandText)
        {
            return GetCommand(commandText, DefaultCommandType, DefaultTimeOut);
        }

        public virtual DbCommand GetCommand(string commandText, CommandType commandType)
        {
            return GetCommand(commandText, DefaultCommandType, DefaultTimeOut);
        }

        public virtual DbCommand GetCommand(string commandText, CommandType commandType, int commandTimeout)
        {
            if (connection == null)
            {
                this.connection = CreateConnection();
                this.connection.ConnectionString = connectionString;
                this.connection.Open();
            }
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            cmd.CommandTimeout = commandTimeout;
            if (transaction != null)
                cmd.Transaction = transaction;

            return CreateDbCommand(cmd);
        }

        public virtual IDbTransaction BeginTransaction()
        {
            if (transaction == null)
            {
                if (connection != null)
                {
                    transaction = connection.BeginTransaction();
                }
            }
            return transaction;
        }

        public virtual IDbTransaction Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
            return transaction;
        }

        public virtual IDbTransaction Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
            return transaction;
        }

        public virtual void Dispose()
        {
            if (transaction != null)
            {
                if (AutoCommit)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                transaction.Dispose();
            }

            if (connection != null)
            {
                connection.Close();
            }
        }
    }

}
