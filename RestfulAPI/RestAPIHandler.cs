using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;

namespace RestfulAPI
{
   public class RestAPIHandler
    {

        public  string responseContent;
        public IList<RestSharp.Parameter> responseHeader;        

        public void Invoke_REST(string AuthenticationType, String requestType, string restURL)
        {
            if(AuthenticationType == "None" && requestType == "GET")
            {
                GET_Request(restURL);
            }
        }
        public void GET_Request(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(url, DataFormat.Json);
            var response = client.Get(request);

            responseContent = response.Content;
            responseHeader = response.Headers;
           
        }
    }
}
