using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_SetAttributes_Tests
    {
        [Fact]
        public void FileSetAttributes_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.SetAttributes(null, FileAttributes.Archive);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }
    }
}
