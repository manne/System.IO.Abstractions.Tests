using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    public class File_GetCreationTime_Tests
    {
        private readonly ITestOutputHelper _output;

        public File_GetCreationTime_Tests(ITestOutputHelper output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            _output = output;
        }

        [Fact]
        public void FileDoesExist()
        {
            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(system.Path.GetTempPath(), "sioa", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath  + "\\mustexist.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                system.File.AppendAllText(result.FullName, "foo");
                return result;
            };

            Action<FileInfoBase> clean = file => file.Directory.Delete(true);

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            FileInfoBase realFile = null;
            try
            {
                realFile = prepare(realFileSystem);
                var mockFile = prepare(mockFileSystem);
                Func<IFileSystem, FileSystemType, FileInfoBase, bool> execute = (fs, _, file) => fs.File.Exists(file.FullName);

                execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile);
            }
            finally
            {
                // only the real file system must be cleaned
                clean(realFile);
            }
        }
    }
}
