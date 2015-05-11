using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PeterHenell.SSMS.Plugins.Utils
{
    static class NotepadHelper
    {
        private class Notepad
        {
            [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            [DllImport("User32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

            public static void ShowInNotepad(string message)
            {
                var notepad = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("notepad.exe"));
                notepad.WaitForInputIdle();

                if (notepad != null)
                {
                    IntPtr child = FindWindowEx(notepad.MainWindowHandle, new IntPtr(0), "Edit", null);
                    SendMessage(child, 0x000C, 0, message);
                }
            }
        }

        public static void ShowInNotepad(string message)
        {
            Notepad.ShowInNotepad(message);
        }
    }
}
