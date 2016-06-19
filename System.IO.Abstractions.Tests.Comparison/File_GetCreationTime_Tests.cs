using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_GetCreationTime_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_GetCreationTime_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void GetCreationTime_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Func<IFileSystem, FileSystemType, FileInfoBase, DateTime> execute = (fs, _, file) => fs.File.GetCreationTime(null);

            Actor.CustomResultComparer<DateTime> ignoreComparison = (_, __) => { };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, null, ignoreComparison);
        }

        [Fact]
        public void GetCreationTime_FileDoesExist()
        {
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath  + "\\mustexist.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                system.File.AppendAllText(result.FullName, "foo");
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Func<IFileSystem, FileSystemType, FileInfoBase, DateTime> execute = (fs, _, file) => fs.File.GetCreationTime(file.FullName);

            Actor.CustomResultComparer<DateTime> ignoreComparison = (_, __) => { };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, ignoreComparison);
        }

        [Fact]
        public void GetCreationTime_FileDoesNotExist()
        {
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath + "\\mustnotexist.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Func<IFileSystem, FileSystemType, FileInfoBase, DateTime> execute = (fs, _, file) => fs.File.GetCreationTime(file.FullName);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }
    }
}
