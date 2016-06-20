using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;

using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_Move_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_Move_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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
        public void FileMove_FirstArgumentIsNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Move(null, "foo.txt");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileMove_SecondArgumentIsNull()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Move("foo.txt", null);

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileMove_FirstArgumentHasInvalidCharacters()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Move("|", "foo.txt");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileMove_SecondArgumentHasInvalidCharacters()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Move("foo.txt", "|");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }
    }
}
