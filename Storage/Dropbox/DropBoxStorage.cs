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
        private readonly ITokenProvider _tokenProvider;
        private readonly IErrorService _errorService;

        public DropBoxStorage(
            ITokenProvider tokenProvider, 
            IErrorService errorService)
        {
            _tokenProvider = tokenProvider;
            _errorService = errorService;
        }

        public override async Task<PortfolioDto> LoadFromStorageAsync()
        {
            try
            {
                using (var client = new DropboxClient(_tokenProvider.Token))
                {
                    var response = await client.Files.DownloadAsync("/easystocks.json");
                    if (response != null)
                    {
                        var content = await response.GetContentAsStringAsync();
                        var portfolioDto = FromJson(content);
                        return portfolioDto;
                    }
                    return new PortfolioDto();
                }
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
                using (var client = new DropboxClient(_tokenProvider.Token))
                {
                    var content = ToJson(portfolio);
                    var commitInfo = new CommitInfo("/easystocks.json",WriteMode.Overwrite.Instance,mute:true);
                    await client.Files.UploadAsync(commitInfo, new MemoryStream(Encoding.UTF8.GetBytes(content)));
                }
            }
            catch (Exception ex)
            {
                _errorService.TrackError(ex, ErrorId.CannotSavePortfolioToStorage);
            }
        }

        public override Task<bool> HasDataAsync()
        {
           throw new NotImplementedException("Dropbox Storage should not need HasData");
        }

        public override Task ClearAsync()
        {
            throw new NotImplementedException("Dropbox Storage should not need Clear");
        }
    }
}
