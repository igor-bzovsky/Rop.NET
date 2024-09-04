using FluentAssertions;

namespace Rop.NET.Tests
{
    public abstract class TestsBase
    {
        protected virtual void AssertSuccess<TNewSuccess, TFailure>(Result<TNewSuccess, TFailure> result, TNewSuccess expectedValue)
        {
            result.IsSuccess().Should().BeTrue();
            result.Value.Should().Be(expectedValue);
        }

        protected virtual void AssertFailure<TNewSuccess, TFailure>(Result<TNewSuccess, TFailure> result, TFailure expectedError)
        {
            result.IsFailure().Should().BeTrue();
            result.Error.Should().Be(expectedError);
        }
    }
}
