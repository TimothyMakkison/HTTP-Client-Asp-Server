using Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Senders
{
    public class TalkBack
    {
        private readonly ISender sender;

        public TalkBack(ISender sender)
        {
            this.sender = sender;
        }

        [Command("TalkBack Hello")]
        public async Task<string> Hello()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");

            HttpResponseMessage response = await sender.SendAsync(request);

            return await sender.GetResponseString(response);
        }

        [Command("TalkBack Sort")]
        public async Task<string> SortIntAsync(IEnumerable<int> parameters)
        {
            string uri = BuildQuery(parameters);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage response = await sender.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        private static string BuildQuery(IEnumerable<int> parameters)
        {
            string uri = "talkback/sort";
            var namedValues = parameters.Select(x => $"integers={x}");
            uri += "?" + string.Join('&', namedValues);
            return uri;
        }
    }
}