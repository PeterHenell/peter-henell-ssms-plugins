using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.SqlStudio.Explorer;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using PeterHenell.SSMS.Plugins.Shell.Integration;
using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class NodeManager
    {
        //private QueryEditorExtender queryEditorExtender;
        private ObjectExplorerExtender objectExplorerExtender;
        //private WindowManager windowManager;
        private DTE2 applicationObject;
        private EnvDTE.AddIn addInInstance;
        //private CommandBarButton openViewerButton;

        public static ObjectExplorerNodeDescriptorBase CurrentNode { get; set; }


        public NodeManager()
        {
        }

        //public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        public void OnConnection(object addInInst)
        {
            addInInstance = (AddIn)addInInst;
            applicationObject = (DTE2)addInInstance.DTE;

            //switch (connectMode)
            //{
            //    case ext_ConnectMode.ext_cm_Startup:

            //        Commands2 commands = (Commands2)applicationObject.Commands;

            //        CommandBar menuBarCommandBar = ((CommandBars)applicationObject.CommandBars)["MenuBar"];
            //        CommandBar standardCommandBar = ((CommandBars)applicationObject.CommandBars)["Standard"];

            //        this.openViewerButton = AddCommandBarButton(commands,
            //                                                    standardCommandBar,
            //                                                    "OpenInternalsViewer",
            //                                                    "Open Internals Viewer",
            //                                                    Properties.Resources.allocationMapIcon,
            //                                                    Properties.Resources.allocationMapIconMask);

            //        CommandBarPopup commandBarPopup = (CommandBarPopup)menuBarCommandBar.Controls.Add(MsoControlType.msoControlPopup,
            //                                                                                          System.Type.Missing,
            //                                                                                          System.Type.Missing,
            //                                                                                          8,
            //                                                                                          Properties.Resources.AppWindow);
            //        commandBarPopup.Caption = "Internals Viewer";

            //        AddCommandBarPopup(commands,
            //                           commandBarPopup,
            //                           "AllocationMap",
            //                           "Allocation Map",
            //                           "Show the Allocation Map",
            //                           Properties.Resources.allocationMapIcon,
            //                           Properties.Resources.allocationMapIconMask);

            //        AddCommandBarPopup(commands,
            //                           commandBarPopup,
            //                           "TransactionLog",
            //                           "Display Transaction Log",
            //                           "Include the Transaction Log with query results",
            //                           Properties.Resources.TransactionLogIcon,
            //                           Properties.Resources.allocationMapIconMask);

            //IObjectExplorerEventProvider provider = ServiceCache.GetObjectExplorer().GetService(typeof(IObjectExplorerEventProvider)) as IObjectExplorerEventProvider;
            //http://sqlblog.com/blogs/jonathan_kehayias/archive/2009/08/22/sql-2008-r2-breaks-ssms-addins.aspx
            //ObjectExplorerService objExplorerService = (ObjectExplorerService)ServiceCache.ServiceProvider;
                //.GetService(typeof(IObjectExplorerService));
            // Test to get using name instead of array position
            //ContextService cs = (ContextService)objExplorerService.Container.Components["ContextService"];
            //cs.ObjectExplorerContext.CurrentContextChanged += new NodesChangedEventHandler(Provider_SelectionChanged);
            ////cs.ObjectExplorerContext.ItemsRefreshed 

            //provider.NodesRefreshed += new NodesChangedEventHandler(Provider_NodesRefreshed);
            //provider.NodesAdded += new NodesChangedEventHandler(Provider_NodesRefreshed);
            //provider.BufferedNodesAdded += new NodesChangedEventHandler(Provider_NodesRefreshed);

            //        this.windowManager = new WindowManager(applicationObject, addInInstance);
            //        this.queryEditorExtender = new QueryEditorExtender(applicationObject, this.windowManager);

            //        break;
            //}
        }

        private void Provider_SelectionChanged(object sender, NodesChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void TreeView_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (e.Node.FullPath.Substring(e.Node.FullPath.LastIndexOf(@"\") + 1).StartsWith("Indexes"))
            {
                string tableName = e.Node.Parent.Text;
                int tableImageIndex = e.Node.Parent.ImageIndex;

                string databaseName = e.Node.Parent.Parent.Parent.Text;

                // Wait for the async node expand to finish or we could miss indexes
                while ((e.Node as HierarchyTreeNode).Expanding)
                {
                    Application.DoEvents();
                }

                foreach (TreeNode node in e.Node.Nodes)
                {
                    // TODO: Should we add anything here?



                    //if (node.Text != "(Heap)")
                    //{
                    //    string connectionString = GetConnectionString(node);

                    //    AddIndexPageNodes(connectionString, node, databaseName, tableName, NodeName(node), 1);
                    //}
                }
            }
        }


        /// <summary>
        /// Gets the object explorer tree view.
        /// </summary>
        /// <returns></returns>
        private TreeView GetObjectExplorerTreeView()
        {
            throw new NotImplementedException("Need to fix getObjectExplorer first");
            //Type t = ServiceCache.GetObjectExplorer().GetType();

            //FieldInfo field = t.GetField("tree", BindingFlags.NonPublic | BindingFlags.Instance);

            //if (field != null)
            //{
            //    return (TreeView)field.GetValue(ServiceCache.GetObjectExplorer());
            //}
            //else
            //{
            //    return null;
            //}
        }
    }
}
