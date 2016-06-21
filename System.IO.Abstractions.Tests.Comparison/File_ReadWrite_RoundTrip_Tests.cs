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
            yield return new [] { Encoding.Unicode };
        }

        [Theory]
        [MemberData(nameof(GetEncodings))]
        public void AppendAllLinesThenReadAllBytes_Encoding(Encoding encoding)
        {
            var faker = new Bogus.Faker("ko");
            var linesToAppend = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Func<IFileSystem, FileSystemType, FileInfoBase, byte[]> execute = (fs, fst, file) =>
            {
                fs.File.AppendAllLines(file.FullName, linesToAppend, encoding);
                var l = fs.File.ReadAllBytes(file.FullName);
                return l;
            };

            Actor.CustomResultComparer<byte[]> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().ContainInOrder(r, "the content in the mock file system should be the same as in the real file system");
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer, _output);
        }

        [Theory]
        [MemberData(nameof(GetEncodings))]
        public void AppendAllLinesWithEncoding_ThenReadAllLinesWithoutEncoding(Encoding encoding)
        {
            var faker = new Bogus.Faker("ko");
            var linesToAppend = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Func<IFileSystem, FileSystemType, FileInfoBase, string[]> execute = (fs, fst, file) =>
            {
                fs.File.AppendAllLines(file.FullName, linesToAppend, encoding);
                var l = fs.File.ReadAllLines(file.FullName);
                return l;
            };

            Actor.CustomResultComparer<string[]> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().ContainInOrder(r, "the content in the mock file system should be the same as in the real file system");
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer, _output);
        }

        [Theory]
        [MemberData(nameof(GetEncodings))]
        public void AppendAllLinesWithEncoding_ThenReadAllLinesWithEncoding(Encoding encoding)
        {
            var faker = new Bogus.Faker("ko");
            var linesToAppend = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Func<IFileSystem, FileSystemType, FileInfoBase, string[]> execute = (fs, fst, file) =>
            {
                fs.File.AppendAllLines(file.FullName, linesToAppend, encoding);
                var l = fs.File.ReadAllLines(file.FullName, encoding);
                return l;
            };

            Actor.CustomResultComparer<string[]> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().ContainInOrder(r, "the content in the mock file system should be the same as in the real file system");
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer, _output);
        }

        [Theory]
        [MemberData(nameof(GetEncodings))]
        public void WriteAndAppendAllLinesWithEncoding_ThenReadAllLinesWithEncoding(Encoding encoding)
        {
            var faker = new Bogus.Faker("ko");
            var linesToWrite = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };
            var linesToAppend = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Func<IFileSystem, FileSystemType, FileInfoBase, string[]> execute = (fs, fst, file) =>
            {
                fs.File.WriteAllLines(file.FullName, linesToWrite, encoding);
                fs.File.AppendAllLines(file.FullName, linesToAppend, encoding);
                var l = fs.File.ReadAllLines(file.FullName, encoding);
                return l;
            };

            Actor.CustomResultComparer<string[]> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().ContainInOrder(r, "the content in the mock file system should be the same as in the real file system");
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer, _output);
        }

        [Theory]
        [MemberData(nameof(GetEncodings))]
        public void WriteWithEncodingAndAppendAllLinesWithDifferentEncoding_ThenReadAllLinesWithEncoding(Encoding encoding)
        {
            var faker = new Bogus.Faker("ko");
            var linesToWrite = new List<string>
            {
                faker.Lorem.Lines(20, Environment.NewLine),
                faker.Lorem.Lines(20, Environment.NewLine)
            };
            var linesToAppend = new List<string>
            {
                faker.Random.Words(344),
                faker.Lorem.Lines(20, Environment.NewLine)
            };

            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Func<IFileSystem, FileSystemType, FileInfoBase, string[]> execute = (fs, fst, file) =>
            {
                fs.File.WriteAllLines(file.FullName, linesToWrite, encoding);
                fs.File.AppendAllLines(file.FullName, linesToAppend, Encoding.UTF7);
                var l = fs.File.ReadAllLines(file.FullName, encoding);
                return l;
            };

            Actor.CustomResultComparer<string[]> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().ContainInOrder(r, "the content in the mock file system should be the same as in the real file system");
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer, _output);
        }

        [Fact]
        public void WriteWithEncodingAndAppendAllLinesWithDifferentEncoding2_ThenReadAllLinesWithEncoding()
        {
            var subFolder = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            Func<IFileSystem, FileSystemType, FileInfoBase> prepare = (system, type) =>
            {
                var tempPath = system.Path.Combine(_fileSystemFixture.BaseDirectory, subFolder);
                var tempDirectory2 = system.Directory.CreateDirectory(tempPath);
                _output.WriteLine("Temporary Directory {0} ({1})", tempDirectory2.FullName, type);
                var realFilePath = system.Path.Combine(tempDirectory2.FullName, "willbecreated.txt");
                var result = system.FileInfo.FromFileName(realFilePath);
                return result;
            };

            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            var realFile = prepare(realFileSystem, FileSystemType.Real);
            var mockFile = prepare(mockFileSystem, FileSystemType.Mock);
            Func<IFileSystem, FileSystemType, FileInfoBase, byte[]> execute = (fs, fst, file) =>
            {
                fs.File.WriteAllText(file.FullName, "Demo text content");
                fs.File.AppendAllText(file.FullName, " some text", Encoding.Unicode);
                var b = fs.File.ReadAllBytes(file.FullName);
                return b;
            };

            Actor.CustomResultComparer<byte[]> comparer = (r, m) =>
            {
                if (r == null)
                {
                    m.Should().BeNull();
                }
                else
                {
                    m.Should().ContainInOrder(r, "the content in the mock file system should be the same as in the real file system");
                }
            };
            execute.OnFileSystemsWithParameter(realFileSystem, mockFileSystem, realFile, mockFile, null, comparer, _output);
        }
    }
}
