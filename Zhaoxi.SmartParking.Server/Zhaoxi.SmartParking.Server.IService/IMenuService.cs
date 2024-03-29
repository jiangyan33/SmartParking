﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface IMenuService
    {
        public Task<List<MenuModel>> GetAllMenus();

        public Task<List<int>> GetMenus(int roleId);


        public Task Save(MenuModel menuModel);
    }
}
