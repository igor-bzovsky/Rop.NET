using FluentAssertions;
using Rop.NET.BaseTypes;
using Rop.NET.Extensions;
using Rop.NET.Tests.Common;

namespace Rop.NET.Tests
{
    using static Errors;
    using static Results.Strings;

    public class MapTests : TestsBase
    {
        [Fact]
        public void Throws_exception_when_map_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, string> mapFunction = null!;

            sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Rethrows_map_function_exception()
        {
            var exception = new InvalidOperationException();
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, string> mapFunction = _ => throw exception;

            sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .Throw<InvalidOperationException>()
                .Which.Equals(exception);
        }

        [Fact]
        public void Maps_success_result_to_new_success_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, string> mapFunction = _ => Success;

            var result = sut.Map(mapFunction);

            AssertSuccess(result, Success);
        }

        [Fact]
        public void Returns_initial_failure()
        {
            Result<Unit, Error> sut = UnitResult.Fail(UnexpectedError);
            Func<Unit, string> mapFunction = _ => Success;

            var result = sut.Map(mapFunction);

            AssertFailure(result, UnexpectedError);
        }
    }

    public class MapExtensionsTests : TestsBase
    {
        [Fact]
        public async Task Throws_exception_when_task_result_is_null()
        {
            Task<Result<Unit, Error>> sut = null!;
            Func<Unit, string> mapFunction = _ => Success;

            var task = () => sut.Map(mapFunction);

            await task.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Throws_exception_when_map_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, string> mapFunction = _ => null!;

            await sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Rethrows_map_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, string> mapFunction = _ => throw exception;


            var result = await sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .ThrowAsync<InvalidOperationException>();
            result.Which.Should().Be(exception);
        }

        [Fact]
        public async Task Maps_success_result_to_new_success_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, string> mapFunction = _ => Success;

            var result = await sut.Map(mapFunction.Invoke);

            AssertSuccess(result, Success);
        }

        [Fact]
        public async Task Returns_initial_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(UnexpectedError).AsTask();
            Func<Unit, string> mapFunction = _ => Success;

            var result = await sut.Map(mapFunction);

            AssertFailure(result, UnexpectedError);
        }
    }
}
