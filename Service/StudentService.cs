using IntroductionwebservicesClient.Service.dto;
using IntroductionwebservicesClient.Service.form;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IntroductionwebservicesClient.Service
{
    public class StudentService : BaseService
    {
        public async Task<HttpResponseMessage> InsertStudentAsync(StudentForm studentForm)
        {
            string json = new JavaScriptSerializer().Serialize(studentForm);
            var response = await GetHttpClient()
                .PostAsync("students", new StringContent(json, Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<List<StudentDto>> FetchByCourse(long id)
        {
            HttpResponseMessage response = await GetHttpClient().GetAsync("students/byCourse/" + id);
            var courses = await response.Content.ReadAsAsync<List<StudentDto>>();

            return courses;
        }
    }
}
