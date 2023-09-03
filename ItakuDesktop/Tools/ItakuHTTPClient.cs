using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItakuDesktop.Tools
{
    public class ItakuHTTPClient
    {
        public string url;
        public string auth_token;
        public CookieContainer container;

        public ItakuHTTPClient() { }
        public ItakuHTTPClient(string url, string auth_token, CookieContainer container) 
        {
            this.url = url; this.auth_token = auth_token; this.container = container;
        }

        public Task<ItakuHTTPResponse> GetResponseAsync() => GetResponseAsync(url, auth_token, container);
        public static async Task<ItakuHTTPResponse> GetResponseAsync(string _url, string _auth_token, CookieContainer _container)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                if(_container != null) handler.CookieContainer = new CookieContainer();
                using (HttpClient client = new HttpClient(handler))
                {
                    // Set headers
                    client.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
                    client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
                    if (!string.IsNullOrWhiteSpace(_auth_token))
                        client.DefaultRequestHeaders.Add("authorization", "Token " + _auth_token);
                    client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                    client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"116\", \"Not)A;Brand\";v=\"24\", \"Microsoft Edge\";v=\"116\"");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                    client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                    client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                    client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
                    client.DefaultRequestHeaders.Add("Referer", "https://itaku.ee/home");
                    client.DefaultRequestHeaders.Add("Referrer-Policy", "strict-origin-when-cross-origin");

                    // Send the GET request
                    HttpResponseMessage response = await client.GetAsync(_url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();
                        return new ItakuHTTPResponse(true, content, response);
                    }
                    else
                    {
                        return new ItakuHTTPResponse(false, null, response);
                    }
                }
            }
        }
    }

    public class ItakuHTTPResponse
    {
        public bool isContentFetched;
        public string content;
        public HttpResponseMessage message;

        public ItakuHTTPResponse() { }
        public ItakuHTTPResponse(bool error, string text, HttpResponseMessage msg) 
        {
            isContentFetched = error;
            content = text;
            message = msg;
        }
    }
}
