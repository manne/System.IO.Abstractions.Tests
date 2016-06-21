using System.Globalization;
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

        [Fact]
        public void FileCopy_FirstArgumentDoesNotExist()
        {
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            Action<IFileSystem, FileSystemType, FileInfoBase> execute = (fs, _, file) => fs.File.Copy("foo.txt", "bar.txt");

            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, null, null, _output);
        }

        [Fact]
        public void FileCopy_SourceDoesNotExist()
        {
            var linesToAppend = "first line Î♫";

            Func<IFileSystem, Tuple<FileInfoBase, FileInfoBase>> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var sourcePath = tempPath + "\\source.txt";
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
            Action<IFileSystem, FileSystemType, Tuple<FileInfoBase, FileInfoBase>> execute = (fs, _, tuple) => fs.File.Copy(tuple.Item1.FullName, tuple.Item2.FullName);
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }

        [Fact]
        public void FileCopy_SourceDoesExist_DestinationDoesExist()
        {
            var linesToAppend = "first line Î♫";

            Func<IFileSystem, Tuple<FileInfoBase, FileInfoBase>> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
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
            Action<IFileSystem, FileSystemType, Tuple<FileInfoBase, FileInfoBase>> execute = (fs, _, tuple) => fs.File.Copy(tuple.Item1.FullName, tuple.Item2.FullName);
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
        }
    }
}
