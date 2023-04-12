using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Core;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;
using Zhaoxi.SmartParking.Server.Models.Dto;

namespace Zhaoxi.SmartParking.Server.Service
{
    public class AutoRegisterService : BaseService, IAutoRegisterService
    {
        public AutoRegisterService(SqlSugarContext sqlSugarContext) : base(sqlSugarContext) { }

        public async Task<PageResult<List<AutoRegisterModel>>> Pages(PageQuery pageQuery)
        {
            RefAsync<int> total = 0;

            var result = await _sqlSugarClient.Queryable<AutoRegisterModel>().Where(x => x.State == 1)
                .WhereIF(!string.IsNullOrEmpty(pageQuery.Search), (x) => x.AutoLicense.Contains(pageQuery.Search))
                .ToPageListAsync(pageQuery.PageNo, pageQuery.PageSize, total);

            if (result.Count > 0)
            {
                var licenseColorIdList = new List<int>();

                var autoColorIdList = new List<int>();

                var feeModeIdList = new List<int>();

                foreach (var item in result)
                {
                    if (!licenseColorIdList.Contains(item.LicenseColorId)) licenseColorIdList.Add(item.LicenseColorId);

                    if (!autoColorIdList.Contains(item.AutoColorId)) autoColorIdList.Add(item.AutoColorId);

                    if (!feeModeIdList.Contains(item.FeeModeId)) feeModeIdList.Add(item.FeeModeId);
                }

                var feeModeList = await _sqlSugarClient.Queryable<BaseFeeModel>().Where(x => feeModeIdList.Contains(x.FeeModelId)).ToListAsync();

                var autoColorList = await _sqlSugarClient.Queryable<BaseAutoColorModel>().Where(x => autoColorIdList.Contains(x.ColorId)).ToListAsync();

                var licenseColorList = await _sqlSugarClient.Queryable<BaseLicenseColorModel>().Where(x => licenseColorIdList.Contains(x.ColorId)).ToListAsync();

                foreach (var item in result)
                {
                    var feeModel = feeModeList.Find(x => x.FeeModelId == item.FeeModeId);

                    item.FeeModeName = feeModel?.FeeModelName;

                    var autoColor = autoColorList.Find(x => x.ColorId == item.AutoColorId);

                    item.AutoColorName = autoColor?.ColorName;

                    var licenseColor = licenseColorList.Find(x => x.ColorId == item.LicenseColorId);

                    item.LicenseColorName = licenseColor?.ColorName;
                }
            }

            var pages = (int)Math.Ceiling(total * 1.0 / pageQuery.PageSize);

            var pageResult = new PageResult<List<AutoRegisterModel>> { Data = result, Rows = total, Pages = pages, IsSuccess = true };

            return pageResult;
        }

        public async Task Save(AutoRegisterModel autoRegisterModel)
        {
            if (autoRegisterModel.AutoId == 0)
            {
                var list = await _sqlSugarClient.Queryable<AutoRegisterModel>().Where(x => x.AutoLicense == autoRegisterModel.AutoLicense && x.State == 1).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"车牌号[{autoRegisterModel.AutoLicense}]已经存在");
                }

                autoRegisterModel.ValidStartTime = DateTime.Now;

                autoRegisterModel.State = 1;

                await _sqlSugarClient.Insertable(autoRegisterModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<AutoRegisterModel>().Where(x => x.AutoId == autoRegisterModel.AutoId && x.State == 1).ToListAsync();

                if (list.Count == 0) return;

                if (autoRegisterModel.State == 1)
                {
                    // 修改
                    // 校验名称是否存在

                    var tempList = await _sqlSugarClient.Queryable<AutoRegisterModel>().Where(x => x.AutoLicense == autoRegisterModel.AutoLicense && x.State == 1).ToListAsync();

                    // 查询到多条记录 或者查询到一条记录且不是当前记录
                    if (tempList.Count > 1 || (tempList.Count == 1 && tempList[0].AutoId != autoRegisterModel.AutoId))
                    {
                        throw new Exception($"车牌号[{autoRegisterModel.AutoLicense}]已经存在");
                    }

                    list[0].AutoLicense = autoRegisterModel.AutoLicense;

                    list[0].AutoColorId = autoRegisterModel.AutoColorId;

                    list[0].LicenseColorId = autoRegisterModel.LicenseColorId;

                    list[0].FeeModeId = autoRegisterModel.FeeModeId;

                    list[0].Description = autoRegisterModel.Description;

                    list[0].ValidEndTime = autoRegisterModel.ValidEndTime;
                }
                else
                {
                    // 删除
                    list[0].State = 0;
                }
                await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();
            }
        }

    }
}
