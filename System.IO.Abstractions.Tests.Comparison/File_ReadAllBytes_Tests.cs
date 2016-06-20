using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;
using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_ReadAllBytes_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_ReadAllBytes_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (fileSystemFixture == null)
            {
                throw new ArgumentNullException(nameof(fileSystemFixture));
            }

            _output = output;
            _fileSystemFixture = fileSystemFixture;
        }

        [Fact]
        public void ReadAllBytes_PathIsNull()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem, FileSystemType> action = (fs, _) => fs.File.ReadAllBytes(null);

            // Assert
            action.OnFileSystems(realFileSystem, mockFileSystem);
        }

        [Fact]
        public void ReadAllBytes_FileDoesNotExist()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willnotbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };
            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Actor.CustomExceptionComparer exceptionComparer = (re, me) =>
            {
                using (new AssertionScope())
                {
                    me.GetType().Should().Be(re.GetType());
                    re.Message.Should().Match("Could not find file '*'.", "the test writer assumes that this is the message of the exception. If this assertion fails, adapt this assertion and the next");
                    me.Message.Should().Match("Could not find file '*'.");
                }
            };


            // Act
            Func<IFileSystem, FileSystemType, FileInfoBase, byte[]> execute = (fs, fst, file) =>
            {
                var l = fs.File.ReadAllBytes(file.FullName);
                return l;
            };

            // Assert
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, null, _output, exceptionComparer);
        }
    }
}
