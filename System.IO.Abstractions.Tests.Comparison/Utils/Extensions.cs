namespace System.IO.Abstractions.Tests.Comparison.Utils
{
    public static class Extensions
    {
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
    }
}
