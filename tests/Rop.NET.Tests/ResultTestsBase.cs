using FluentAssertions;

namespace Rop.NET.Tests
{
    public abstract class ResultTestsBase
    {
        protected const string ResultParameterName = "result";
        protected const string ErrorParameterName = "error";

        protected virtual void AssertSuccessInvariants<TNewSuccess, TFailure>(Result<TNewSuccess, TFailure> result, TNewSuccess expectedValue)
        {
            result.IsSuccess().Should().BeTrue();
            result.IsFailure().Should().BeFalse();
            result.Value.Should().Be(expectedValue);
            result.Invoking(res => res.Error).Should().Throw<InvalidOperationException>();
        }

        protected virtual void AssertFailureInvariants<TNewSuccess, TFailure>(Result<TNewSuccess, TFailure> result, TFailure expectedError)
        {
            result.IsSuccess().Should().BeFalse();
            result.IsFailure().Should().BeTrue();
            result.Error.Should().Be(expectedError);
            result.Invoking(res => res.Value).Should().Throw<InvalidOperationException>();
        }
    }
}
