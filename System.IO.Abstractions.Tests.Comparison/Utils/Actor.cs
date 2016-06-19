using System.IO.Abstractions.TestingHelpers;

using FluentAssertions;

namespace System.IO.Abstractions.Tests.Comparison.Utils
{
    public static class Actor
    {
        public static void OnFileSystems(this Action<IFileSystem, FileSystemType> action, FileSystem realFileSystem, MockFileSystem mockFileSystem)
        {
            Exception realException = null;
            try
            {
                action(realFileSystem, FileSystemType.Real);
            }
            catch (Exception e)
            {
                realException = e;
            }

            Exception mockException = null;
            try
            {
                action(mockFileSystem, FileSystemType.Mock);
            }
            catch (Exception e)
            {
                mockException = e;
            }

            if (realException != null)
            {
                mockException.Should().NotBeNull("the action on the real file system fired an exception");
                // ReSharper disable PossibleNullReferenceException Reason: code can not be reached if is null
                mockException.GetType().Should().Be(realException.GetType(), "the mock file system should throw an exception of the the same type as the real file system");
                // ReSharper restore PossibleNullReferenceException
            }
        }

        public static void OnFileSystemsWithParameter<TParameter, TResult>(this Func<IFileSystem, FileSystemType, TParameter, TResult> action, FileSystem realFileSystem, MockFileSystem mockFileSystem, TParameter realParameter, TParameter mockParameter, Action<IFileSystem, TParameter> cleanAction = null)
        {
            Exception realException = null;
            TResult realResult = default(TResult);
            try
            {
                realResult = action(realFileSystem, FileSystemType.Real, realParameter);
            }
            catch (Exception e)
            {
                realException = e;
            }

            Exception mockException = null;
            TResult mockResult = default(TResult);
            try
            {
                mockResult = action(mockFileSystem, FileSystemType.Mock, mockParameter);
            }
            catch (Exception e)
            {
                mockException = e;
            }

            try
            {
                if (realException != null)
                {
                    mockException.Should().NotBeNull("the action on the real file system fired an exception");

                    // ReSharper disable PossibleNullReferenceException Reason: code can not be reached if is null
                    mockException.GetType().Should().Be(realException.GetType(), "the mock file system should throw an exception of the the same type as the real file system");
                    // ReSharper restore PossibleNullReferenceException
                }

                mockResult.Should().Be(realResult);
            }
            catch
            {
                if (cleanAction != null)
                {

                    try
                    {
                        cleanAction(realFileSystem, realParameter);
                    }
                    catch
                    {

                    }
                }

                throw;
            }
        }
    }
}