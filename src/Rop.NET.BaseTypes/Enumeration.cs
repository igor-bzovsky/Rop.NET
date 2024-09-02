using System.Reflection;

namespace Rop.NET.BaseTypes
{
    public abstract class Enumeration : IComparable, IEquatable<Enumeration>
    {
        public int Value { get; private set; }
        public string DisplayName { get; private set; }

        protected Enumeration(int value, string displayName) => (Value, DisplayName) = (value, displayName);

        public override string ToString() => DisplayName;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public static T GetByValue<T>(int value) where T : Enumeration
            => GetAll<T>().First(x => x.Value == value);

        public static T GetByDisplayName<T>(string dispayName) where T : Enumeration
            => GetAll<T>().First(x => x.DisplayName.Equals(dispayName));

        public bool Equals(Enumeration? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualsCore(other);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return EqualsCore((Enumeration)obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(object? obj) => obj == null ? 1 : Value.CompareTo(((Enumeration)obj).Value);

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return false;
            return ReferenceEquals(left, null) || left.Equals(right!);
        }

        public static bool operator !=(Enumeration left, Enumeration right) => !(left == right);

        private bool EqualsCore(Enumeration other) => Value.Equals(other.Value);
    }
}
