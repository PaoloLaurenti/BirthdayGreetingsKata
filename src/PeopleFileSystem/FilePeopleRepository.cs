using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings;

namespace PeopleFileSystem
{
    public class FilePeopleRepository : PeopleRepository
    {
        private readonly string _filePath;

        public FilePeopleRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<PersonDto> GetAll()
        {
            var lines = FileUtils.ReadAllLinesWithNoLock(_filePath);

            return lines
                .Skip(1)
                .Select(l =>
                    {
                        var values = l.Split(',');
                        return CreatePersonDto(values[0], values[1], DateTime.Parse(values[2]), values[3]);
                    });
        }

        private static PersonDto CreatePersonDto(string firstName, string lastName, DateTime birthday, string email)
        {
            return new PersonDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Birthday = birthday,
                    Email = email
                };
        }
    }
}