using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Zhaoxi.SmartParking.Client.Upgrade.Models;

namespace Zhaoxi.SmartParking.Client.Upgrade.DataAccess
{
    public static class LocalDataAccess
    {
        private static SQLiteConnection _conn;

        private static SQLiteCommand _comm;

        private static SQLiteDataAdapter _adapter;

        private static SQLiteTransaction _trans;

        private static void Dispose()
        {
            if (_trans != null)
            {
                _trans.Rollback();

                _trans.Dispose();

                _trans = null;
            }

            if (_adapter != null)
            {
                _adapter.Dispose();

                _adapter = null;
            }

            if (_comm != null)
            {
                _comm.Dispose();

                _comm = null;
            }

            if (_conn != null)
            {
                _conn.Close();

                _conn.Dispose();

                _conn = null;
            }
        }

        private static bool Connection()
        {
            try
            {
                if (_conn == null)
                {
                    _conn = new SQLiteConnection("data source=Zhaoxi.SmartParking.Client.Data.dll");
                }
                _conn.Open();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdateFile(string fileName, string fileMd5, string fileLen)
        {
            if (Connection())
            {
                try
                {
                    var querySql = $"select file_name,file_md5,file_len from file_version where file_name='{fileName}'";

                    _adapter = new SQLiteDataAdapter(querySql, _conn);

                    var dataTable = new DataTable();

                    _adapter.Fill(dataTable);

                    string exeSql;

                    if (dataTable.Rows.Count == 0)
                    {
                        // 不存在执行新增
                        exeSql = $"insert into file_version(file_name,file_md5,file_len) values('{fileName}','{fileMd5}','{fileLen}')";
                    }
                    else
                    {
                        // 存在执行修改
                        exeSql = $"update file_version set file_md5='{fileMd5}', file_len={fileLen} where file_name='{fileName}'";
                    }

                    _comm = new SQLiteCommand(exeSql, _conn);

                    _comm.ExecuteNonQuery();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    Dispose();
                }
            }
            return false;
        }

        public static void InitTables()
        {
            // 先判断是否存在数据库文件，如果不存在，创建一个文件，实例化数据库表
            if (!File.Exists("Zhaoxi.SmartParking.Client.Data.dll"))
            {
                Connection();

                var sql = $"CREATE TABLE \"file_version\" (\r\n  \"file_id\" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,\r\n  \"file_name\" TEXT,\r\n  \"file_md5\" TEXT,\r\n  \"file_len\" TEXT\r\n);";

                _comm = new SQLiteCommand();

                _comm.Connection = _conn;

                _comm.CommandText = sql;

                _comm.ExecuteNonQuery();

                Dispose();
            }
        }
    }
}
