using System;

namespace Code.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static unsafe TDest UnsafeCast<TDest>(this object value)
        {
            TypedReference sourceRef = __makeref(value);
            TDest dest = default;
            TypedReference destRef = __makeref(dest);
            *(IntPtr*)&destRef = *(IntPtr*)&sourceRef;
            return __refvalue(destRef, TDest);
        }
    }
}