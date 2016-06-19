using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;

namespace System.IO.Abstractions.Tests.Comparison.Utils
{
    public static class Actor
    {
        public delegate void CustomResultComparer<T>(T realParameter, T MockParameter);

        public static void OnFileSystems(this Action<IFileSystem, FileSystemType> action, FileSystem realFileSystem, MockFileSystem mockFileSystem)
        {
            OnFileSystemsWithParameter<object, object>((fileSystem, fileSystemType, _) =>
            {
                action(fileSystem, fileSystemType);
                return null;
            }, realFileSystem, mockFileSystem, null, null);
        }

        public static void OnFileSystemsWithParameter<TParameter>(this Action<IFileSystem, FileSystemType, TParameter> action, FileSystem realFileSystem, MockFileSystem mockFileSystem, TParameter realParameter, TParameter mockParameter)
        {
            OnFileSystemsWithParameter<TParameter, object>((fileSystem, fileSystemType, parameter) =>
            {
                action(fileSystem, fileSystemType, parameter);
                return null;
            }, realFileSystem, mockFileSystem, realParameter, mockParameter);
        }

        public static void OnFileSystemsWithParameter<TParameter, TResult>(this Func<IFileSystem, FileSystemType, TParameter, TResult> function, FileSystem realFileSystem, MockFileSystem mockFileSystem, TParameter realParameter, TParameter mockParameter, Action<IFileSystem, TParameter> cleanAction = null, CustomResultComparer<TResult> customResultComparer = null)
        {
            Exception realException = null;
            TResult realResult = default(TResult);
            try
            {
                realResult = function(realFileSystem, FileSystemType.Real, realParameter);
            }
            catch (Exception e)
            {
                realException = e;
            }

            Exception mockException = null;
            TResult mockResult = default(TResult);
            try
            {
                mockResult = function(mockFileSystem, FileSystemType.Mock, mockParameter);
            }
            catch (Exception e)
            {
                mockException = e;
            }

            try
            {
                if (realException != null)
                {
                    mockException.Should().NotBeNull("the interaction with the real file system fired an exception");

                    // ReSharper disable PossibleNullReferenceException Reason: code can not be reached if is null
                    mockException.Should().BeOfType(realException.GetType(), "the corresponding interaction on the mock file system should throw an exception of the the same type as the real file system");
                    // ReSharper restore PossibleNullReferenceException
                }

                if (customResultComparer != null)
                {
                    customResultComparer(mockResult, realResult);
                }
                else
                {
                    mockResult.Should().Be(realResult);
                }
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
                        // we are not interested in exceptions in the clean up process
                    }
                }

                throw;
            }
        }
    }
}