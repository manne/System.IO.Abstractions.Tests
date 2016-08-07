using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;
using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_WriteAllBytes_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_WriteAllBytes_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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

        [Theory]
        [InlineData("|")]
        [InlineData("<<<<")]
        public void WriteAllBytes_PathContainsInvalidCharacters(string path)
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem, FileSystemType> action = (fs, _) => fs.File.WriteAllBytes(path, new byte[] { 1 });

            // Assert
            action.OnFileSystems(realFileSystem, mockFileSystem, _output);
        }

        [Fact]
        public void WriteAllBytes_DirectoryDoesNotExist()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();
            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var realFilePath = tempPath + "\\willnotbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFile = prepare(mockFileSystem);
            var realFile = prepare(realFileSystem);

            // Act
            Action<IFileSystem, FileSystemType, FileInfoBase> action = (fs, _, f) => fs.File.WriteAllBytes(f.FullName, new byte[] { 1 });

            // Assert
            action.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, _output);
        }

        [Fact]
        public void WriteAllLines_SubDirectoryDoesNotExist()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();
            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder, "foo");
                var realFilePath = tempPath + "\\willnotbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFile = prepare(mockFileSystem);
            var realFile = prepare(realFileSystem);

            // Act
            Action<IFileSystem, FileSystemType, FileInfoBase> action = (fs, _, f) => fs.File.WriteAllBytes(f.FullName, new byte[] { 1 });

            // Assert
            action.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, _output);
        }
    }
}
