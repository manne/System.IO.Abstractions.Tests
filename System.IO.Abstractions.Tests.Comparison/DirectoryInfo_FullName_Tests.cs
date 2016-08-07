using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class DirectoryInfo_FullName_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public DirectoryInfo_FullName_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void DirectoryInfoFullName_MultipleDirectorySeparators()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, DirectoryInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var lastIndexOf = tempPath.LastIndexOf('\\');
                tempPath = tempPath.Insert(lastIndexOf - 1, "//");
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realDirectoryPath = tempPath;
                var result = system.DirectoryInfo.FromDirectoryName(realDirectoryPath);
                return result;
            };

            var realDirectory = prepare(realFileSystem);
            var mockDirectory = prepare(mockFileSystem);
            Func<IFileSystem, FileSystemType, DirectoryInfoBase, string> execute = (fs, _, dir) => dir.FullName;

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realDirectory, mockDirectory);
        }
    }
}
