using System.IO.Abstractions.TestingHelpers;

namespace System.IO.Abstractions.Tests.Comparison.Utils
{
    /// <summary>
    /// Some extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates the file with no content.
        /// </summary>
        /// <param name="instance">The file to create.</param>
        public static void CreateFileWithNoContent(this FileInfoBase instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            using (instance.CreateText())
            {
                // nothing to do here flusing is done in disposing
            }
        }

        /// <summary>
        /// Create the current directory of <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The file system.</param>
        /// <returns>The <paramref name="instance"/>.</returns>
        public static MockFileSystem WithCurrentDirectory(this MockFileSystem instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var directory = instance.Directory.CreateDirectory(".");
            directory.Create();
            return instance;
        }
    }
}
