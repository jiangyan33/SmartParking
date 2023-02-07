using System;
using System.Data;
using System.Data.SQLite;

namespace Zhaoxi.SmartParking.Client.DAL
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

        public static DataTable GetFileList()
        {
            if (Connection())
            {
                try
                {
                    var sql = "select file_name,file_md5,file_len from file_version";

                    _adapter = new SQLiteDataAdapter(sql, _conn);

                    var dataTable = new DataTable();

                    _adapter.Fill(dataTable);

                    return dataTable;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Dispose();
                }
            }
            return null;
        }
    }
}
