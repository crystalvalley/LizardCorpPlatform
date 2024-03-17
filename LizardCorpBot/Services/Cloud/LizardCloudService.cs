namespace LizardCorpBot.Services.Cloud
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord.Rest;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;
    using RestSharp;

    /// <summary>
    /// 클라우드 관련 서비스.
    /// </summary>
    /// <param name="config">The <see cref="IConfiguration"/> that should be inject.</param>
    public class LizardCloudService(IConfiguration config)
    {
        private readonly IConfiguration _configuration = config;
        private readonly RestClient _client = new(config["LizardCloudUrl"]!);
        private string _authHeader = string.Empty;

        private async Task RefreshAuthHeader()
        {
            RestRequest request = new("/api2/auth-token/", Method.Post)
            {
                AlwaysMultipartFormData = true,
            };
            request.AddParameter("username", _configuration["admin_id"]);
            request.AddParameter("password", _configuration["admin_pw"]);
            RestResponse response = await _client.ExecuteAsync(request);
            if (response.Content is null) return;
            _authHeader = JObject.Parse(response.Content)["token"]!.ToString();
        }

        public async void GetAccount()
        {
            await RefreshAuthHeader();
            RestRequest request = new("/api2/account/info/", Method.Get)
            {
                AlwaysMultipartFormData = true,
            };
            request.AddHeader("Authorization", $"Token {_authHeader}");
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
        }

        public async Task AddUser(string email)
        {
            await RefreshAuthHeader();
            RestRequest request = new("/api/v2.1/admin/users/", Method.Post);
            request.AddParameter("email", email);
            request.AddParameter("password", "lizard_password");
            request.AddParameter("is_active", true);
            request.AddParameter("is_staff", false);
            request.AddParameter("quota_total", 10 * 1024);
            request.AddHeader("Authorization", $"Token {_authHeader}");
            await _client.ExecuteAsync(request);
        }
    }
}
