using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Core;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Service
{
    public class BaseInfoService : BaseService, IBaseInfoService
    {
        public BaseInfoService(SqlSugarContext sqlSugarContext) : base(sqlSugarContext) { }

        public async Task<List<BaseAutoColorModel>> GetAllAutoColor()
        {
            var list = await _sqlSugarClient.Queryable<BaseAutoColorModel>().ToListAsync();

            var model = list.FirstOrDefault();

            return list;
        }

        public async Task<List<BaseLicenseColorModel>> GetAllLicenseColor()
        {
            var list = await _sqlSugarClient.Queryable<BaseLicenseColorModel>().ToListAsync();

            var model = list.FirstOrDefault();

            return list;
        }

        public async Task<List<BaseFeeModel>> GetAllFeeModel()
        {
            var list = await _sqlSugarClient.Queryable<BaseFeeModel>().ToListAsync();

            var model = list.FirstOrDefault();

            return list;
        }

        public async Task SaveAutoColor(BaseAutoColorModel baseAutoColorModel)
        {
            if (baseAutoColorModel.ColorId == 0)
            {
                var list = await _sqlSugarClient.Queryable<BaseAutoColorModel>().Where(x => x.ColorName == baseAutoColorModel.ColorName).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"车身颜色[{baseAutoColorModel.ColorName}]已经存在");
                }

                await _sqlSugarClient.Insertable(baseAutoColorModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<BaseAutoColorModel>().Where(x => x.ColorId == baseAutoColorModel.ColorId).ToListAsync();

                if (list.Count == 0) return;

                if (baseAutoColorModel.State == 1)
                {
                    // 修改
                    // 校验名称是否存在

                    var tempList = await _sqlSugarClient.Queryable<BaseAutoColorModel>().Where(x => x.ColorName == baseAutoColorModel.ColorName).ToListAsync();

                    // 查询到多条记录 或者查询到一条记录且不是当前记录
                    if (tempList.Count > 1 || (tempList.Count == 1 && tempList[0].ColorId != baseAutoColorModel.ColorId))
                    {
                        throw new Exception($"车身颜色[{baseAutoColorModel.ColorName}]已经存在");
                    }

                    list[0].ColorName = baseAutoColorModel.ColorName;

                    await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();

                }
                else
                {
                    // 删除
                    await _sqlSugarClient.Deleteable(list[0]).ExecuteCommandAsync();
                }
            }
        }

        public async Task SaveLicenseColor(BaseLicenseColorModel baseLicenseColorModel)
        {

            if (baseLicenseColorModel.ColorId == 0)
            {
                var list = await _sqlSugarClient.Queryable<BaseLicenseColorModel>().Where(x => x.ColorName == baseLicenseColorModel.ColorName).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"车牌颜色[{baseLicenseColorModel.ColorName}]已经存在");
                }

                await _sqlSugarClient.Insertable(baseLicenseColorModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<BaseLicenseColorModel>().Where(x => x.ColorId == baseLicenseColorModel.ColorId).ToListAsync();

                if (list.Count == 0) return;

                if (baseLicenseColorModel.State == 1)
                {
                    // 修改
                    // 校验名称是否存在

                    var tempList = await _sqlSugarClient.Queryable<BaseLicenseColorModel>().Where(x => x.ColorName == baseLicenseColorModel.ColorName).ToListAsync();

                    // 查询到多条记录 或者查询到一条记录且不是当前记录
                    if (tempList.Count > 1 || (tempList.Count == 1 && tempList[0].ColorId != baseLicenseColorModel.ColorId))
                    {
                        throw new Exception($"车牌颜色[{baseLicenseColorModel.ColorName}]已经存在");
                    }

                    list[0].ColorName = baseLicenseColorModel.ColorName;

                    await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();

                }
                else
                {
                    // 删除
                    await _sqlSugarClient.Deleteable(list[0]).ExecuteCommandAsync();
                }
            }
        }

        public async Task SavelFeeModel(BaseFeeModel baseFeeModel)
        {
            if (baseFeeModel.FeeModelId == 0)
            {
                var list = await _sqlSugarClient.Queryable<BaseFeeModel>().Where(x => x.FeeModelName == baseFeeModel.FeeModelName).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"计费模式[{baseFeeModel.FeeModelName}]已经存在");
                }

                await _sqlSugarClient.Insertable(baseFeeModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<BaseFeeModel>().Where(x => x.FeeModelId == baseFeeModel.FeeModelId).ToListAsync();

                if (list.Count == 0) return;

                if (baseFeeModel.State == 1)
                {
                    // 修改
                    // 校验名称是否存在

                    var tempList = await _sqlSugarClient.Queryable<BaseFeeModel>().Where(x => x.FeeModelName == baseFeeModel.FeeModelName).ToListAsync();

                    // 查询到多条记录 或者查询到一条记录且不是当前记录
                    if (tempList.Count > 1 || (tempList.Count == 1 && tempList[0].FeeModelId != baseFeeModel.FeeModelId))
                    {
                        throw new Exception($"计费模式[{baseFeeModel.FeeModelName}]已经存在");
                    }

                    list[0].FeeModelName = baseFeeModel.FeeModelName;

                    await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();

                }
                else
                {
                    // 删除
                    await _sqlSugarClient.Deleteable(list[0]).ExecuteCommandAsync();
                }
            }
        }

    }
}
