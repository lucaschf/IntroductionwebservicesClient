using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IntroductionwebservicesClient.Service
{
    class Client<E, K>
    {
        private HttpClient httpClient = new HttpClient();

        public Client()
        {
            httpClient.BaseAddress = new Uri("http://localhost:8080");
            httpClient.DefaultRequestHeaders.
        }

        public K post()
        {

        }
    }
}
