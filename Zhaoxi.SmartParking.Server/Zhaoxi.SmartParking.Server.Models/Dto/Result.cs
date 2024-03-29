﻿namespace Zhaoxi.SmartParking.Server.Models.Dto
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
