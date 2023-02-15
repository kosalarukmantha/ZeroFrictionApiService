using ZeroFriction.DataAccessLayer.Configuration;
using ZeroFriction.DataAccessLayer.DataStucture;
using ZeroFriction.DataAccessLayer.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ZeroFriction.DataAccessLayer.StandardData.Databases
{
    public class MsSQLDatabase : AbstractDatabase
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly object syncLock = new object();

        private ConfigWrapper config = new ConfigWrapper();
        protected SqlConnection msSqlCon;
        protected string conString;
        private SqlTransaction msSQLTran;

        protected SqlParameter[] paraList;
        protected int index = 0;

        public MsSQLDatabase(ILoggerFactory logger, DBMode dbMode)
        {
            loggerFactory = logger;
            _logger = logger.CreateLogger<MsSQLDatabase>();

            CreateDBSets(dbMode);
        }

        public override string GetConnectionString(DBMode dbMode)
        {
            NameValueCollection section = new NameValueCollection();
            ConfigWrapper wrapper = new ConfigWrapper();
            try
            {
                switch (dbMode)
                {
                    case DBMode.DB1: section = wrapper.GetDBCollection("DB1"); break;
                    case DBMode.DB2: section = wrapper.GetDBCollection("DB2"); break;
                    default: section = wrapper.GetDBCollection("DB1"); break;
                }

                string ConnString = "";
                string integratedSecurity = section.GetValues("integratedSecurity")[0];
                string Server = section.GetValues("server")[0];
                string Database = section.GetValues("database")[0];
                string ConnectTimeOut = section.GetValues("connecttimeout") == null ? "" : section.GetValues("connecttimeout")[0];
                string Connectionminpoolsize = section.GetValues("connectionminpoolsize") == null ? "" : section.GetValues("connectionminpoolsize")[0];
                string Connectionmaxpoolsize = section.GetValues("connectionmaxpoolsize") == null ? "" : section.GetValues("connectionmaxpoolsize")[0];
                var alwaysEncryptionEnabled = section.GetValues("alwaysEncryptionEnabled") == null ? false : bool.Parse(section.GetValues("alwaysEncryptionEnabled")[0]);

                if (bool.Parse(integratedSecurity))
                {
                    ConnString = "Server=" + Server + ";database=" + Database + ";Integrated Security=True";
                }
                else
                {
                    string Username = section.GetValues("username")[0];
                    string Password = section.GetValues("password")[0];
                    //password encrption can handle
                    ConnString = "Server=" + Server + ";database=" + Database + ";UID=" + Username + ";PWD=" + Password;
                }

                if (!string.IsNullOrEmpty(ConnectTimeOut))
                {
                    ConnString += ";Connect Timeout=" + ConnectTimeOut;
                }

                if (!string.IsNullOrEmpty(Connectionminpoolsize))
                {
                    ConnString += ";Min Pool Size=" + Connectionminpoolsize;
                }

                if (!string.IsNullOrEmpty(Connectionmaxpoolsize))
                {
                    ConnString += ";Max Pool Size=" + Connectionmaxpoolsize;
                }

                return ConnString;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetConnectionString error thrown:{0}", ex);
                throw ex;
            }
        }

        protected override void CreateDBSets(DBMode dbMode)
        {
            NameValueCollection section = new NameValueCollection();

            try
            {
                string ConnString = GetConnectionString(dbMode);
                OpenConnection(ConnString);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateDBSets error thrown:{0}", ex);
                throw ex;
            }
        }

        public override string GetConnectionString(DBMode dbMode, int portalId)
        {
            throw new NotImplementedException();
        }

        protected override void CreateDBSets(DBMode dbMode, int portalId)
        {
            throw new NotImplementedException();
        }

        protected override bool OpenConnection(string conString)
        {
            try
            {
                msSqlCon = new SqlConnection();
                msSqlCon.ConnectionString = conString;

                //check connection is open   
                if (msSqlCon.State == ConnectionState.Open)
                {
                    //if open then close  
                    msSqlCon.Close();
                }

                msSqlCon.Open();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("OpenConnection error thrown:{0}", ex);
                msSqlCon.Dispose();
                return false;
            }
        }

        public override void CloseConnection()
        {
            try
            {
                ExecutionMode = Execution.Single;
                msSqlCon.Close();
                msSqlCon.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("CloseConnection error thrown:{0}", ex);
                throw ex;
            }
        }

        public override int[] ExecuteQuery(ExecuteQueryObj executeObj)
        {
            int[] result = new int[1];
            string paramValues = "";
            int commandTimeOut = int.Parse(config.GetAppSettingsItem("TimeOut").ToString());

            try
            {
                string sql = executeObj.Sql;
                List<SqlParameter> sqlParams = executeObj.SqlParams;


                foreach (SqlParameter sqlParam in sqlParams)
                {
                    sql = sql.Replace("??" + sqlParam.ParameterName, sqlParam.Value.ToString());
                    sql = sql.Replace("_@" + sqlParam.ParameterName, "_" + sqlParam.Value.ToString());
                    sql = sql.Replace("N@" + sqlParam.ParameterName, "N'" + sqlParam.Value.ToString() + "'");

                    if (null != sqlParam.Value)
                        paramValues += sqlParam.ParameterName + "=" + sqlParam.Value.ToString() + " ";
                }

                SqlCommand sqlCommand = new SqlCommand(sql, msSqlCon);
                sqlCommand.Parameters.Clear();

                DateTime dt1 = DateTime.Now;

                sqlCommand.CommandText = sql;
                sqlCommand.CommandTimeout = commandTimeOut;

                if (msSQLTran != null)
                {
                    sqlCommand.Transaction = msSQLTran;
                }

                //Create view does not support parameterization
                if (!sql.Contains("CREATE VIEW"))
                {
                    foreach (SqlParameter sqlParam in sqlParams)
                    {
                        sqlCommand.Parameters.Add(sqlParam);
                    }
                }

                result[0] = sqlCommand.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("ExecuteQuery error thrown:{0}", ex);
                throw ex;
            }
            finally
            {
                if (ExecutionMode != Execution.Multiple)
                    CloseConnection();
            }
        }

        public override IDataReader[] ExecuteQueryWithIDataReader(string sql, List<SqlParameter> sqlParams)
        {
            DateTime startTime = DateTime.Now;
            int commandTimeOut = int.Parse(config.GetAppSettingsItem("TimeOut").ToString());
            string paramValues = "";
            lock (syncLock)
            {
                IDataReader[] result = new IDataReader[1];

                try
                {
                    foreach (SqlParameter sqlParam in sqlParams)
                    {
                        sql = sql.Replace("??" + sqlParam.ParameterName, sqlParam.Value.ToString());
                        sql = sql.Replace("_@" + sqlParam.ParameterName, "_" + sqlParam.Value.ToString());
                        sql = sql.Replace("N@" + sqlParam.ParameterName, "N'" + sqlParam.Value.ToString() + "'");

                        if (null != sqlParam.Value)
                            paramValues = sqlParam.ParameterName + "=" + sqlParam.Value.ToString() + " ";
                    }


                    SqlCommand sqlCommand = new SqlCommand(sql, msSqlCon);
                    foreach (SqlParameter sqlParam in sqlParams)
                    {
                        sqlCommand.Parameters.Add(sqlParam);
                    }

                    sqlCommand.CommandTimeout = commandTimeOut;

                    if (msSQLTran != null)
                    {
                        sqlCommand.Transaction = msSQLTran;
                    }

                    SqlDataReader drGet = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    result[0] = (IDataReader)drGet;

                    LogTimeoutQueries(paramValues, sql, startTime, commandTimeOut);

                    return result;

                }
                catch (Exception ex)
                {
                    LogTimeoutQueries(paramValues, sql, startTime, commandTimeOut);

                    _logger.LogError("ExecuteQueryWithIDataReader error thrown:{0}", ex);
                    if (ExecutionMode != Execution.Multiple)
                        CloseConnection();
                    throw;
                }
                finally
                {
                    
                }
            }
        }

        private void LogTimeoutQueries(string paramValues, string sql, DateTime queryStart, int commandTimeOut)
        {
            try
            {
                DateTime dt2 = DateTime.Now;
                double timeTaken = (dt2 - queryStart).TotalSeconds;
                if (timeTaken > commandTimeOut)
                {
                    string body = "Executed Query " + sql + " @@Parameters " + paramValues + " Time taken(ms)-" + timeTaken + " commandTimeOut " + commandTimeOut;
                    _logger.LogInformation("body:{0}", body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LogTimeoutQueries error thrown:{0}", ex);
            }
        }



        public override DataSet ExecuteQueryWithDataSet(string sql, List<SqlParameter> sqlParams)
        {
            throw new NotImplementedException();
        }

        public override object[] ExecuteQueryWithScalar(ExecuteQueryObj executeObj)
        {
            throw new NotImplementedException();
        }

        public override void BeginTran()
        {
            throw new NotImplementedException();
        }

        public override void CommitTran()
        {
            throw new NotImplementedException();
        }

        public override void RollbackTran()
        {
            throw new NotImplementedException();
        }

        public override void saveBulkData(ExecuteQueryObj eqo)
        {
            throw new NotImplementedException();
        }

        public override void MergeTableData(string SourceTableName, string TargetTableName, List<string> matchingColumns, List<string> updatingColumns)
        {
            throw new NotImplementedException();
        }

        public override void CreateTable(string tableName, DataColumnCollection columns)
        {
            throw new NotImplementedException();
        }

        public override void DropTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override int BatchWiseBulkSave(ExecuteQueryObj eqo)
        {
            throw new NotImplementedException();
        }

        public override void AddParameter(string paramName, object value, DataType paramType)
        {
            throw new NotImplementedException();
        }

        public override int[] CallSP()
        {
            throw new NotImplementedException();
        }

        public override IDataReader[] CallSPWithDataSet(Timeout timeOut)
        {
            throw new NotImplementedException();
        }

        public override object[] CallSPWithScaler()
        {
            throw new NotImplementedException();
        }

        public override void SetSP(string name)
        {
            throw new NotImplementedException();
        }

        public override void SetParamCount(int count)
        {
            throw new NotImplementedException();
        }
    }
}
