using ZeroFriction.DataAccessLayer.DataStucture;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroFriction.DataAccessLayer.StandardData.Databases
{
    public class DBFactory
    {
        #region Private
        private readonly ILogger _logger;
        private readonly ILoggerFactory loggerFactory;

        private static DBFactory instance;
        #endregion

        private DBFactory(ILoggerFactory logger)
        {
            loggerFactory = logger;
            _logger = logger.CreateLogger<DBFactory>();
        }

        /// <summary>
        /// Get selected Database and can extended
        /// </summary>
        /// <param name="dbType">Database Type</param>
        /// <param name="dbMode">Database Mode</param>
        /// <returns>AbstractDatabase DB</returns>
        public AbstractDatabase GetDB(DBType dbType, DBMode dbMode)
        {
            AbstractDatabase DB = null;
            try
            {
                switch (dbType)
                {
                    case DBType.MsSQL:
                        if (dbMode == DBMode.DB1)
                            DB = new MsSQLDatabase(loggerFactory, dbMode);
                        break;
                    case DBType.MongoDB:
                        break;
                }
                return DB;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetDB error thrown:{0}", ex);
                throw ex;
            }

        }

        /// <summary>
        /// Create singleton instance
        /// </summary>
        /// <param name="dbType">Database Type</param>
        /// <param name="dbMode">Database Mode</param>
        /// <returns>DBFactory object</returns>
        public static DBFactory CreateDB(ILoggerFactory logger)
        {
            ILoggerFactory loggerFactory = logger;
            ILogger _logger = logger.CreateLogger<DBFactory>();

            try
            {
                lock (typeof(DBFactory))
                {
                    if (instance == null)
                    {
                        instance = new DBFactory(loggerFactory);
                    }
                    return instance;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
