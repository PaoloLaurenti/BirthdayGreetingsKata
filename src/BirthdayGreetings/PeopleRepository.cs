using System.Collections.Generic;

namespace BirthdayGreetings
{
    public interface PeopleRepository
    {
        IEnumerable<PersonDto> GetAll();
    }
}