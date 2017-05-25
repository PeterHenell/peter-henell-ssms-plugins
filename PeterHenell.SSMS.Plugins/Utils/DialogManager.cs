using PeterHenell.SSMS.Plugins.Forms;
using System;
using System.Collections.Generic;
using System.IO;
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

        public class InputWithCheckboxesDialogManager<T> where T : Dictionary<string, bool>
        {
            public void Show(
                                    string question,
                                    string defaultAnswer,
                                    T options,
                                    Action<string, T> okPressedCallback,
                                    Action cancelPressedcallback = null)
            {
                using (TextBoxAndCheckboxesForm<T> testDialog = new TextBoxAndCheckboxesForm<T>(question, defaultAnswer, options))
                {
                    // Show testDialog as a modal dialog and determine if DialogResult = OK.
                    if (testDialog.ShowDialog() == DialogResult.OK)
                    {
                        var t = testDialog.input_txt.Text;
                        var checkedBoxes = testDialog.CheckedOptions;
                        okPressedCallback(t, checkedBoxes);
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



        public static FileInfo ShowExcelSaveFileDialog(string filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*")
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = filter;

            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK)
                return null;

            return new FileInfo(saveFileDialog.FileName);
        }

        public static FileInfo ShowSelectFileDialog(string filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*")
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = filter;

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK)
                return null;

            if (!File.Exists(openFileDialog.FileName))
                return null;

            return new FileInfo(openFileDialog.FileName);
        }

        public static FileInfo ShowSaveFileDialog(string filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*")
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = filter;

            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK)
                return null;

            return new FileInfo(saveFileDialog.FileName);
        }
    }
}
