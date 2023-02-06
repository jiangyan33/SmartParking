using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
