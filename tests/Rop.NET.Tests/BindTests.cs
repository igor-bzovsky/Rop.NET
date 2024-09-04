using FluentAssertions;
using Rop.NET.BaseTypes;
using Rop.NET.Extensions;
using Rop.NET.Tests.Common;

namespace Rop.NET.Tests
{
    using static Errors;
    using static Results.Strings;

    public class BindTests : TestsBase
    {
        [Fact]
        public void Throws_exception_when_success_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Result<string, Error>> successFunction = null!;

            sut.Invoking(res => res.Bind(successFunction))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Rethrows_success_function_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, Result<string, Error>> successFunction = _ => throw exception;

            sut.Invoking(res => res.Bind(successFunction))
                .Should()
                .Throw<InvalidOperationException>()
                .Which.Equals(exception);
        }

        [Fact]
        public void Returns_success_function_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Succeed(Success);

            var result = sut.Bind(successFunction);

            AssertSuccess(result, Success);
        }

        [Fact]
        public void Returns_success_function_failure()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Fail(DefaultError);

            var result = sut.Bind(successFunction);

            AssertFailure(result, DefaultError);
        }

        [Fact]
        public void Returns_initial_failure()
        {
            Result<Unit, Error> sut = UnitResult.Fail(UnexpectedError);
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Succeed(Success);

            var result = sut.Bind(successFunction);

            AssertFailure(result, UnexpectedError);
        }
    }

    public class BindAsyncTests : TestsBase
    {
        [Fact]
        public async Task Throws_exception_when_success_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync = null!;

            await sut.Invoking(res => res.BindAsync(successFunctionAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Rethrows_success_function_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync = _ => throw exception;

            var result = await sut.Invoking(res => res.BindAsync(successFunctionAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>();

            result.Which.Should().Be(exception);
        }

        [Fact]
        public async Task Returns_success_function_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync =
                _ => Result.Succeed<string, Error>(Success).AsTask();

            var result = await sut.BindAsync(successFunctionAsync);

            AssertSuccess(result, Success);
        }

        [Fact]
        public async Task Returns_success_function_failure()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync =
                _ => Result.Fail<string, Error>(DefaultError).AsTask();

            var result = await sut.BindAsync(successFunctionAsync);

            AssertFailure(result, DefaultError);
        }

        [Fact]
        public async Task Returns_initial_failure()
        {
            Result<Unit, Error> sut = Result.Fail(UnexpectedError);
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync =
                _ => Result.Succeed<string, Error>(Success).AsTask();

            var result = await sut.BindAsync(successFunctionAsync);

            AssertFailure(result, UnexpectedError);
        }
    }

    public class BindExtensionsTests : TestsBase
    {
        [Fact]
        public async Task Throws_exception_when_task_result_is_null()
        {
            Task<Result<Unit, Error>> sut = null!;
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Succeed(Success);

            var task = () => sut.Bind(successFunction);

            await task.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Throws_exception_when_success_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Result<string, Error>> successFunction = null!;

            // Act + Assert
            await sut.Invoking(res => res.Bind(successFunction))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Rethrows_success_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, Result<string, Error>> successFunction = _ => throw exception;

            var result = await sut.Invoking(res => res.Bind(successFunction))
                .Should()
                .ThrowAsync<InvalidOperationException>();
            result.Which.Should().Be(exception);
        }

        [Fact]
        public async Task Returns_success_function_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Succeed(Success);

            var result = await sut.Bind(successFunction);

            AssertSuccess(result, Success);
        }

        [Fact]
        public async Task Returns_success_function_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Fail(DefaultError);

            var result = await sut.Bind(successFunction);

            AssertFailure(result, DefaultError);
        }

        [Fact]
        public async Task Returns_initial_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(UnexpectedError).AsTask();
            Func<Unit, Result<string, Error>> successFunction = _ => Result.Succeed(Success);

            var result = await sut.Bind(successFunction);

            AssertFailure(result, UnexpectedError);
        }
    }

    public class BindAsyncExtensionsTests : TestsBase
    {
        [Fact]
        public async Task Throws_exception_when_task_result_is_null()
        {
            Task<Result<Unit, Error>> sut = null!;
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync = null!;

            var task = () => sut.BindAsync(successFunctionAsync);

            await task.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Throws_exception_when_success_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync = null!;

            await sut.Invoking(res => res.BindAsync(successFunctionAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Rethrows_success_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync = _ => throw exception;

            var result = await sut.Invoking(res => res.BindAsync(successFunctionAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>();
            result.Which.Should().Be(exception);
        }

        [Fact]
        public async Task Returns_success_function_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync =
                _ => Result.Succeed<string, Error>(Success).AsTask();

            var result = await sut.BindAsync(successFunctionAsync);

            AssertSuccess(result, Success);
        }

        [Fact]
        public async Task Returns_success_function_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync =
                _ => Result.Fail<string, Error>(DefaultError).AsTask();

            var result = await sut.BindAsync(successFunctionAsync);

            AssertFailure(result, DefaultError);
        }

        [Fact]
        public async Task Returns_initial_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(UnexpectedError).AsTask();
            Func<Unit, Task<Result<string, Error>>> successFunctionAsync =
                _ => Result.Succeed<string, Error>(Success).AsTask();

            var result = await sut.BindAsync(successFunctionAsync);

            AssertFailure(result, UnexpectedError);
        }
    }
}
