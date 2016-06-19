using Xunit;

namespace System.IO.Abstractions.Tests.Comparison.Utils
{
    /// <summary>
    /// The only collection definition.
    /// </summary>
    [CollectionDefinition(CollectionDefinitions.THE_TRUTH)]
    public class TheTruthCollection : ICollectionFixture<FileSystemFixture>
    {
    }

    /// <summary>
    /// Fixture to set a base directory in which the tests resides.
    /// Cleans up this directory after all the tests are run.
    /// </summary>
    public class FileSystemFixture : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="FileSystemFixture"/>.
        /// Sets the base directory to a special directory inside the local user's temporary folder.
        /// </summary>
        public FileSystemFixture()
        {
            BaseDirectory = Path.GetTempPath() + "\\sioa";
        }

        /// <summary>
        /// Gets the base directory for all the tests.
        /// </summary>
        public string BaseDirectory { get; }

        /// <summary>
        /// Deletes the base directory.
        /// </summary>
        public void Dispose()
        {
            var directory = new DirectoryInfo(BaseDirectory);
            if (directory.Exists)
            {
                directory.Delete(true);
            }
        }
    }
}
