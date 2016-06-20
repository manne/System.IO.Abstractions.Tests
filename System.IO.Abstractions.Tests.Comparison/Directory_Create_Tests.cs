using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class Directory_Create_Tests
    {
        [Fact]
        public void DirectoryCreate_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.CreateDirectory(null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }
    }
}
