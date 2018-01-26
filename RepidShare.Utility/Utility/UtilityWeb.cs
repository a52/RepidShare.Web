using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RepidShare.Utility
{
    public class UtilityWeb
    {
        private string ServiceUri = CommonUtils.WebAPIURL;
        private HttpClient mdsClient = new HttpClient();

        public HttpResponseMessage GetAsync(string requestUri)
        {
            return
                Task<HttpResponseMessage>.Run(async () => { return await mdsClient.GetAsync(ServiceUri + requestUri); }).Result;
        }

        public HttpResponseMessage PostAsJsonAsync(string requestUri, object objData)
        {
            return
            Task<HttpResponseMessage>.Run(async () => { return await mdsClient.PostAsJsonAsync(ServiceUri + requestUri, objData); }).Result;

        }


        public HttpResponseMessage PutAsJsonAsync(string requestUri, object objData)
        {
            return
                   Task<HttpResponseMessage>.Run(async () => { return await mdsClient.PutAsJsonAsync(ServiceUri + requestUri, objData); }).Result;
        }
    }
}