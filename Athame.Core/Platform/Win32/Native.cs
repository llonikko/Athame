using System;
using System.Runtime.InteropServices;

namespace Athame.Core.Platform.Win32
{
    [Flags]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
        // Legacy flag, should not be used.
        // ES_USER_PRESENT = 0x00000004
    }

    public static class Native
    {
        public const int WM_USER = 0x400;

        internal const string User32 = "user32.dll";
        internal const string Kernel32 = "kernel32.dll";

        [DllImport(User32, CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport(Native.User32)]
        public static extern bool ReleaseCapture();

        [DllImport(Native.User32)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        public uint cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }
}
