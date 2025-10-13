using Newtonsoft.Json;
using SecShare.Core.BaseClass;
using SecShare.DocumentAPI.Services.IService;
using SecShare.Helper.Utils;
using System.Text;

namespace SecShare.DocumentAPI.Services;

public class UServiceConnect : IUServiceConnect
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public UServiceConnect(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<ResponseDTO> GetUsersShared(IEnumerable<string> userIds)
    {
        try
        {


            var client = _httpClientFactory.CreateClient("User");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");


            message.RequestUri = new Uri($"{SD.AuthAPIBase}/api/user/getUsersShared");
            message.Content = new StringContent(JsonConvert.SerializeObject(userIds), Encoding.UTF8, "application/json");

            message.Method = HttpMethod.Get;

            HttpResponseMessage? apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            return apiResponseDto;
        }catch (Exception e)
        {
            return new ResponseDTO
            {
                IsSuccess = false,
                Message = e.Message
            };
        }
    }
}
