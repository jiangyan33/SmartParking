using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
