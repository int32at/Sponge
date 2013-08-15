using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace Sponge.WebParts
{
    public class DocLibTreeViewWebPartEditor : EditorPart
    {
        private DropDownList _ddDocLibs;
        private CheckBox _cbxShowExpand;
        private CheckBox _cbxExpandAll;
        private CheckBox _cbxShowLines;

        protected override void CreateChildControls()
        {
            var tblProperties = new Table();
            Controls.Add(tblProperties);

            var row = new TableRow();

            #region row 0 

            //cell 0
            var cell = new TableCell();
            var libs = new Label {Text = "Library:"};
            cell.Controls.Add(libs);
            row.Cells.Add(cell);

            //cell 1
            var cell1 = new TableCell();
            _ddDocLibs = new DropDownList();
            var docLibraryColl = SPContext.Current.Web.GetListsOfType(SPBaseType.DocumentLibrary);
            foreach (SPList list in docLibraryColl)
            {
                _ddDocLibs.Items.Add(list.Title);
            }
            cell1.Controls.Add(_ddDocLibs);
            row.Cells.Add(cell1);

            tblProperties.Rows.Add(row);

            #endregion

            #region row 1
            var row1 = new TableRow();

            //cell 2
            var cell2 = new TableCell();
            var expand = new Label { Text = "Show Expand:" };
            cell2.Controls.Add(expand);
            row1.Cells.Add(cell2);

            //cell 3
            var cell3 = new TableCell();
            _cbxShowExpand = new CheckBox();
            cell3.Controls.Add(_cbxShowExpand);
            row1.Cells.Add(cell3);

            tblProperties.Rows.Add(row1);
            #endregion

            #region row 2
            var row2 = new TableRow();

            //cell 4
            var cell4 = new TableCell();
            var expandAll = new Label { Text = "Expand All:" };
            cell4.Controls.Add(expandAll);
            row2.Cells.Add(cell4);

            //cell 5
            var cell5 = new TableCell();
            _cbxExpandAll = new CheckBox();
            cell5.Controls.Add(_cbxExpandAll);
            row2.Cells.Add(cell5);

            tblProperties.Rows.Add(row2);
            #endregion

            #region row 3
            var row3 = new TableRow();

            //cell 6
            var cell6 = new TableCell();
            var showLines = new Label { Text = "Show Lines:" };
            cell6.Controls.Add(showLines);
            row3.Cells.Add(cell6);

            //cell 7
            var cell7 = new TableCell();
            _cbxShowLines = new CheckBox();
            cell7.Controls.Add(_cbxShowLines);
            row3.Cells.Add(cell7);

            tblProperties.Rows.Add(row3);
            #endregion

            base.CreateChildControls();
        }

        public override bool ApplyChanges()
        {
            EnsureChildControls();
            var wp = (DocLibTreeViewWebPart)WebPartToEdit;
            wp.DocLib = _ddDocLibs.SelectedItem.Value;
            wp.AutoExpand = _cbxShowExpand.Checked;
            wp.ShowLines = _cbxShowLines.Checked;
            wp.ShowExpand = _cbxExpandAll.Checked;

            return true;
        }

        public override void SyncChanges()
        {
            EnsureChildControls();
            var wp = (DocLibTreeViewWebPart)WebPartToEdit;
            if (!string.IsNullOrEmpty(wp.DocLib)) _ddDocLibs.SelectedValue = wp.DocLib;

            _cbxShowExpand.Checked = wp.AutoExpand;
            _cbxShowLines.Checked = wp.ShowLines;
            _cbxExpandAll.Checked = wp.ShowExpand;
        }
    }
}
