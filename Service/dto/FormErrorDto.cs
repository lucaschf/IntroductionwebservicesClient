using System;

namespace IntroductionwebservicesClient.Service.dto
{
    public class FormErrorDto
    {
        public String field { get; set; }

        public String errorMessage { get; set; }

        public override string ToString()
        {
            return field + " " + errorMessage;
        }
    }
}
