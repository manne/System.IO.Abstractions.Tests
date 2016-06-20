using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_Copy_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_Copy_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void FileCopy_FirstArgumentIsNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Copy(null, null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileCopy_SecondArgumentIsNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Copy("foo.txt", null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileCopy_FirstArgumentHasInvalidCharacters()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Copy("|", "foo.txt");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileCopy_SecondArgumentHasInvalidCharacters()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Copy("foo.txt", "|");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }
    }
}
