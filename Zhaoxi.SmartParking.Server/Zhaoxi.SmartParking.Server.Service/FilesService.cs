using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Core;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Service
{
    public class FilesService : BaseService, IFilesService
    {
        public FilesService(SqlSugarContext sqlSugarContext) : base(sqlSugarContext) { }

        public async Task<List<UpgradeFileModel>> List()
        {
            var list = await _sqlSugarClient.Queryable<UpgradeFileModel>().Where(x => x.State == 1).ToListAsync();

            return list;
        }

        public async Task Save(UpgradeFileModel upgradeFileModel)
        {
            var list = await _sqlSugarClient.Queryable<UpgradeFileModel>().Where(x => x.State == 1 && x.FileName == upgradeFileModel.FileName).ToListAsync();

            if (list.Count > 0)
            {
                // 修改
                list[0].UpdateTime = System.DateTime.Now;

                list[0].FileMD5 = upgradeFileModel.FileMD5;

                list[0].UpdatePath = upgradeFileModel.UpdatePath;

                await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();
            }
            else
            {
                upgradeFileModel.FilePath = "Web_Files\\Upgrade_Files\\" + upgradeFileModel.FileName;

                upgradeFileModel.CreateTime = System.DateTime.Now;

                upgradeFileModel.UpdateTime = System.DateTime.Now;

                await _sqlSugarClient.Insertable(upgradeFileModel).ExecuteCommandAsync();
            }
        }

        public async Task<bool> Delete(string fileName)
        {
            var list = await _sqlSugarClient.Queryable<UpgradeFileModel>().Where(x => x.State == 1 && x.FileName == fileName).ToListAsync();

            if (list.Count == 0) return true;

            list[0].UpdateTime = DateTime.Now;

            list[0].State = 0;

            await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();

            // 删除文件
            var root = Path.Combine(Environment.CurrentDirectory, @"Web_Files\Upgrade_Files");

            var filePath = $@"{root}\{fileName}";

            if (!System.IO.File.Exists(filePath)) return true;

            System.IO.File.Delete(filePath);

            return true;
        }
    }
}
