using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class Directory_Time_Tests
    {
        [Fact]
        public void DirectoryGetCreationTime_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.GetCreationTime(null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void DirectorySetCreationTime_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.SetCreationTime(null, DateTime.Now);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }
    }
}
