using System;

namespace Zhaoxi.SmartParking.Client.Entity
{
    public class ResultEntity<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
