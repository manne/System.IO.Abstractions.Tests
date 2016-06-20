using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class Directory_Delete_Tests
    {
        [Fact]
        public void DirectoryDelete_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.Delete(null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void DirectoryDelete_Recursive_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.Delete(null, true);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }
    }
}
