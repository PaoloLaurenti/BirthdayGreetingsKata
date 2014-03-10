using System.Collections.Generic;
using System.IO;
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
            File.ReadLines(_filePath);
            return null;
        }
    }
}