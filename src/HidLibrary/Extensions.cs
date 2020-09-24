using System;
using System.ComponentModel;
using System.Text;

namespace HidLibrary
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class will be removed in a future version.")]
    public static class Extensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method will be removed in a future version.")]
        public static string ToUTF8String(this byte[] buffer)
        {
            var value = Encoding.UTF8.GetString(buffer);
            return value.Remove(value.IndexOf((char)0));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method will be removed in a future version.")]
        public static string ToUTF16String(this byte[] buffer)
        {
            var value = Encoding.Unicode.GetString(buffer);
            return value.Remove(value.IndexOf((char)0));
        }
    }
}