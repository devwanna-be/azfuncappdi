using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Sample.Common;

namespace Sample.Function
{
    public class Function1
    {
        private readonly IHttpClientFactory client;
        public Function1(IHttpClientFactory client)
        {
            this.client = client;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            var logger = executionContext.GetLogger("Function1");

            logger.LogInformation("C# HTTP trigger function processed a request.");
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            try
            {
                IRESTClient rest = new PostClient(client);
                SampleRequest request = new()
                {
                    Name = "Sample Name",
                    NRP = "Sample NRP"
                };

                response.WriteString(await rest.SendAsync(request));
            }
            catch (System.Exception ex)
            {
                logger.LogInformation(ex.Message);
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.WriteString("An exception has occured");
            }

            return response;
        }
    }
}
