using System;

namespace IntroductionwebservicesClient.Service.form
{
    public class StudentForm
    {
        public String name{ get; set; }

        public String courseName { get; set; }

        public bool enrolled { get; set; }

        public override string ToString()
        {
            return name + " " + courseName + " " + enrolled;
        }
    }
}
