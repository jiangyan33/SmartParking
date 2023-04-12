using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;
using Zhaoxi.SmartParking.Server.Models.Dto;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface IAutoRegisterService
    {
        public Task<PageResult<List<AutoRegisterModel>>> Pages(PageQuery pageQuery);

        public Task Save(AutoRegisterModel autoRegisterModel);
    }
}
