﻿using Microsoft.SqlServer.Management.Sdk.Sfc;
using global::Microsoft.SqlServer.Management.SqlStudio.Explorer;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Shell.Integration
{
        /// <summary>
        /// Extends the SSMS Object Explorer
        /// </summary>
        class ObjectExplorerExtender
        {
            private ContextMenuStrip contextMenuStrip;

            /// <summary>
            /// Initializes a new instance of the <see cref="ObjectExplorerExtender"/> class.
            /// </summary>
            /// <param name="windowManager">The window manager.</param>
            public ObjectExplorerExtender()
            {
                TreeView tree = GetObjectExplorerTreeView();

                tree.AfterExpand += new TreeViewEventHandler(TreeView_AfterExpand);
                tree.BeforeExpand += new TreeViewCancelEventHandler(TreeView_BeforeExpand);
                //tree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(Tree_NodeMouseDoubleClick);

                //tree.ImageList.Images.Add("Page", Properties.Resources.pageImage);

                contextMenuStrip = new ContextMenuStrip();
                contextMenuStrip.Items.Add("Open in Page Viewer...");
            }

            /// <summary>
            /// Handles the NodeMouseDoubleClick event of the TreeView control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
            //private void Tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
            //{
            //    if ((e.Node.Tag ?? Type.Missing).GetType() == typeof(PageAddress))
            //    {
            //        string connectionString = this.GetConnectionString(e.Node.Parent.Parent);

            //        windowManager.CreatePageViewerWindow(connectionString, new RowIdentifier((PageAddress)e.Node.Tag, 0));
            //    }
            //}


            //private TreeView GetObjectExplorerTreeView()
            //{
            //    //Type t = ServiceCache.GetObjectExplorer().GetType();

            //    //FieldInfo field = t.GetField("tree", BindingFlags.NonPublic | BindingFlags.Instance);

            //    //if (field != null)
            //    //{
            //    //    return (TreeView)field.GetValue(ServiceCache.GetObjectExplorer());
            //    //}
            //    //else
            //    //{
            //    //    return null;
            //    //}
            //    throw new NotImplementedException("All this code might work, more or less when GetObjectExplorer have been implemented");
            //}

            /// <summary>
            /// Gets the object explorer tree view.
            /// </summary>
            /// <returns></returns>
            public TreeView GetObjectExplorerTreeView()
            {
                //var Package = IServiceProvider
                var objectExplorerService = GetObjectExplorerService();
                //var objectExplorerService = (IObjectExplorerService)objectExplorerService.GetService(typeof(IObjectExplorerService));
                if (objectExplorerService != null)
                {
                    var oesTreeProperty = objectExplorerService.GetType().GetProperty("Tree", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (oesTreeProperty != null)
                        return (TreeView)oesTreeProperty.GetValue(objectExplorerService, null);
                    //else
                    //    debug_message("Object Explorer Tree property not found.");
                }
                //else
                //    debug_message("objectExplorerService == null");

                return null;
            }

            /// <summary>
            /// Handles the AfterExpand event of the TreeView control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
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
                        // TODO: Add code to open object file from disk.
                        // Here be code to be done.


                        //if (node.Text != "(Heap)")
                        //{
                        //    string connectionString = GetConnectionString(node);
                        Console.WriteLine(node.Text);
                        //    AddIndexPageNodes(connectionString, node, databaseName, tableName, NodeName(node), 1);
                        //}
                    }
                }
            }

            /// <summary>
            /// Handles the BeforeExpand event of the TreeView control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/> instance containing the event data.</param>
            private void TreeView_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
            {
                if (e.Node.Text == "Indexes")
                {
                    //string tableName = e.Node.Parent.Text;
                    //int tableImageIndex = e.Node.Parent.ImageIndex;

                    //string databaseName = e.Node.Parent.Parent.Parent.Text;
                    //string connectionString = GetConnectionString(e.Node);

                    //if (Hobt.HobtType(connectionString, databaseName, tableName) == StructureType.Heap)
                    //{
                    //    TreeNode heapNode = new TreeNode("(Heap)", tableImageIndex, tableImageIndex);
                    //    e.Node.Nodes.Add(heapNode);

                    //    AddIndexPageNodes(connectionString, heapNode, databaseName, tableName, string.Empty, e.Node.Parent.Parent.ImageIndex);
                    //}
                }
            }

            /// <summary>
            /// Gets the connection string from a node
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private string GetConnectionString(TreeNode node)
            {
                //INodeInformation service = null;
                //IServiceProvider provider = node as IServiceProvider;

                //if (provider != null)
                //{
                //    service = provider.GetService(typeof(INodeInformation)) as INodeInformation;
                //}

                //Urn urn = new Urn(service.Context);

                ////System.Diagnostics.Debug.Print(service.Connection.ConnectionString);

                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(service.Connection.ConnectionString);

                //builder.InitialCatalog = urn.GetAttribute("Name", "Database");

                //return builder.ToString();
                throw new NotImplementedException("Hm");
            }

            ///// <summary>
            ///// Adds the index page nodes.
            ///// </summary>
            ///// <param name="connectionString">The connection string.</param>
            ///// <param name="node">The node.</param>
            ///// <param name="databaseName">Name of the database.</param>
            ///// <param name="tableName">Name of the table.</param>
            ///// <param name="indexName">Name of the index.</param>
            ///// <param name="folderImageIndex">Index of the folder image.</param>
            //private void AddIndexPageNodes(string connectionString, TreeNode node, string databaseName, string tableName, string indexName, int folderImageIndex)
            //{
            //    // This suppresses the Object Explorer expand behavior
            //    ChildrenEnumerated(node, true);

            //    List<HobtEntryPoint> entryPoints = Hobt.EntryPoints(connectionString, databaseName, tableName, indexName);

            //    bool partitioned = entryPoints.Count > 1;

            //    foreach (HobtEntryPoint entryPoint in entryPoints)
            //    {
            //        TreeNode parentNode;

            //        if (partitioned)
            //        {
            //            parentNode = new TreeNode(string.Format("Partition {0}", entryPoint.PartitionNumber));
            //            parentNode.SelectedImageIndex = folderImageIndex;
            //            parentNode.ImageIndex = folderImageIndex;

            //            node.Nodes.Add(parentNode);
            //        }
            //        else
            //        {
            //            parentNode = node;
            //        }

            //        TreeNode firstIam = new TreeNode(string.Format("First IAM {0}", entryPoint.FirstIam));
            //        firstIam.SelectedImageKey = "Page";
            //        firstIam.ImageKey = "Page";
            //        firstIam.ContextMenuStrip = contextMenuStrip;
            //        firstIam.Tag = entryPoint.FirstIam;

            //        TreeNode rootPage = new TreeNode(string.Format("Root Page {0}", entryPoint.RootPage));
            //        rootPage.SelectedImageKey = "Page";
            //        rootPage.ImageKey = "Page";
            //        rootPage.ContextMenuStrip = contextMenuStrip;
            //        rootPage.Tag = entryPoint.RootPage;

            //        TreeNode firstPage = new TreeNode(string.Format("First Page {0}", entryPoint.FirstPage));
            //        firstPage.SelectedImageKey = "Page";
            //        firstPage.ImageKey = "Page";
            //        firstPage.ContextMenuStrip = contextMenuStrip;
            //        firstPage.Tag = entryPoint.FirstPage;

            //        parentNode.Nodes.Add(firstIam);
            //        parentNode.Nodes.Add(rootPage);
            //        parentNode.Nodes.Add(firstPage);
            //    }
            //}

            /// <summary>
            /// Get the Node Name property
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private static string NodeName(TreeNode node)
            {
                Type t = node.GetType();
                PropertyInfo property = t.GetProperty("NodeName", typeof(string));

                if (property != null)
                {
                    return Convert.ToString(property.GetValue(node, null));
                }
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Sets the childrenEnumerated field for a node
            /// </summary>
            /// <param name="node">The node.</param>
            /// <param name="enumerated">if set to <c>true</c> [enumerated].</param>
            /// <remarks>
            /// This is to suppress an error when SSMS assumes the nodes we add are HierarchyTreeNodes as opposed to 
            /// bog standard TreeNodes.
            /// </remarks>
            private static void ChildrenEnumerated(TreeNode node, bool enumerated)
            {
                Type t = node.GetType();
                FieldInfo field = t.GetField("childrenEnumerated", BindingFlags.NonPublic | BindingFlags.Instance);

                if (field != null)
                {
                    field.SetValue(node, enumerated);
                }
            }

            // http://stackoverflow.com/questions/13999352/ssms-2012-addin-objectexplorerservice-not-available-in-ssmsaddindenali
            public IObjectExplorerService GetObjectExplorerService()
            {
                /* Microsoft.SqlServer.Management.UI.VSIntegration.ServiceCache
                 * is from SqlPackageBase.dll and not from Microsoft.SqlServer.SqlTools.VSIntegration.dll
                 * the code below just throws null exception if you have wrong reference */

                var objExplorerService = (ObjectExplorerService)ServiceCache.ServiceProvider.GetService(typeof(IObjectExplorerService));
                return objExplorerService;

                ////http://sqlblog.com/blogs/jonathan_kehayias/archive/2009/08/22/sql-2008-r2-breaks-ssms-addins.aspx
                //var objExplorerService = ServiceCache.ServiceProvider.GetService(typeof(IObjectExplorerService));
                //// Test to get using name instead of array position
                //ContextService cs = (ContextService)objExplorerService.Container.Components["ContextService"];

                //// Add events to the explorerContext when it have been found
                //cs.ObjectExplorerContext.CurrentContextChanged += new NodesChangedEventHandler(Provider_SelectionChanged);


                //INavigationContextProvider provider = cs.ObjectExplorerContext;
                //provider.ItemsRefreshed.Event += ItemsRefreshed_Event;
                
                //int count = objExplorerService.Container.Components.Count;

                //int nodeCount; INodeInformation[] nodes;

                //objExplorerService.GetSelectedNodes(out nodeCount, out nodes);
                //count = nodeCount; count = nodes.Length;
                //count = objExplorerService.Container.Components.Count;
                //ContextService contextService;
                //try
                //{
                //    contextService = (ContextService)objExplorerService.Container.Components[1];
                //}
                //catch (Exception ex)
                //{
                //    contextService = (ContextService)objExplorerService.Container.Components[0];
                //}
                //INavigationContextProvider provider = contextService.ObjectExplorerContext;
                //provider.CurrentContextChanged += new NodesChangedEventHandler(ObjectExplorerContext_CurrentContextChanged);
                //return null;
            }

            //private void ItemsRefreshed_Event(object sender, NodesChangedEventArgs args)
            //{
            //    args.ChangedNodes.Add()
            //}

            //private void Provider_SelectionChanged(object sender, NodesChangedEventArgs args)
            //{
            //    Console.WriteLine(args.ChangedNodes);
            //}
        }
    

}
