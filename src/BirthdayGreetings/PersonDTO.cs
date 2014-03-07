using System;

namespace BirthdayGreetings
{
    public class PersonDto
    {
        //public string Address { get; set; }
        public string Email { get; set; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public DateTime Birthday { get; set; }
    }
}