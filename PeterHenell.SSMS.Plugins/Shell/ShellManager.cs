using EnvDTE;
using EnvDTE80;
using RedGate.SIPFrameworkShared;
using System;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Shell
{
    public class ShellManager
    {
        public ISsmsFunctionalityProvider4 provider { get; private set; }

        public ShellManager(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Get an EditPoint at the bottom of the selected text.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>null if no text is selected OR if no document is active</returns>
        public EditPoint GetEditPointAtBottomOfSelection()
        {
            DTE2 a = (DTE2)provider.SsmsDte2;
            Document document = a.ActiveDocument;
            if (document != null)
            {
                // find the selected text, and return the edit point at the bottom of it.
                TextSelection selection = document.Selection as TextSelection;
                if (selection == null)
                    return null;

                return selection.BottomPoint.CreateEditPoint();
            }
            return null;
        }
        public EditPoint GetEditPointAtTopOfSelection()
        {
            DTE2 a = (DTE2)provider.SsmsDte2;
            Document document = a.ActiveDocument;
            
            if (document != null)
            {
                // find the selected text, and return the edit point at the bottom of it.
                TextSelection selection = document.Selection as TextSelection;
                if (selection == null)
                    return null;

                return selection.TopPoint.CreateEditPoint();
            }
            return null;            
        }

        public void AppendToEndOfSelection(string text)
        {
            var editPoint = GetEditPointAtBottomOfSelection();
            editPoint.Insert(Environment.NewLine + text);
            
        }

        public string GetSelectedText()
        {
            var currentWindow = provider.GetQueryWindowManager();
            var selectedText = "";

            try
            {
                selectedText = currentWindow.GetActiveAugmentedQueryWindowContents();
            }
            catch (Exception)
            {
                throw new Exception("There is no open query window. Open a query window, enter and select any query, then try running again.");
            }

            if (string.IsNullOrWhiteSpace(selectedText))
            {
                throw new Exception("There is no selection made. Select one or more queries, then try running again.");
            }
            return selectedText;
        }

        public void AddTextToTopOfSelection(string text)
        {
            var editPoint = GetEditPointAtTopOfSelection();
            editPoint.Insert(text + Environment.NewLine);
            
        }

        public void ReplaceSelectionWith(string text)
        {
            DTE2 a = (DTE2)provider.SsmsDte2;
            Document document = a.ActiveDocument;
            TextSelection selection = document.Selection as TextSelection;
            selection.Delete();

            var editPoint = GetEditPointAtTopOfSelection();
            var endPoint = GetEditPointAtTopOfSelection();
            editPoint.Delete(endPoint);
            editPoint.Insert(text + Environment.NewLine);
        }

        public static void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }
    }
}
