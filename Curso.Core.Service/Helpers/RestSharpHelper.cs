using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Curso.Core.Service.Helpers
{
    public static class RestSharpHelper
    {
        public async static Task<IRestResponse> ExecuteRequest(string resource, object data, Method method, List<Tuple<string, string>> headers = null)
        {
            RestClient _restClient = new RestClient() { Timeout = -1 };

            var request = new RestRequest(resource, method) { RequestFormat = DataFormat.Json };
            request.AddHeader("Content-Type", "application/json");

            if (headers != null)
                foreach (var item in headers)
                    request.AddHeader(item.Item1, item.Item2);

            if (!(data is null))
                request.AddJsonBody(JsonConvert.SerializeObject(data));

            // execute the request
            IRestResponse response = await _restClient.ExecuteAsync(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            return response;
        }
    }
}
