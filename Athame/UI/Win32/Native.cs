using System;
using System.Runtime.InteropServices;

namespace Athame.UI.Win32
{
    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
        // Legacy flag, should not be used.
        // ES_USER_PRESENT = 0x00000004
    }

    internal static class Native
    {
        internal const int WM_USER = 0x400;

        internal const string User32 = "user32.dll";
        internal const string Kernel32 = "kernel32.dll";

        [DllImport(User32, CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        [DllImport(Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}
