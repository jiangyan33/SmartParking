using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
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

        public FilesController(IFilesService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("list")]
        public async Task<Result<List<UpgradeFileModel>>> List()
        {
            var result = await _filesService.List();

            return new Result<List<UpgradeFileModel>> { IsSuccess = true, Data = result };
        }

        [HttpGet("download/{fileName}")]
        public IActionResult Download(string fileName)
        {
            // 获取更新文件地址
            var root = Path.Combine(Environment.CurrentDirectory, @"Web_Files\Upgrade_Files");

            var filePath = $@"{root}\{fileName}";

            var type = new MediaTypeHeaderValue("application/oct-stream").MediaType;

            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, type, fileName);
        }
    }
}
