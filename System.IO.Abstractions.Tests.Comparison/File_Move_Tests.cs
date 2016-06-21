using System.Globalization;
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

        [Fact]
        public void FileMove_SourceDoesExist_DestinationDoesExist()
        {
            var subFolderPath = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, Tuple<FileInfoBase, FileInfoBase>> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolderPath);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var sourcePath = tempPath + "\\source.txt";
                var source = system.FileInfo.FromFileName(sourcePath);
                source.CreateFileWithNoContent();
                var destinationPath = tempPath + "\\destination.txt";
                var destination = system.FileInfo.FromFileName(destinationPath);
                destination.CreateFileWithNoContent();
                return Tuple.Create(source, destination);
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Action<IFileSystem, FileSystemType, Tuple<FileInfoBase, FileInfoBase>> execute = (fs, _, tuple) => fs.File.Move(tuple.Item1.FullName, tuple.Item2.FullName);
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }

        [Fact]
        public void FileMove_SourceDoesNotExist_DestinationDoesExist()
        {
            var subFolderPath = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, Tuple<FileInfoBase, FileInfoBase>> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolderPath);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var sourcePath = tempPath + "\\sourceNotExists.txt";
                var source = system.FileInfo.FromFileName(sourcePath);
                var destinationPath = tempPath + "\\destination.txt";
                var destination = system.FileInfo.FromFileName(destinationPath);
                destination.CreateFileWithNoContent();
                return Tuple.Create(source, destination);
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Action<IFileSystem, FileSystemType, Tuple<FileInfoBase, FileInfoBase>> execute = (fs, _, tuple) => fs.File.Move(tuple.Item1.FullName, tuple.Item2.FullName);
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }
    }
}
