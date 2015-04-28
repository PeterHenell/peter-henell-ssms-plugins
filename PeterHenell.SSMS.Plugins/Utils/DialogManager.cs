﻿using PeterHenell.SSMS.Plugins.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class DialogManager
    {
        public static void GetDialogInputFromUser(string question, string defaultAnswer, Action<string> okPressedCallback, Action cancelPressedcallback = null)
        {
            using (InputSingleValueForm testDialog = new InputSingleValueForm(question, defaultAnswer))
            {
                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (testDialog.ShowDialog() == DialogResult.OK)
                {
                    var t = testDialog.input_txt.Text;
                    okPressedCallback(t);
                }
                else
                {
                    // dialog was cancelled
                    if (cancelPressedcallback != null)
                    {
                        cancelPressedcallback.Invoke();
                    }
                }
            }
        }
    }
}