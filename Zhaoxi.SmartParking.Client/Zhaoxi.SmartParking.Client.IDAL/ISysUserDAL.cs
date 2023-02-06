using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface ISysUserDAL
    {
        public Task<string> Login(string userName, string password);
    }
}
