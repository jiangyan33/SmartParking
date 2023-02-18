using System;
using System.Net;
using Zhaoxi.SmartParking.Client.Upgrade.Models;

namespace Zhaoxi.SmartParking.Client.Upgrade.DataAccess
{
    public class WebDataAccess
    {

        /// <summary>
        /// 文件下载进度
        /// </summary>
        public Action<int> DownloadPrograssChanged;

        /// <summary>
        /// 文件下载完成
        /// </summary>
        public Action DownloadCompleted;

        private WebClient _webClient = new WebClient();

        public async void DownloadAsync(FileModel fileModel)
        {
            _webClient.DownloadProgressChanged += (se, ev) =>
            {
                fileModel.Progress = ev.ProgressPercentage;

                DownloadPrograssChanged?.Invoke(ev.ProgressPercentage);
            };

            _webClient.DownloadFileCompleted += (se, ev) =>
            {
                DownloadCompleted?.Invoke();
            };

            _webClient.DownloadFileAsync(new Uri(fileModel.FileUrl), fileModel.FileName);
        }
    }
}
