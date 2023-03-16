using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.BLL
{
    public class FilesBLL : IFilesBLL
    {
        private readonly IFilesDAL _filesDAL;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public FilesBLL(IFilesDAL filesDAL)
        {
            _filesDAL = filesDAL;

            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<List<UpgradeFileEntity>> List()
        {
            var str = await _filesDAL.List();

            var result = JsonSerializer.Deserialize<ResultEntity<List<UpgradeFileEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> Delete(string fileName)
        {
            var str = await _filesDAL.Delete(fileName);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<List<UpgradeFileEntity>> LocalList()
        {
            var str = await _filesDAL.LocalList();

            var result = JsonSerializer.Deserialize<List<UpgradeFileEntity>>(str);

            return result;
        }

        public async void UploadFile(string file, string updatePath, Action<int> progressChanged, Action completed)
        {
            _filesDAL.UploadFile(file, updatePath, progressChanged, completed);
        }

    }
}
