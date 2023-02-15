using ZeroFriction.DataAccessLayer.DataStucture;
using ZeroFriction.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ZeroFriction.DataAccessLayer.StandardData.Databases
{
    public abstract class AbstractDatabase
    {
        protected string spName;
        protected int paramCount;
        public Execution ExecutionMode;


        public abstract string GetConnectionString(DBMode dbMode);

        protected abstract void CreateDBSets(DBMode dbMode);

        public abstract string GetConnectionString(DBMode dbMode, int portalId);

        protected abstract void CreateDBSets(DBMode dbMode, int portalId);

        protected abstract bool OpenConnection(string conString);

        public abstract void CloseConnection();

  
        public abstract int[] ExecuteQuery(ExecuteQueryObj executeObj);
        public abstract IDataReader[] ExecuteQueryWithIDataReader(string sql, List<SqlParameter> sqlParams);
        public abstract DataSet ExecuteQueryWithDataSet(string sql, List<SqlParameter> sqlParams);
        public abstract object[] ExecuteQueryWithScalar(ExecuteQueryObj executeObj);
        public abstract void BeginTran();
        public abstract void CommitTran();
        public abstract void RollbackTran();
        public abstract void saveBulkData(ExecuteQueryObj eqo);
        public abstract void MergeTableData(string SourceTableName, string TargetTableName, List<string> matchingColumns, List<string> updatingColumns);
        public abstract void CreateTable(string tableName, DataColumnCollection columns);

        public abstract void DropTable(string tableName);
        public abstract int BatchWiseBulkSave(ExecuteQueryObj eqo);

        /// <summary>
        /// Add parameters to the SP
        /// </summary>
        public abstract void AddParameter(string paramName, object value, DataType paramType);

        /// <summary>
        /// Execute a SP in Databases
        /// </summary>
        /// <returns>Array of Interger</returns>
        public abstract int[] CallSP();

        /// <summary>
        /// Execute a SP in Databases and Get Resultsets
        /// </summary>
        /// <param name="timeOut">Timeout period</param>
        /// <returns>Array of IDataReaders</returns>
        public abstract IDataReader[] CallSPWithDataSet(Timeout timeOut);

        /// <summary>
        /// Execute a SP in Database and Get  Scalar Results
        /// </summary>
        /// <returns>Array of Objects</returns>
        public abstract object[] CallSPWithScaler();

        /// <summary>
        /// Set Stored Procedure Name
        /// </summary>
        /// <param name="sp">Name of SP</param>
        public abstract void SetSP(string name);

        /// <summary>
        /// Set the count of parameters
        /// </summary>
        /// <param name="count">param count</param>
        public abstract void SetParamCount(int count);

        //can extended for the other db executers like Code first, DB first
    }
}
