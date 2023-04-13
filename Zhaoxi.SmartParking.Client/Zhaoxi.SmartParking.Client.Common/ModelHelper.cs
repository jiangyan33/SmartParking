using System;
using System.Text.Json;

namespace Zhaoxi.SmartParking.Client.Common
{
    public class ModelHelper
    {
        public static T CopyModel<T>(T model) where T : new()
        {
            try
            {
                var str = JsonSerializer.Serialize(model);

                return JsonSerializer.Deserialize<T>(str);
            }
            catch (Exception ex)
            {

            }
            return new T();
        }
    }
}
