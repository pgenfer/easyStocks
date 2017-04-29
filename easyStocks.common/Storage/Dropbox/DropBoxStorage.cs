using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using EasyStocks.Dto;
using EasyStocks.Error;

namespace EasyStocks.Storage.Dropbox
{
    public class DropBoxStorage : JsonBaseStorage
    {
        private readonly string _token;
        private readonly IErrorService _errorService;

        protected virtual string StorageFileName => "easystocks.json";

        public DropBoxStorage(
            string token, 
            IErrorService errorService)
        {
            _token = token;
            _errorService = errorService;
        }

        public override async Task<PortfolioDto> LoadFromStorageAsync()
        {
            try
            {
                var hasFile = await HasDataAsync();
                if (hasFile)
                {
                    using (var client = new DropboxClient(_token))
                    {
                        var response = await client.Files.DownloadAsync($"/{StorageFileName}");
                        if (response != null)
                        {
                            var content = await response.GetContentAsStringAsync();
                            var portfolioDto = FromJson(content);
                            return portfolioDto;
                        }
                    }
                }
                return new PortfolioDto();
            }
            catch (Exception ex)
            {
                _errorService.TrackError(ex,ErrorId.CannotLoadPortfolioFromStorage);
                return new PortfolioDto();
            }
        }

        public override async Task SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                using (var client = new DropboxClient(_token))
                {
                    var content = ToJson(portfolio);
                    var commitInfo = new CommitInfo($"/{StorageFileName}", WriteMode.Overwrite.Instance,mute:true);
                    await client.Files.UploadAsync(commitInfo, new MemoryStream(Encoding.UTF8.GetBytes(content)));
                }
            }
            catch (Exception ex)
            {
                _errorService.TrackError(ex, ErrorId.CannotSavePortfolioToStorage);
            }
        }

        public override async Task<bool> HasDataAsync()
        {
            using (var client = new DropboxClient(_token))
            {
                var files = await client.Files.ListFolderAsync(string.Empty);
                var hasFile = files.Entries.Any(x => x.Name == StorageFileName);
                return hasFile;
            }
        }

        public override Task ClearAsync()
        {
            throw new NotImplementedException("Dropbox StorageType should not need Clear");
        }
    }
}
