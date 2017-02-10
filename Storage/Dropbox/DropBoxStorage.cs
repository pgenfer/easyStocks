using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using EasyStocks.Dto;

namespace EasyStocks.Storage.Dropbox
{
    public class DropBoxStorage : JsonBaseStorage
    {
        private readonly ITokenProvider _tokenProvider;

        public DropBoxStorage(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
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
                // TODO: handle error here
                return new PortfolioDto();
            }
        }

        public override async Task<bool> SaveToStorageAsync(PortfolioDto portfolio)
        {
            try
            {
                using (var client = new DropboxClient(_tokenProvider.Token))
                {
                    var content = ToJson(portfolio);
                    var commitInfo = new CommitInfo("/easystocks.json",WriteMode.Overwrite.Instance,mute:true);
                    var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(Encoding.UTF8.GetBytes(content)));
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override async Task<bool> HasDataAsync()
        {
            try
            {
                using (var client = new DropboxClient(_tokenProvider.Token))
                {
                    var list = await client.Files.ListFolderAsync(string.Empty);
                    return list?.Entries.FirstOrDefault(x => x.Name == "easystocks.json") != null;
                }
            }
            catch (Exception ex)
            {
                // TODO: handle error here
                return false;
            }
        }

        public override async Task<bool> ClearAsync()
        {
            try
            {
                using (var client = new DropboxClient(_tokenProvider.Token))
                {
                    await client.Files.DeleteAsync("/easystocks.jon");
                    return true;
                }
            }
            catch (Exception ex)
            {
                // TODO: handle error here
                return false;
            }
        }
    }
}
