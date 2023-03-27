using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public Task<string> Delete(string fileName)
        {
            return WebDataAccess.PostDatas("/files/delete/" + fileName, new { });
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

        public async void UploadFile(string file, string updatePath, Action<int> progressChanged, Action completed)
        {
            var uri = "/files/upload";

            var datas = new Dictionary<string, object>();

            var (md5, fileLen) = GetFileMd5(file);

            datas.Add("md5", md5);

            datas.Add("updatePath", updatePath);

            datas.Add("length", fileLen);

            WebDataAccess.Upload(uri, file, progressChanged, completed, datas);
        }

        public async void UploadIcon(string userName, string file)
        {
            var uri = "/files/uploadIcon/" + userName;

            WebDataAccess.Upload(uri, file, null, null, null);
        }

        private (string, long) GetFileMd5(string fileName)
        {
            try
            {
                if (!File.Exists(fileName)) throw new Exception("文件不存在");

                var file = new FileStream(fileName, FileMode.Open);

                var fileLen = file.Length;

                var md5 = new MD5CryptoServiceProvider();

                var retVal = md5.ComputeHash(file);

                file.Close();

                var sb = new StringBuilder();

                for (var i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return (sb.ToString(), fileLen);
            }
            catch (Exception ex)
            {
                throw new Exception("GetFileMd5 fail,error:" + ex.Message);
            }
        }
    }
}
