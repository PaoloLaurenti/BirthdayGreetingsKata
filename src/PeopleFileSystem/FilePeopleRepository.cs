using System.Collections.Generic;
using System.IO;
using System.Text;
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
            ReadLines();

            return new List<PersonDto>();
        }

        private IEnumerable<string> ReadLines()
        {
            var lines = new List<string>();

            if (!File.Exists(_filePath))
                throw new FileNotFoundException();

            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);
            }

            return lines;
        }
    }
}