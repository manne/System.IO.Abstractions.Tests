using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_Exists_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_Exists_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void FileExists_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Func<IFileSystem, FileSystemType, FileInfoBase, bool> execute = (fs, _, file) => fs.File.Exists(null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void FileExists_InvalidCharacters()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Func<IFileSystem, FileSystemType, FileInfoBase, bool> execute = (fs, _, file) => fs.File.Exists("|");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void FileExists_FileDoesExist()
        {
            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath + "\\willbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                result.CreateFileWithNoContent();

                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Func<IFileSystem, FileSystemType, FileInfoBase, bool> execute = (fs, _, file) => fs.File.Exists(file.FullName);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }

        [Fact]
        public void FileExists_FileDoesNoExist()
        {
            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath + "\\doesnotexist.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Func<IFileSystem, FileSystemType, FileInfoBase, bool> execute = (fs, _, file) => fs.File.Exists(file.FullName);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }
    }
}
