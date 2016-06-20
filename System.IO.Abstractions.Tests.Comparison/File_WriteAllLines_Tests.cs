using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_WriteAllLines_Tests
    {
        private readonly ITestOutputHelper _output;

        public File_WriteAllLines_Tests(ITestOutputHelper output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            _output = output;
        }

        [Theory]
        [InlineData("|")]
        [InlineData("<<<<")]
        public void WriteAllLines_PathContainsInvalidCharacters(string path)
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem, FileSystemType> action = (fs, _) => fs.File.WriteAllLines(path, new [] { "does not matter" });

            // Assert
            action.OnFileSystems(realFileSystem, mockFileSystem, _output);
        }
    }
}
