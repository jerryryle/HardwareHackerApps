using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CaptainHook
{
    class KeyboardHook : IDisposable
    {
        public class KeyPressedEventArgs : EventArgs
        {
            public bool TrapKey {
                get { return _TrapKey; }
                // This allows event handlers to only set TrapKey to true.
                // This ensures that if any one handler sets it to true, it remains true
                // regardless of what any other handlers do with it.
                set { if (value == true) _TrapKey = value; }
            }
            public int VKCode { get; }
            public bool IsDown { get; }
            public bool IsSysKey { get; }

            public KeyPressedEventArgs(int VKCode, bool IsDown, bool IsSysKey)
            {
                TrapKey = false;
                this.VKCode = VKCode;
                this.IsDown = IsDown;
                this.IsSysKey = IsSysKey;
            }

            private bool _TrapKey;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public bool EnableHook()
        {
            if (IsHooked)
            {
                return false;
            }

            KeyboardHookProcedure = new LowLevelKeyboardProc(HookCallback);

            HookID = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(GetType().Module), 0);

            IsHooked = true;

            return true;
        }

        public bool DisableHook()
        {
            if (!IsHooked)
            {
                return false;
            }
            if (!UnhookWindowsHookEx(HookID))
            {
                return false;
            }
            IsHooked = false;
            return true;
        }

        public KeyboardHook()
        {
        }

        ~KeyboardHook()
        {
            Dispose();
        }

        public void Dispose()
        {
            DisableHook();
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private const int HC_ACTION = 0;

        private LowLevelKeyboardProc KeyboardHookProcedure;
        private bool IsHooked = false;
        private IntPtr HookID = IntPtr.Zero;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == HC_ACTION)
            {
                int messageID = (int)wParam;
                if ((messageID == WM_KEYDOWN) || (messageID == WM_KEYUP) ||
                    (messageID == WM_SYSKEYDOWN) || (messageID == WM_SYSKEYUP))
                {
                    if (KeyPressed != null)
                    {
                        KBDLLHookStruct hookData = Marshal.PtrToStructure<KBDLLHookStruct>(lParam);
                        bool keyIsDown = ((messageID == WM_KEYDOWN) || (messageID == WM_SYSKEYDOWN));
                        bool keyIsSystemKey = ((messageID == WM_SYSKEYDOWN) || (messageID == WM_SYSKEYUP));

                        KeyPressedEventArgs e = new KeyPressedEventArgs(hookData.vkCode, keyIsDown, keyIsSystemKey);
                        KeyPressed(this, e);
                        if (e.TrapKey)
                        {
                            return (IntPtr)1;
                        }
                    }
                }
            }
            return CallNextHookEx(HookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
