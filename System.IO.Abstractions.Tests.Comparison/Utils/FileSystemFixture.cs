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
    // ReSharper disable ClassNeverInstantiated.Global Reason: the class is initialized by xunit
    public sealed class FileSystemFixture : IDisposable
    // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="FileSystemFixture"/>.
        /// Sets the base directory to a special directory inside the local user's temporary folder.
        /// </summary>
        public FileSystemFixture()
        {
            BaseDirectory = Path.GetTempPath() + "sioa";
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
            // only the real file system must be cleaned up
            var directory = new DirectoryInfo(BaseDirectory);
            try
            {
                DeleteRecursiveFolder(directory);
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        private void DeleteRecursiveFolder(DirectoryInfo directory)
        {
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                DeleteRecursiveFolder(subDirectory);
            }

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }

            directory.Delete();
        }
    }
}
