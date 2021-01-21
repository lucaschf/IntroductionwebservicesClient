using System;
using System.Net.Http;

namespace IntroductionwebservicesClient.Controller
{
    public abstract class BaseService
    {
        protected HttpClient GetHttpClient()
        {
            string baseUrl = "http://localhost:8080";
            return new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }
}