using Rop.NET.BaseTypes;

namespace Rop.NET.Tests.Common
{
    internal class Errors
    {
        public static Error DefaultError => new Error("default_error", "Error occured");
        public static Error UnexpectedError => new Error("unexpected_error", "Unexpected error occured");
    }
}
