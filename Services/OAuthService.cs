using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Services
{
    public class OAuthService
    {
        public async Task<TokenResponse> GetAuthCodeAsync()
        {
            var spotifyAuthUrl = "https://accounts.spotify.com/authorize" +
            "?response_type=code" +
            "&client_id=" + "09599187f02e4e4e821c9be5915a8420" +
            "&redirect_uri=" + Uri.EscapeDataString("http://localhost:8888/callback") +
            "&scope=" + Uri.EscapeDataString("user-read-private user-read-email");

            Process.Start(new ProcessStartInfo
            {
                FileName = spotifyAuthUrl,
                UseShellExecute = true
            });

            var authorizationCode = StartHttpListener(); // this will block until the code is captured


            var client = new HttpClient();
            var requestParams = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", "http://localhost:8888/callback" },
                { "client_id", "09599187f02e4e4e821c9be5915a8420" },
                { "client_secret", "PRIVATE_KEY" }
            };

            var requestContent = new FormUrlEncodedContent(requestParams);
            var response = await client.PostAsync("https://accounts.spotify.com/api/token", requestContent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Deserialize the response (which includes access_token, refresh_token, etc.)
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString);

            return tokenResponse;
        }

        public string StartHttpListener()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8888/callback/");
            listener.Start();

            // Wait for a request
            HttpListenerContext context = listener.GetContext();
            var request = context.Request;

            // Extract the authorization code from the query string
            var authorizationCode = request.QueryString["code"];

            // Send a response to the user in the browser
            HttpListenerResponse response = context.Response;
            string responseString = "<html><body>Authorization code received. You can close this window now.</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            listener.Stop();

            return authorizationCode;
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
