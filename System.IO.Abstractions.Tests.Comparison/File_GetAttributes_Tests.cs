using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_GetAttributes_Tests
    {
        private readonly ITestOutputHelper _output;

        public File_GetAttributes_Tests(ITestOutputHelper output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            _output = output;
        }

        [Fact]
        public void FileSetAttributes_ArgumentNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.GetAttributes(null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null);
        }

        [Fact]
        public void FileSetAttributes_EmptyPath()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.GetAttributes(string.Empty);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileSetAttributes_ArgumentContainsInvalidCharacters()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.GetAttributes("||");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileGetAttributes_FileDoesNotExistButDirectory()
        {
            var mockFileSystem = new MockFileSystem().WithCurrentDirectory();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.GetAttributes("doesnotexist.txt");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }
    }
}
