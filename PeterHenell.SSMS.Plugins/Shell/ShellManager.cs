using EnvDTE;
using EnvDTE80;
using RedGate.SIPFrameworkShared;
using System;

namespace PeterHenell.SSMS.Plugins.Shell
{
    class ShellManager
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
        internal  EditPoint GetEditPointAtBottomOfSelection()
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
        internal EditPoint GetEditPointAtTopOfSelection()
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

        internal  void AddTextToEndOfSelection(string text)
        {
            var editPoint = GetEditPointAtBottomOfSelection();
            editPoint.Insert(Environment.NewLine + text);
            
        }

        internal string GetSelectedText()
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

        internal void AddTextToTopOfSelection(string text)
        {
            var editPoint = GetEditPointAtTopOfSelection();
            editPoint.Insert(text + Environment.NewLine);
            
        }

        internal void ReplaceSelectionWith(string text)
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
    }
}
