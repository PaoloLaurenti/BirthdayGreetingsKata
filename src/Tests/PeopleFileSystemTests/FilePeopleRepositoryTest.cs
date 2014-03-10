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

        [Test]
        public void ShouldThrowExceptionWhenFileDoesNotExist()
        {
            _notExistingFilePath = @"C:\NotExistingPath";

            var sut = new FilePeopleRepository(_notExistingFilePath);

            Executing.This(() => sut.GetAll()).Should().Throw<FileNotFoundException>();
        }
    }
}
