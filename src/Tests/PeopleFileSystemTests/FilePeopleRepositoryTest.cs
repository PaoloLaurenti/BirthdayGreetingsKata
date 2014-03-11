using System;
using System.IO;
using System.Linq;
using BirthdayGreetings;
using NUnit.Framework;
using PeopleFileSystem;
using SharpTestsEx;

namespace PeopleFileSystemTests
{
    [TestFixture]
    public class FilePeopleRepositoryTest
    {
        private string _notExistingFilePath;
        private static string _emptyFilePath;
        private string _filePath;

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void ShouldThrowExceptionProvidingAllPeopleWhenFileDoesNotExist()
        {
            _notExistingFilePath = @"C:\NotExistingPath";

            var sut = new FilePeopleRepository(_notExistingFilePath);

            Executing.This(() => sut.GetAll()).Should().Throw<FileNotFoundException>();
        }

        [Test]
        public void ShouldProvideAnyPeopleWhenFileIsEmpty()
        {
            CreateEmptyFile();
            var sut = new FilePeopleRepository(_emptyFilePath);

            var people = sut.GetAll();

            people.Should().Be.Empty();

            File.Delete(_emptyFilePath);
        }

        [Test]
        public void ShouldProvideAllPeopleInFile()
        {
            _filePath = GetFilePath();
            var person1 = new Person { Birthday = new DateTime(1982, 7, 6), FirstName = "Paolo", LastName = "Laurenti", Email = "laurentipaolo@gmail.com" };
            var person2 = new Person { Birthday = new DateTime(1964, 11, 25), FirstName = "Gino", LastName = "Rossi", Email = "ginoross@libero.it" };
            CreateFile(person1, person2);
            var sut = new FilePeopleRepository(_filePath);

            var people = sut.GetAll().ToList();

            people.Count().Should().Be.EqualTo(2);
            CheckHaveSameValues(people[0], person1);
            CheckHaveSameValues(people[1], person2);

            File.Delete(_filePath);
        }

        private void CreateFile(Person person1, Person person2)
        {
            var f = File.CreateText(_filePath);
            f.WriteLine("last_name, first_name, date_of_birth, email");
            AddLine(f, person1);
            AddLine(f, person2);
            f.Flush();
            f.Close();
        }

        private static void CheckHaveSameValues(PersonDto personDto, Person person1)
        {
            personDto.FirstName = person1.FirstName;
            personDto.LastName = person1.LastName;
            personDto.Birthday = person1.Birthday;
            personDto.Email = person1.Email;
        }

        private static void AddLine(TextWriter f, Person person)
        {
            f.WriteLine(GetPersonString(person));
        }

        private static string GetPersonString(Person person)
        {
            return string.Format("{0}, {1}, {2}, {3}", person.LastName, person.FirstName, person.Birthday.ToShortDateString(), person.Email);
        }

        private static void CreateEmptyFile()
        {
            _emptyFilePath = GetFilePath();

            var emptyFile = File.Create(_emptyFilePath);
            emptyFile.Close();
        }

        private static string GetFilePath()
        {
            var tempFolderPath = Path.GetTempPath();
            return Path.Combine(tempFolderPath, Path.GetTempFileName());
        }

        private class Person
        {
            internal string LastName { get; set; }
            internal string FirstName { get; set; }
            internal DateTime Birthday { get; set; }
            internal string Email { get; set; }
        }
    }
}
