using IntroductionwebservicesClient.Service.dto;
using IntroductionwebservicesClient.Service.form;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IntroductionwebservicesClient.Service
{
    public class CourseService : BaseService 
    {
        
        public async Task<HttpResponseMessage> InsertCourseAsync(CourseForm courseForm)
        {
            string json = new JavaScriptSerializer().Serialize(courseForm);
            var response = await GetHttpClient()
                .PostAsync("courses", new StringContent(json, Encoding.UTF8, "application/json"));

            return response;
        
        }

        public async Task<List<CourseDto>> FetchAll()
        {
            HttpResponseMessage response = await GetHttpClient().GetAsync("courses");
            var courses = await response.Content.ReadAsAsync<List<CourseDto>>();

            return courses;
        }  
    }
}
