using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_SetAttributes_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_SetAttributes_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void FileSetAttributes_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.SetAttributes(null, FileAttributes.Archive);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void FileSetAttributes_FileDoesNotExist()
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
            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.SetAttributes(file.FullName, FileAttributes.Archive);

            // Assert
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, _output);
        }

        [Fact]
        public void FileSetAttributes_FileDoesNotExistButDirectory()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();
            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                system.Directory.CreateDirectory(tempPath);
                var realFilePath = tempPath + "\\willnotbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFile = prepare(mockFileSystem);
            var realFile = prepare(realFileSystem);

            // Act
            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.SetAttributes(file.FullName, FileAttributes.Archive);

            // Assert
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, _output);
        }
    }
}
