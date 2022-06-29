using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Net_Core_ServerTests.Infrastructure;

public static class HttpClientExtension
{
    public async static Task<T> ResponseTo<T>(this HttpResponseMessage message)
    {
        var body = await message.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(body);
    }
}