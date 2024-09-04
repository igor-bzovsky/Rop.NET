using Rop.NET.Extensions;

namespace Rop.NET
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        public Result<TNewSuccess, TFailure> Bind<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> successFunction)
        {
            _ = successFunction ?? throw new ArgumentNullException(nameof(successFunction));
            return BindInternal(successFunction);
        }

        public async Task<Result<TNewSuccess, TFailure>> BindAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> successFunctionAsync)
        {
            _ = successFunctionAsync ?? throw new ArgumentNullException(nameof(successFunctionAsync));
            return await BindInternalAsync(successFunctionAsync).ConfigureAwait(false);
        }

        protected abstract Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> successFunction);

        protected abstract Task<Result<TNewSuccess, TFailure>> BindInternalAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> successFunctionAsync);


        #region Success & Failure
        private sealed partial class Success : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> successFunction) => successFunction(Value);

            override protected async Task<Result<TNewSuccess, TFailure>> BindInternalAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> successFunctionAsync) => await successFunctionAsync(Value).ConfigureAwait(false);
        }
        private sealed partial class Failure : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> successFunction) => Result.Fail(Error);

            override protected async Task<Result<TNewSuccess, TFailure>> BindInternalAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> successFunctionAsync)
                => await Result.Fail<TNewSuccess, TFailure>(Error).AsTask().ConfigureAwait(false);
        }
        #endregion
    }

    public static class BindExtensions
    {
        public static async Task<Result<TNewSuccess, TFailure>> Bind<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Result<TNewSuccess, TFailure>> successFunction)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = successFunction ?? throw new ArgumentNullException(nameof(successFunction));

            var result = await taskResult.ConfigureAwait(false);
            return result.Bind(successFunction);
        }

        public static async Task<Result<TNewSuccess, TFailure>> BindAsync<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> successFunctionAsync)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = successFunctionAsync ?? throw new ArgumentNullException(nameof(successFunctionAsync));

            var result = await taskResult.ConfigureAwait(false);
            return await result.BindAsync(successFunctionAsync).ConfigureAwait(false);
        }
    }
}
