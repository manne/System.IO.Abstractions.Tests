using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Abstractions;

namespace System.IO.Abstractions.Tests.Comparison.Utils
{
    public static class Actor
    {
        public delegate void CustomResultComparer<T>(T realParameter, T mockParameter);
        public delegate void CustomExceptionComparer(Exception realException, Exception mockException);

        public static void OnFileSystems(this Action<IFileSystem, FileSystemType> action, FileSystem realFileSystem, MockFileSystem mockFileSystem, ITestOutputHelper outputHelper = null)
        {
            OnFileSystemsWithParameter<object, object>((fileSystem, fileSystemType, _) =>
            {
                action(fileSystem, fileSystemType);
                return null;
            }, realFileSystem, mockFileSystem, null, null, null, null, outputHelper);
        }

        public static void OnFileSystemsWithParameter<TParameter>(this Action<IFileSystem, FileSystemType, TParameter> action, FileSystem realFileSystem, MockFileSystem mockFileSystem, TParameter realParameter, TParameter mockParameter, ITestOutputHelper outputHelper = null)
        {
            OnFileSystemsWithParameter<TParameter, object>((fileSystem, fileSystemType, parameter) =>
            {
                action(fileSystem, fileSystemType, parameter);
                return null;
            }, realFileSystem, mockFileSystem, realParameter, mockParameter, null, null, outputHelper);
        }

        public static void OnFileSystemsWithParameter<TParameter, TResult>(this Func<IFileSystem, FileSystemType, TParameter, TResult> function, FileSystem realFileSystem, MockFileSystem mockFileSystem, TParameter realParameter, TParameter mockParameter, Action<IFileSystem, TParameter> cleanAction = null, CustomResultComparer<TResult> customResultComparer = null, ITestOutputHelper outputHelper = null, CustomExceptionComparer exceptionComparer = null)
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
                    if (exceptionComparer != null)
                    {
                        exceptionComparer(realException, mockException);
                    }
                    else
                    {
                        Action<string, Exception> writeException = (message, exception) => outputHelper?.WriteLine("{0} exception\nType: {1}\nMessage:\n{2}\n----------------", message, exception.GetType().FullName, exception.Message);
                        writeException("Real", realException);
                        mockException.Should().NotBeNull("the interaction with the real file system fired an exception");

                        // ReSharper disable PossibleNullReferenceException Reason: code can not be reached if is null
                        using (new AssertionScope())
                        {
                            writeException("Mock", mockException);
                            mockException.Should().BeOfType(realException.GetType(), "the corresponding interaction on the mock file system should throw an exception of the the same type as the real file system");
                            mockException.Message.Should().StartWith(realException.Message);
                        }
                        // ReSharper restore PossibleNullReferenceException
                    }
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