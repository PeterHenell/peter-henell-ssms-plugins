using EnvDTE;
using EnvDTE80;
using RedGate.SIPFrameworkShared;

namespace PeterHenell.SSMS.Plugins
{
    class ShellManager
    {
        /// <summary>
        /// Get an EditPoint at the bottom of the selected text.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>null if no text is selected OR if no document is active</returns>
        internal static EditPoint GetEditPointAtBottomOfSelection(ISsmsFunctionalityProvider4 provider)
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
    }
}
