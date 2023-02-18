using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;

        private readonly IConfiguration _configuration;

        public FilesController(IFilesService filesService, IConfiguration configuration)
        {
            _filesService = filesService;

            _configuration = configuration;
        }

        [HttpPost("list")]
        public async Task<Result<List<UpgradeFileModel>>> List()
        {
            var result = await _filesService.List();

            var url = _configuration["Urls"] + "/api/Files/download/";

            foreach (var item in result)
            {
                item.FilePath = url + item.FileName;
            }

            return new Result<List<UpgradeFileModel>> { IsSuccess = true, Data = result };
        }

        [HttpGet("download/{fileName}")]
        public IActionResult Download(string fileName)
        {
            // 获取更新文件地址
            var root = Path.Combine(Environment.CurrentDirectory, @"Web_Files\Upgrade_Files");

            var filePath = $@"{root}\{fileName}";

            var type = new MediaTypeHeaderValue("application/oct-stream").MediaType;

            var fs = new ResFileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fs, contentType: type, fileName, enableRangeProcessing: true);


            //var bytes = System.IO.File.ReadAllBytes(filePath);

            //return File(bytes, type, fileName);
        }

    }

    public class ResFileStream : FileStream
    {
        public ResFileStream(string path, FileMode mode, FileAccess access) : base(path, mode, access) { }

        public override int Read(byte[] array, int offset, int count)
        {
            // 文件读取限速，方便测试  3M的文件大概需要10s
            count = 2048;

            Thread.Sleep(5);

            return base.Read(array, offset, count);
        }

    }
}
