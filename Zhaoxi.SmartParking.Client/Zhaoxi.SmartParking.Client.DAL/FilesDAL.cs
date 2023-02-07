using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class FilesDAL : IFilesDAL
    {
        public Task<string> List()
        {
            return WebDataAccess.PostDatas("/files/list", new { });
        }

        public Task<string> LocalList()
        {
            var list = LocalDataAccess.GetFileList();

            // file_name,file_md5,file_len
            var temp = list.AsEnumerable().Select(x => new
            {
                FileName = x["file_name"].ToString(),
                FileMD5 = x["file_md5"].ToString(),
                Length = Convert.ToInt32(x["file_len"].ToString())
            }).ToList();

            return Task.FromResult(JsonSerializer.Serialize(temp));
        }
    }
}
