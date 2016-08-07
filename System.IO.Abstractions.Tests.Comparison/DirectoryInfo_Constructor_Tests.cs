using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class DirectoryInfo_Constructor_Tests
    {
        private readonly ITestOutputHelper _output;

        public DirectoryInfo_Constructor_Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DirectoryInfoConstructor_DirectoryIsNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType> execute = (fs, fst) =>
            {
                if (fst == FileSystemType.Real)
                {
                    new DirectoryInfo(null);
                }
                else
                {
                    new MockDirectoryInfo((IMockFileDataAccessor) fs, null);
                }
            };

            execute.OnFileSystems(realFileSystem, mockFileSystem, _output);
        }

        [Fact]
        public void DirectoryInfoConstructor_DirectoryIsEmpty()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType> execute = (fs, fst) =>
            {
                if (fst == FileSystemType.Real)
                {
                    new DirectoryInfo(string.Empty);
                }
                else
                {
                    new MockDirectoryInfo((IMockFileDataAccessor)fs, string.Empty);
                }
            };

            execute.OnFileSystems(realFileSystem, mockFileSystem, _output);
        }
    }
}
