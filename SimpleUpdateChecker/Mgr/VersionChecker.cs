using Newtonsoft.Json;
using SimpleUpdateChecker.Data;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SimpleUpdateChecker.VersionChecker
{
    class VersionChecker
    {
        public static async Task<string> GetCurrentVersionAsync()
        {
#if (!DEBUG)
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string url = Plugin.SimpleUpdatePlugin.UpdateCheckUrl;
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not 2xx
                        if (response.IsSuccessStatusCode)
                        {
                            VersionInfo version = null;
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                version = JsonConvert.DeserializeObject<VersionInfo>(result);
                                return version.NewestVersion;
                            }
                        }
                        else
                        {
                            throw new Exception($"Server response was unsuccessful: Code: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        throw new Exception($"Server response was unsuccessful Request error: {e.Message}");
                    }
                }
            }
            catch(Exception ex)
            {
                Plugin.SimpleUpdatePlugin.Log.Error($"SimpleUpdateChecker failed to get newest version for mod: {Plugin.SimpleUpdatePlugin.ModCheckName} Error: {ex.Message}");
                return string.Empty;
            }
            return string.Empty;
#else
            return "1.0.0";
#endif
        }
    }
}
