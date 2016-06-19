using System.Collections.Generic;
using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_ReadWrite_RoundTrip_Tests
    {
        private readonly ITestOutputHelper _output;
        private readonly FileSystemFixture _fileSystemFixture;

        public File_ReadWrite_RoundTrip_Tests(ITestOutputHelper output, FileSystemFixture fileSystemFixture)
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

        private static IEnumerable<Encoding[]> GetEncodings()
        {
            yield return new []{ Encoding.ASCII };
            yield return new []{ Encoding.BigEndianUnicode };
            yield return new []{ Encoding.UTF32 };
            yield return new []{ Encoding.UTF7 };
            yield return new []{ Encoding.UTF8 };
            yield return new []{ Encoding.Default };
            yield return new[] { Encoding.Unicode };
        }

        [Theory]
        [MemberData(nameof(GetEncodings))]
        public void ReadWrite_Encoding(Encoding encoding)
        {
            var faker = new Bogus.Faker("ko");
            var linesToAppend = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };

            Func<IFileSystem, FileInfoBase> prepare = system =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0}", tempDirectory2.FullName);
                var realFilePath = tempPath + "\\willbecreated.txt";
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem);
            var mockFile = prepare(mockFileSystem);
            Func<IFileSystem, FileSystemType, FileInfoBase, string> execute = (fs, fst, file) =>
            {
                fs.File.AppendAllLines(file.FullName, linesToAppend, encoding);
                var l = fs.File.ReadAllText(file.FullName);
                _output.WriteLine("Data of {0}", fst);
                _output.WriteLine(l);

                return l;
            };

            Actor.CustomResultComparer<string> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().Be(r);
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer);
        }
    }
}
