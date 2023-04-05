using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.Common
{
    public class GlobalValue
    {
        public static SysUserEntity UserInfo { get; set; }

        /// <summary>
        /// 数据Copy
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T1 CopyData<T1, T2>(T2 entity) where T1 : new() where T2 : new()
        {
            var t1 = new T1();

            var t1PropertyList = typeof(T1).GetProperties().ToList();

            var t2PropertyList = typeof(T2).GetProperties().ToList();

            foreach (var property in t2PropertyList)
            {
                var index = t1PropertyList.FindIndex(x => x.Name == property.Name);

                if (index != -1)
                {
                    var value = property.GetValue(entity);

                    try
                    {
                        t1PropertyList[index].SetValue(t1, value);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return t1;
        }
    }
}
