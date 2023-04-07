using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.BLL
{
    public class RolesBLL : IRolesBLL
    {

        private readonly IRolesDAL _rolesDAL;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RolesBLL(IRolesDAL menusDAL)
        {
            _rolesDAL = menusDAL;

            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<bool> Save(RoleEntity roleEntity)
        {
            var str = await _rolesDAL.Save(roleEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> SaveRelation(RoleEntity roleEntity)
        {
            var str = await _rolesDAL.SaveRelation(roleEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<List<RoleEntity>> GetAll()
        {
            var str = await _rolesDAL.GetAll();

            var result = JsonSerializer.Deserialize<ResultEntity<List<RoleEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }
    }
}
