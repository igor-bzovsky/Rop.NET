namespace Rop.NET
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        public Result<TNewSuccess, TFailure> Map<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction)
        {
            _ = mapFunction ?? throw new ArgumentNullException(nameof(mapFunction));
            return MapInternal(mapFunction);
        }

        protected abstract Result<TNewSuccess, TFailure> MapInternal<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction);

        #region Success & Failure
        private sealed partial class Success : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> MapInternal<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction) => Result.Succeed(mapFunction(Value));
        }
        private sealed partial class Failure : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> MapInternal<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction) => Result.Fail(Error);
        }
        #endregion
    }

    public static class MapExtensions
    {
        public static async Task<Result<TNewSuccess, TFailure>> Map<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, TNewSuccess> mapFunction)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = mapFunction ?? throw new ArgumentNullException(nameof(mapFunction));

            var result = await taskResult.ConfigureAwait(false);
            return result.Map(mapFunction);
        }
    }
}
