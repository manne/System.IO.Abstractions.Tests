using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;
using System.Text;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_AppendAllText_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_AppendAllText_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void AppendAllText_PathIsNull()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem, FileSystemType> action = (fs, _) => fs.File.AppendAllText(null, "does not matter");

            // Assert
            action.OnFileSystems(realFileSystem, mockFileSystem);
        }

        [Fact]
        public void AppendAllText_PathContainsInvalidCharacters()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem, FileSystemType> action = (fs, _) => fs.File.AppendAllText("|", "does not matter");

            // Assert
            action.OnFileSystems(realFileSystem, mockFileSystem);
        }

        [Fact]
        public void AppendAllText_WithWeirdEncoding()
        {
            var linesToAppend = "first line Î♫";

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath + "\\willbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.AppendAllText(file.FullName, linesToAppend, Encoding.UTF32);
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }

        [Fact]
        public void AppendAllText_WriteOnReadOnlyFile()
        {
            var linesToAppend = "first line Î♫";

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath + "\\willbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                result.CreateFileWithNoContent();
                result.Attributes |= FileAttributes.ReadOnly;
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.AppendAllText(file.FullName, linesToAppend, Encoding.UTF32);
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }
    }
}
