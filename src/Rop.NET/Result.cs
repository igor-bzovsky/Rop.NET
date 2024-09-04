using Rop.NET.BaseTypes;

namespace Rop.NET
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        private Result() { }

        public bool IsSuccess() => this is Success;
        public bool IsFailure() => this is Failure;

        public abstract TSuccess Value { get; }
        public abstract TFailure Error { get; }

        public static implicit operator Result<TSuccess, TFailure>(Result.GenericSuccess<TSuccess> success) => new Success(success.Value);
        public static implicit operator Result<TSuccess, TFailure>(Result.GenericFailure<TFailure> failure) => new Failure(failure.Error);
        public static implicit operator Result<TSuccess, TFailure>(TFailure error) => new Failure(error);


        #region Success & Failure
        private sealed partial class Success : Result<TSuccess, TFailure>
        {
            public override TSuccess Value { get; }
            public override TFailure Error => throw new InvalidOperationException();
            public Success(TSuccess value) => Value = value;
        }
        private sealed partial class Failure : Result<TSuccess, TFailure>
        {
            public override TSuccess Value => throw new InvalidOperationException();
            public override TFailure Error { get; }
            public Failure(TFailure error) => Error = error;
        }
        #endregion
    }

    public static class Result
    {
        public static Result<TSuccess, TFailure> Succeed<TSuccess, TFailure>(TSuccess result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            return Succeed(result);
        }
        public static GenericSuccess<TSuccess> Succeed<TSuccess>(TSuccess result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            return new(result);
        }

        public static Result<TSuccess, TFailure> Fail<TSuccess, TFailure>(TFailure error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            return Fail(error);
        }
        public static GenericFailure<TFailure> Fail<TFailure>(TFailure error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            return new(error);
        }


        #region GenericSuccess & GenericFailure
        public readonly struct GenericSuccess<T>
        {
            internal T Value { get; }
            internal GenericSuccess(T value) => Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public readonly struct GenericFailure<T>
        {
            internal T Error { get; }
            internal GenericFailure(T error) => Error = error ?? throw new ArgumentNullException(nameof(error));
        }
        #endregion
    }
    public static class UnitResult
    {
        public static Result<Unit, TFailure> Succeed<TFailure>() => Result.Succeed(Unit.value);
        public static Result<Unit, TFailure> Fail<TFailure>(TFailure error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            return Result.Fail(error);
        }
        public static Result.GenericSuccess<Unit> Succeed() => Result.Succeed(Unit.value);
    }
}
