using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artisan.Tools.DbClient
{
    public class DbCommand : IDisposable
    {
        protected List<IDbDataParameter> parameters;
        protected IDbCommand command;
        protected Db db;

        internal DbCommand(Db database, IDbCommand cmd)
        {
            this.db = database;
            command = cmd;
            parameters = new List<IDbDataParameter>();
        }

        public virtual IDbDataParameter this[string name]
        {
            get
            {
                return parameters.FirstOrDefault(p => p.ParameterName == name);
            }
            set
            {
                int index = parameters.FindIndex(p => p.ParameterName == name);
                if ((index >= 0) && (index < parameters.Count))
                {
                    this[index] = value;
                }
                else
                {
                    this.Add(value);
                }
            }
        }


        public virtual IDbDataParameter this[int index]
        {
            get
            {
                if ((index < 0) && (index >= parameters.Count))
                    throw new ArgumentException("Index + " + index + " is out of range");
                return parameters[index];
            }
            set
            {
                if ((index < 0) && (index >= parameters.Count))
                    throw new ArgumentException("Index + " + index + " is out of range");
                parameters[index] = value;
            }
        }

        public virtual void Insert(int index, IDbDataParameter item)
        {
            if ((index < 0) && (index >= parameters.Count))
                throw new ArgumentException("Index + " + index + " is out of range");

            if (item == null)
                throw new ArgumentException("Item can not be null");

            parameters.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            if ((index < 0) && (index >= parameters.Count))
                throw new ArgumentException("Index + " + index + " is out of range");

            parameters.RemoveAt(index);
        }

        public virtual void Add(IDbDataParameter item)
        {
            if (item == null)
                throw new ArgumentException("Item can not be null");

            parameters.Add(item);
        }


        public virtual IDbDataParameter Add(string name, object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameters.Add(parameter);
            return parameter;
        }

        public virtual IDbDataParameter Add(string name, DbType type, object value)
        {
            return Add(name, type, ParameterDirection.InputOutput, value);
        }

        public virtual IDbDataParameter Add(string name, DbType type, ParameterDirection direction, object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = direction;
            parameter.DbType = type;
            parameters.Add(parameter);
            return parameter;
        }

        public virtual void Clear()
        {
            parameters.Clear();
        }

        public virtual bool Contains(IDbDataParameter item)
        {
            if (item == null)
                throw new ArgumentException("Item can not be null");

            return parameters.Contains(item);
        }

        public virtual int Count
        {
            get { return parameters.Count; }
        }

        public virtual IEnumerator<IDbDataParameter> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        public virtual DataSet ExecuteDataSet()
        {
            return ExecuteDataSet(new DataSet(), null);
        }

        public virtual DataSet ExecuteDataSet(string[] tableNames)
        {
            return ExecuteDataSet(new DataSet(), tableNames);
        }

        public virtual DataSet ExecuteDataSet(DataSet dataSet, string[] tableNames)
        {
            IDbDataAdapter adapter = db.CreateAdapter();
            PrepareCommand();
            adapter.SelectCommand = command;
            try
            {
                DateTime startTime = DateTime.Now;
                if (tableNames != null)
                {
                    string systemCreatedTableNameRoot = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        string systemCreatedTableName = (i == 0)
                            ? systemCreatedTableNameRoot
                            : systemCreatedTableNameRoot + i;

                        adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                    }
                }
                adapter.Fill(dataSet);
                //this.instrumentation.CommandExecuted(startTime);
            }
            catch
            {
                //this.instrumentation.CommandFailed("DbDataAdapter.Fill()", ConnectionStringNoCredentials);
                throw;
            }

            return dataSet;
        }


        public virtual object ExecuteScalar()
        {
            try
            {
                DateTime date = DateTime.Now;
                PrepareCommand();
                object returnValue = command.ExecuteScalar();
                //this.instrumentation.CommandExecuted(date);
                return returnValue;
            }
            catch
            {
                //this.instrumentation.CommandFailed(command.Command.CommandText, ConnectionStringNoCredentials);
                throw;
            }
        }

        public virtual int ExecuteNonQuery()
        {
            int rowsAffected;
            try
            {
                DateTime startTime = DateTime.Now;
                PrepareCommand();
                rowsAffected = command.ExecuteNonQuery();
                //this.instrumentation.CommandExecuted(startTime);
            }
            catch
            {
                //this.instrumentation.CommandFailed(command.Command.CommandText, ConnectionStringNoCredentials);
                throw;
            }
            return rowsAffected;
        }

        public virtual IDataReader ExecuteReader()
        {
            return ExecuteReader(CommandBehavior.Default);
        }

        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                PrepareCommand();
                IDataReader reader = command.ExecuteReader(behavior);
                //this.instrumentation.CommandExecuted(startTime);
                return reader;
            }
            catch
            {
                //this.instrumentation.CommandFailed(command.CommandText, ConnectionStringNoCredentials);
                throw;
            }
        }

        protected virtual void PrepareCommand()
        {
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
            command.Prepare();
        }

        public virtual void Dispose()
        {
            if (command != null)
                command.Dispose();
        }
    }
}
