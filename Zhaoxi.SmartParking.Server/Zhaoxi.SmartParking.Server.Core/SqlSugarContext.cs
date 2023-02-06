using Microsoft.Extensions.Configuration;
using SqlSugar;
using System.Data;
using System.Linq;

namespace Zhaoxi.SmartParking.Server.Core
{
    public class SqlSugarContext
    {

        private readonly SqlSugarClient _client;

        /// <summary>
        /// 获取SqlSugarClient实例
        /// </summary>
        public SqlSugarClient GetInstance => _client;

        public SqlSugarContext(IConfiguration Configuration)
        {
            var connectionString = Configuration.GetSection("ConnectionString").Value;

            var connectionConfig = new ConnectionConfig
            {
                DbType = SqlSugar.DbType.MySql,
                ConnectionString = connectionString,
                IsAutoCloseConnection = true,
            };

            _client = new SqlSugarClient(connectionConfig);

#if DEBUG
            _client.Aop.OnLogExecuting = (sql, pars) =>
            {

                System.Console.WriteLine(sql);//输出sql

                System.Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
            };
#endif
        }
    }
}
