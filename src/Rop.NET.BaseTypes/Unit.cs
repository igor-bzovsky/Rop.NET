namespace Rop.NET.BaseTypes
{
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit value;

        public bool Equals(Unit other) => true;
        public override bool Equals(object? obj) => obj is Unit;
        public override int GetHashCode() => 0;
        public static bool operator ==(Unit left, Unit right) => left.Equals(right);
        public static bool operator !=(Unit left, Unit right) => !(left == right);
        public override string ToString() => "()";

    }

    public static class Extension
    {
        public static async Task<Unit> AsUnitTask(this Task task)
        {
            await task;
            return Unit.value;
        }

        public static Func<TResult, Unit> AsUnitFunc<TResult>(this Action<TResult> action)
        {
            return result =>
            {
                action(result);
                return Unit.value;
            };
        }

        public static Func<Unit, Unit> AsUnitFunc(this Action action)
        {
            return _ =>
            {
                action();
                return Unit.value;
            };
        }
    }
}
