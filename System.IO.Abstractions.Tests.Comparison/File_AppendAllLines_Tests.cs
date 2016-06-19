using System.IO.Abstractions.TestingHelpers;

using FluentAssertions;
using Xunit;

namespace System.IO.Abstractions.Tests.Comparison
{
    public class File_AppendAllLines_Tests
    {
        [Fact]
        public void PathContainsInvalidCharacters()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var realFileSystem = new FileSystem();

            // Act
            Action<IFileSystem> action = fs => fs.File.AppendAllLines("|", new[] {"does not matter"});
            Exception realException = null;
            try
            {
                action(realFileSystem);
            }
            catch (Exception e)
            {
                realException = e;
            }

            Exception mockException = null;
            try
            {
                action(mockFileSystem);
            }
            catch (Exception e)
            {
                mockException = e;
            }

            // Assert
            mockException.Should().BeOfType(realException.GetType());
        }
    }
}
