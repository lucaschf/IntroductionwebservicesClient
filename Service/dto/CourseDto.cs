using System;
using System.Collections.Generic;

namespace IntroductionwebservicesClient.Service.dto
{
    public class CourseDto
    {
        public long id { get; set; }

        public int year { get; set; }

        public String name { get; set; }

        public List<StudentDto> students { get; set; }

        public override string ToString()
        {
            return  name+ "-" + year;
        }
    }
}
