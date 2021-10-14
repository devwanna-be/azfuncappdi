using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sample.Common
{
    public interface IRESTClient
    {
        Task<string> SendAsync(SampleRequest request);
    }

    public abstract class BaseClient
    {
        protected readonly IHttpClientFactory client;
        protected BaseClient(IHttpClientFactory client)
        {
            this.client = client;
        }
    }

    public class PostClient : BaseClient, IRESTClient
    {
        public PostClient(IHttpClientFactory client) : base(client)
        { }

        public async Task<string> SendAsync(SampleRequest request)
        {
            var rest = client.CreateClient();

            rest.BaseAddress = new Uri("YOUR URI ADDRESS HERE");
            rest.DefaultRequestHeaders.Accept.Clear();

            rest.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent content = new(Utf8Json.JsonSerializer.ToJsonString(request));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            using var message = await rest.PostAsync(string.Empty, content);
            var result = await message.Content.ReadAsStringAsync();

            return result;
        }
    }
}
