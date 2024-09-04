namespace Rop.NET.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T obj)
        {
            _ = obj ?? throw new ArgumentNullException(nameof(obj));
            return Task.FromResult(obj);
        }
    }
}
