using SqlSugar;
using System;
using System.IO;

namespace DeeGateway.Repository
{
    public class DBInstance
    {
        public static SqlSugarClient GetInstance(string connectionString, DbType dbType, bool isAutoCloseConnection = true)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = isAutoCloseConnection,
                InitKeyType = InitKeyType.Attribute
            });
            return db;
        }
        public static SqlSugarClient GetInstance()
        {
            var config = Utils.Config.ConfigHelper.GetConfig();
            return  GetInstance(config.DataBase.ConnectionString, (DbType)Enum.Parse(typeof(DbType), config.DataBase.DBType,true));
        }

    }
}
