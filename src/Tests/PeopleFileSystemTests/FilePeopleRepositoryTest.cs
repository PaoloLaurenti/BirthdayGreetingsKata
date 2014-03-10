using System.IO;
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
    }
}
