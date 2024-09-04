using FluentAssertions;
using Rop.NET.BaseTypes;
using Rop.NET.Tests.Common;

namespace Rop.NET.Tests
{
    using static Errors;
    using static Results.Strings;

    public class ResultTests : ResultTestsBase
    {
        [Fact]
        public void Succeed_throws_exception_when_result_is_null()
        {
            var action = () => Result.Succeed<string, Error>(null!);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Fail_throws_exception_when_error_is_null()
        {
            var action = () => Result.Fail<string, Error>(null!);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Assert_success_invariants()
        {
            var result = Result.Succeed<string, Error>(Success);

            AssertSuccessInvariants(result, Success);
        }

        [Fact]
        public void Assert_failure_invariants()
        {
            var result = Result.Fail<string, Error>(DefaultError);

            AssertFailureInvariants(result, DefaultError);
        }
    }

    public class GenericSuccessTests : ResultTestsBase
    {
        [Fact]
        public void Succeed_throws_exception_when_result_is_null()
        {
            var action = () => Result.Succeed<string>(null!);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Assert_success_invariants()
        {
            Result<string, Error> result = Result.Succeed(Success);

            AssertSuccessInvariants(result, Success);
        }
    }

    public class GenericFailureTests : ResultTestsBase
    {
        [Fact]
        public void Fail_throws_exception_when_error_is_null()
        {
            var action = () => Result.Fail<Error>(null!);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Assert_failure_invariants()
        {
            Result<string, Error> result = Result.Fail(DefaultError);

            AssertFailureInvariants(result, DefaultError);
        }
    }
}
