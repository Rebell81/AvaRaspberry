using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace qBittorrent.qBittorrentApi
{
    public static class Prefix
    {
        public enum Module
        {
            auth,
            transfer,
            app
        }

        public enum Action
        {
            login,
            info,
            version,
            webapiVersion
        }

        private const string API_PREFIX = "api/v2";

        public static Uri Build(Module module, Action action)
        {
            return new Uri(
                $"{API_PREFIX}/{Enum.GetName(typeof(Module), module)}/{Enum.GetName(typeof(Action), action)}",
                UriKind.Relative);
        }
    }

    public class Api
    {

        public enum AuthResult { True, False, ConnectionError }

        private readonly HttpClient _httpClient;
        private readonly HttpClientHandler _httpClientHandler;
        private readonly ServerCredential _serverCredential;


        public Api(ServerCredential argServerCredential)
        {
            _serverCredential = argServerCredential;
            _httpClientHandler = new HttpClientHandler();
            _httpClient = new HttpClient(_httpClientHandler) { BaseAddress = _serverCredential.Uri };
        }

        private bool IsAuthenticated()
        {
            return _httpClientHandler.CookieContainer.GetCookies(_httpClient.BaseAddress)["SID"] != null;
        }

        private async Task CheckAuthentification()
        {
            if (!IsAuthenticated())
            {
                await Login();
            }
        }

        private async Task<AuthResult> Login()
        {
            try
            {
                HttpContent bodyContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", _serverCredential.Username),
                    new KeyValuePair<string, string>("password", _serverCredential.Password)
                });

                //var uri = new Uri($"{Prefix.API_PREFIX}{Prefix.AUTH_PREFIX}/login", UriKind.Relative);
                var uri = Prefix.Build(Prefix.Module.auth, Prefix.Action.login);

                await _httpClient.PostAsync(uri, bodyContent);

                return IsAuthenticated() ? AuthResult.True : AuthResult.False;
            }
            catch (HttpRequestException)
            {
                return AuthResult.ConnectionError;
            }

        }


        public async Task<TransferInfo> GetTransferInfo()
        {
            await CheckAuthentification();

            //var uriTransferInfo = new Uri($"{prefix}/transfer/info", UriKind.Relative);
            var uriTransferInfo = Prefix.Build(Prefix.Module.transfer, Prefix.Action.info);

            var jsonStr = await GetStringAsync(uriTransferInfo);

            return string.IsNullOrEmpty(jsonStr)
                ? new TransferInfo() { Result = false }
                : JsonConvert.DeserializeObject<TransferInfo>(jsonStr);
        }


        public async Task<Version> GetQBittorrentVersion()
        {
            var uriTransferInfo = Prefix.Build(Prefix.Module.app, Prefix.Action.version);

            var versionStr = await GetStringAsync(uriTransferInfo);

            return string.IsNullOrEmpty(versionStr)
              ? new Version(0, 0, 0) { Result = false }
              : Version.Parse(versionStr);
        }

        private async Task<string> GetStringAsync(Uri url, bool repeat = true)
        {
            try
            {
                return await _httpClient.GetStringAsync(url);
            }
            catch
            {
                if (repeat)
                {
                    await Login();
                    return await GetStringAsync(url, false);
                }

                return string.Empty;
            }
        }
















        public async Task<int> GetApiVersion()
        {
            var uriTransferInfo = Prefix.Build(Prefix.Module.app, Prefix.Action.webapiVersion);

            int.TryParse(await GetStringAsync(uriTransferInfo), out var intNum);

            return intNum;
        }

        public async Task<int> GetApiMinVersion()
        {
            return int.Parse(await _httpClient.GetStringAsync(new Uri("/version/api_min", UriKind.Relative)));
        }


        public async Task<IList<Torrent>> GetTorrents(Filter filter = Filter.All, string category = null)
        {
            await CheckAuthentification();

            var keyValuePairs = new KeyValuePair<string, string>[2];
            keyValuePairs.SetValue(new KeyValuePair<string, string>("filter", filter.ToString().ToLower()), 0);


            if (category != null)
            {
                keyValuePairs.SetValue(new KeyValuePair<string, string>("category", category), 1);
            }

            HttpContent content = new FormUrlEncodedContent(keyValuePairs);

            var uri = new Uri("/query/torrents?" + await content.ReadAsStringAsync(), UriKind.Relative);
            var response = await _httpClient.GetAsync(uri);
            var jsonStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IList<Torrent>>(jsonStr);
        }

        public async Task<GeneralProperties> GetGeneralProperties(string hash)
        {
            var jsonStr =
                await _httpClient.GetStringAsync(new Uri("/query/propertiesGeneral/" + hash, UriKind.Relative));
            return JsonConvert.DeserializeObject<GeneralProperties>(jsonStr);
        }

        public async Task<IList<TrackersProperties>> GetTrackersProperties(string hash)
        {
            var jsonStr =
                await _httpClient.GetStringAsync(new Uri("/query/propertiesTrackers/" + hash, UriKind.Relative));
            return JsonConvert.DeserializeObject<IList<TrackersProperties>>(jsonStr);
        }

        public async Task<IList<FilesProperties>> GetFilesProperties(string hash)
        {
            var jsonStr =
                await _httpClient.GetStringAsync(new Uri("/query/propertiesFiles/" + hash, UriKind.Relative));
            return JsonConvert.DeserializeObject<IList<FilesProperties>>(jsonStr);
        }

        public async Task<bool> DownloadFromUrls(IList<Uri> uris)
        {
            await CheckAuthentification();

            var stringBuilder = new StringBuilder();
            foreach (var uri in uris)
            {
                stringBuilder.Append(uri);
                stringBuilder.Append('\n');
            }

            HttpContent bodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("urls", stringBuilder.ToString())
            });

            var uriDownload = new Uri("/command/download", UriKind.Relative);
            var httpResponseMessage = await _httpClient.PostAsync(uriDownload, bodyContent);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Upload(byte[][] torrents)
        {
            await CheckAuthentification();

            using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now))
            {
                foreach (var torrent in torrents)
                {
                    var guid = Guid.NewGuid().ToString();
                    content.Add(new ByteArrayContent(torrent), guid, guid);
                }

                var uriUpload = new Uri("/command/upload", UriKind.Relative);
                var message = await _httpClient.PostAsync(uriUpload, content);
                return message.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeletePermanently(IList<string> hashes)
        {
            await CheckAuthentification();

            var stringBuilder = new StringBuilder();
            foreach (var hash in hashes)
            {
                stringBuilder.Append(hash);
                stringBuilder.Append('|');
            }

            HttpContent bodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("hashes", stringBuilder.ToString())
            });

            var uriDownload = new Uri("/command/deletePerm", UriKind.Relative);
            var httpResponseMessage = await _httpClient.PostAsync(uriDownload, bodyContent);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> SetCategory(IList<string> hashes, string category)
        {
            await CheckAuthentification();

            var stringBuilder = new StringBuilder();

            foreach (var hash in hashes)
            {
                stringBuilder.Append(hash);
                stringBuilder.Append('|');
            }

            HttpContent bodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("hashes", stringBuilder.ToString()),
                new KeyValuePair<string, string>("category", category)
            });

            var uriSetCategory = new Uri("/command/setCategory", UriKind.Relative);
            var httpResponseMessage = await _httpClient.PostAsync(uriSetCategory, bodyContent);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Recheck(string hash)
        {
            await CheckAuthentification();

            HttpContent bodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("hash", hash),
            });

            var uriRecheck = new Uri("/command/recheck", UriKind.Relative);
            var httpResponseMessage = await _httpClient.PostAsync(uriRecheck, bodyContent);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> AddTrackers(string hash, IList<Uri> trackers)
        {
            await CheckAuthentification();

            var stringBuilder = new StringBuilder();

            foreach (var tracker in trackers)
            {
                stringBuilder.Append(tracker);
                stringBuilder.Append('\n');
            }

            HttpContent bodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("hash", hash),
                new KeyValuePair<string, string>("urls", stringBuilder.ToString())
            });

            var uriAddTrackers = new Uri("/command/addTrackers", UriKind.Relative);
            var httpResponseMessage = await _httpClient.PostAsync(uriAddTrackers, bodyContent);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Resume(string hash)
        {
            await CheckAuthentification();

            HttpContent bodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("hash", hash),
            });

            var uriResume = new Uri("/command/resume", UriKind.Relative);
            var httpResponseMessage = await _httpClient.PostAsync(uriResume, bodyContent);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Shutdown()
        {
            await CheckAuthentification();

            var uriShutdown = new Uri("/command/shutdown", UriKind.Relative);
            var httpResponseMessage = await _httpClient.GetAsync(uriShutdown);

            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}
