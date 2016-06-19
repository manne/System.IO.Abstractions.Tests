using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions.Tests.Comparison.Utils;
using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    [Collection(CollectionDefinitions.THE_TRUTH)]
    public class File_AppendAllLines_Tests
    {
        [Fact]
        public void PathContainsInvalidCharacters()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem, FileSystemType> action = (fs, _) => fs.File.AppendAllLines("|", new[] {"does not matter"});

            // Assert
            action.OnFileSystems(realFileSystem, mockFileSystem);
        }
    }

    public enum FileSystemType
    {
        Real = 0,
        Mock = 1
    }
}
