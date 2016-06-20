using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class Directory_EnumerateDirectories_Tests
    {
        [Fact]
        public void DirectoryEnumerateDirectories_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.EnumerateDirectories(null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void EnumerateDirectories2_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.EnumerateDirectories(null, "foo");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void EnumerateDirectories3_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, DirectoryInfoBase> execute = (fs, _, file) => fs.Directory.EnumerateDirectories(null, "foo", SearchOption.AllDirectories);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }
    }
}
