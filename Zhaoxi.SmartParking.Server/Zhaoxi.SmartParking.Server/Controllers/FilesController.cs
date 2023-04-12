using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;
using Zhaoxi.SmartParking.Server.Models.Dto;

namespace Zhaoxi.SmartParking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;

        private readonly ISysUserService _sysUserService;

        private readonly IConfiguration _configuration;

        public FilesController(IFilesService filesService, IConfiguration configuration, ISysUserService sysUserService)
        {
            _filesService = filesService;

            _configuration = configuration;

            _sysUserService = sysUserService;
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

            if (!System.IO.File.Exists(filePath)) throw new Exception("文件不存在");

            var type = new MediaTypeHeaderValue("application/oct-stream").MediaType;

            var fs = new ResFileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fs, contentType: type, fileName, enableRangeProcessing: true);


            //var bytes = System.IO.File.ReadAllBytes(filePath);

            //return File(bytes, type, fileName);
        }

        [HttpGet("GetImage/{imgName}")]
        public IActionResult GetImage(string imgName)
        {
            // 获取更新文件地址
            var root = Path.Combine(Environment.CurrentDirectory, @"Web_Files\Images");

            var filePath = $@"{root}\{imgName}";

            if (!System.IO.File.Exists(filePath)) throw new Exception("文件不存在");

            var contentTypeDict = new Dictionary<string, string> {
                {"jpg","image/jpeg"},
                {"jpeg","image/jpeg"},
                {"jpe","image/jpeg"},
                {"png","image/png"},
                {"gif","image/gif"},
                {"ico","image/x-ico"},
                {"tif","image/tiff"},
                {"tiff","image/tiff"},
                {"fax","image/fax"},
                {"wbmp","image/nd.wap.wbmp"},
                {"rp","imagend.rn-realpix"}
            };

            var contentTypeStr = "image/jpeg";

            var imgTypeSplit = imgName.Split(',');

            var imgType = imgTypeSplit[imgTypeSplit.Length - 1].ToLower();

            if (contentTypeDict.ContainsKey(imgType))
            {
                contentTypeStr = contentTypeDict[imgType];
            }

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var bytes = new byte[fs.Length];

                fs.Read(bytes, 0, bytes.Length);

                fs.Close();

                return new FileContentResult(bytes, contentTypeStr);
            }
        }


        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] IFormCollection formCollection, [FromHeader] string md5, [FromHeader] string updatePath)
        {
            var result = new Result<long>();

            try
            {
                var fileList = (FormFileCollection)formCollection.Files;
                if (fileList.Count > 0)
                {
                    var fileName = fileList[0].FileName;

                    var root = Path.Combine(Environment.CurrentDirectory, @"Web_Files\Upgrade_Files");

                    var dir = new DirectoryInfo(root);

                    if (!dir.Exists) dir.Create();

                    using (FileStream fs = new FileStream($@"{root}\{fileName}", FileMode.Create))
                    {
                        await fileList[0].CopyToAsync(fs);

                        await fs.FlushAsync();
                    }

                    // 更新到数据库
                    var upgradeFile = new UpgradeFileModel()
                    {
                        FileName = fileName,
                        FileMD5 = md5,
                        Length = Convert.ToInt32(fileList[0].Length),
                        State = 1,
                        UpdatePath = updatePath
                    };

                    await _filesService.Save(upgradeFile);

                    result.Data = fileList.Sum(f => f.Length);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;

                result.Message = ex.Message;
            }

            Debug.WriteLine("上传完成" + result.IsSuccess);

            return Ok(result);
        }


        [HttpPost("uploadIcon/{userName}")]
        [Authorize]
        public async Task<IActionResult> UploadIcon(string userName, [FromForm] IFormCollection formCollection)
        {
            var result = new Result<long>();

            try
            {
                var fileList = (FormFileCollection)formCollection.Files;
                if (fileList.Count > 0)
                {
                    var ext = Path.GetExtension(fileList[0].FileName);

                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;

                    var root = Path.Combine(Environment.CurrentDirectory, @"Web_Files\Images");

                    var dir = new DirectoryInfo(root);

                    if (!dir.Exists) dir.Create();

                    using (FileStream fs = new FileStream($@"{root}\{fileName}", FileMode.Create))
                    {
                        await fileList[0].CopyToAsync(fs);

                        await fs.FlushAsync();
                    }

                    await _sysUserService.UpdateUserIcon(userName, fileName);

                    result.Data = fileList.Sum(f => f.Length);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;

                result.Message = ex.Message;
            }

            Debug.WriteLine("上传完成" + result.IsSuccess);

            return Ok(result);
        }

        [HttpPost("delete/{fileName}")]
        public async Task<Result<bool>> Delete(string fileName)
        {
            await _filesService.Delete(fileName);

            return new Result<bool> { IsSuccess = true, Data = true };

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

        public override void Write(byte[] array, int offset, int count)
        {
            count = 2048;

            Thread.Sleep(5);

            base.Write(array, offset, count);
        }

    }
}
