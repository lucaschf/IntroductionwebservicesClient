using System;

namespace IntroductionwebservicesClient.Controller.form
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
