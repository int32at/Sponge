using System.Collections.Generic;
using Microsoft.SharePoint;
using System;
using System.Linq;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Sponge.Extensions;

namespace Sponge.WebParts
{
    [ToolboxItemAttribute(false)]
    public partial class DocLibTreeViewWebPart : WebPart
    {
        [WebBrowsable(false)]
        public string DocLib { get; set; }

        [WebBrowsable(false)]
        public bool AutoExpand { get; set; }

        [WebBrowsable(false)]
        public bool ShowLines { get; set; }

        [WebBrowsable(false)]
        public bool ShowExpand { get; set; }

        private const string HtmlOpenEditorPane = @"WebPart is not configured. Click <a href=""javascript:ShowToolPane2Wrapper('Edit',this,'{0}');""title=""Open Tool Pane"">here</a> edit the properties.";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            if (string.IsNullOrEmpty(DocLib))
            {
                var lbl = new Literal {Text = string.Format(HtmlOpenEditorPane, ID)};
                Controls.Add(lbl);
                return;
            }
            var list = SPContext.Current.Web.Lists[DocLib];
            var node = new TreeNode(list.Title) {NavigateUrl = list.DefaultViewUrl};
            node = BuildTreeNode(node, list.RootFolder);

            var treeview = new TreeView {EnableViewState = true, ShowLines = ShowLines, ShowExpandCollapse = ShowExpand};

            if(AutoExpand)
                node.ExpandAll();
            else
                node.CollapseAll();

            treeview.Nodes.Add(node);
            Controls.Add(treeview);
        }

        public TreeNode BuildTreeNode(TreeNode parent, SPFolder parentFolder)
        {
            GetFiles(parentFolder).ForEach(file => parent.ChildNodes.Add(BuildItemNode(file)));
            GetFolders(parentFolder).ForEach(folder =>
            {
                if (folder.Name != "Forms") parent.ChildNodes.Add(BuildFolderNode(folder));
            });

            return parent;
        }

        public override EditorPartCollection CreateEditorParts()
        {
           var editorParts = new List<EditorPart>(1);

            var part = new DocLibTreeViewWebPartEditor {ID = ID, Title = "Settings"};
            editorParts.Add(part);
            var baseparts = base.CreateEditorParts();

            return new EditorPartCollection(baseparts, editorParts);
        }

        public List<SPFile> GetFiles(SPFolder folder)
        {
            return folder.Files.Cast<SPFile>().ToList();
        }

        public List<SPFolder> GetFolders(SPFolder folder)
        {
            return folder.SubFolders.Cast<SPFolder>().ToList();
        }

        private TreeNode BuildFolderNode(SPFolder folder)
        {
            var node = new TreeNode("  " + folder.Name) { ImageUrl = "_layouts/images/folder.gif", NavigateUrl = SPContext.Current.Web.Url + "/" + folder.Url };
            node.ChildNodes.Add(BuildTreeNode(node, folder));
            return node;
        }

        private TreeNode BuildItemNode(SPFile file)
        {
            var node = new TreeNode("  " + file.Name) {ImageUrl = "_layouts/images/" + file.IconUrl, NavigateUrl = SPContext.Current.Web.Url + "/" + file.Url, ToolTip = file.GetFileSize()};
            return node;
        }
    }
}
