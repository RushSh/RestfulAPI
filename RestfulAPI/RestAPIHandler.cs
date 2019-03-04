using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace RestfulAPI
{
    class RestAPIHandler
    {

        public Array Invoke_REST(string AuthenticationType, String requestType, string restURL)
        {
            if(AuthenticationType == "None" && requestType == "GET")
            {
               var result = GET_Request(restURL);
                return result ;
            }

            return null;
        }
        public Array GET_Request(string url)
        {
            var client = new RestClient("https://jsonplaceholder.typicode.com/");
            var request = new RestRequest(url, DataFormat.Json);
            var response = client.Get(request);
            return response.Headers.ToArray();
           
        }
    }
}
