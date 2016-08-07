using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class Path_GetFullPath_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public Path_GetFullPath_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
        {
            _output = output;
            _fileSystemFixture = fileSystemFixture;
        }

        [Fact]
        public void PathGetFullPath_MultipleDirectorySeparators()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            Func<IFileSystem, FileSystemType, string> execute = (fs, _) =>
            {
                var tempPath = fs.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                tempPath = tempPath + @"\\/////aaa";
                return fs.Path.GetFullPath(tempPath);
            };

            execute.OnFileSystems(realFileSystem, mockFileSystem, _output);
        }
    }
}
