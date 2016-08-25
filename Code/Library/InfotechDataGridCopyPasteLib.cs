using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExpressHelper1011.Library
{
    public static partial class MyLib
    {

        public static void csRightMouseClick(this DataGridView dgv, DataGridViewCellMouseEventArgs e, ContextMenuStrip contextMenuStrip1)
        {
            if (dgv.SelectedCells.Count > 0)
                dgv.ContextMenuStrip = contextMenuStrip1;
        }

        public static void csCutSelectedSells(this DataGridView dgv)
        {
            //Copy to clipboard
            CopyToClipboard(dgv);

            //Clear selected cells
            foreach (DataGridViewCell dgvCell in dgv.SelectedCells)
                dgvCell.Value = string.Empty;
        }

        public static void csCopySelectedCells(this DataGridView dgv)
        {
            CopyToClipboard(dgv);
        }

        public static void csPasteSelectedCells(this DataGridView dgv)
        {
            //Perform paste Operation
            PasteClipboardValue(dgv);
        }

        public static void csKeyDown(this DataGridView dgv, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            CopyToClipboard(dgv);
                            break;

                        case Keys.V:
                            PasteClipboardValue(dgv);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Copy/paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public static string csGetFirstSelectedCellName(this DataGridView dgv)
        {
            DataGridViewSelectedCellCollection list = dgv.SelectedCells;
            if (list.Count == 0) return "Rid";
            int i = list[0].ColumnIndex;
            return dgv.Columns[i].Name;
        }
        public static string csGetFirstSelectedCellvalue(this DataGridView dgv)
        {
            DataGridViewSelectedCellCollection list = dgv.SelectedCells;
            int x = list[0].ColumnIndex;
            int y = list[0].RowIndex;
            return dgv[x, y].Value == null ? string.Empty : dgv[x, y].Value.ToString();
        }
      
        private static void CopyToClipboard(DataGridView dgv)
        {
            //Copy to clipboard
            DataObject dataObj = dgv.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        public static void csPasteString(this DataGridView dgv,string s)
        {
             DataGridViewSelectedCellCollection cells = dgv.SelectedCells;
            foreach (object cell in cells)
            {
                DataGridViewTextBoxCell c = cell as DataGridViewTextBoxCell;
                if (c == null) return;
                c.Value = s;
            }
        }
        private static void PasteClipboardValue(DataGridView dgv)
        {
            //Show Error if no cell is selected
            if (dgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string s = Clipboard.GetText();
            dgv.csPasteString(s);
            #region Block Copy Stuff
            //Get the clipboard value in a dictionary
            //    Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValues(Clipboard.GetText());
            //   Dictionary<int, string>.ValueCollection v = cbValue[0].Values;
            //  if (v.Count > 0)
            // {
            //    var temp = v[];

            //}
            //int iRowIndex = startCell.RowIndex;
            //foreach (int rowKey in cbValue.Keys)
            //{
            //    int iColIndex = startCell.ColumnIndex;
            //    foreach (int cellKey in cbValue[rowKey].Keys)
            //    {
            //        //Check if the index is with in the limit
            //        if (iColIndex <= dgv.Columns.Count - 1 && iRowIndex <= dgv.Rows.Count - 1)
            //        {
            //            DataGridViewCell cell = dgv[iColIndex, iRowIndex];

            //            //Copy to selected cells if 'chkPasteToSelectedCells' is checked
            //            if (cell.Selected) cell.Value = cbValue[rowKey][cellKey];
            //        }
            //        iColIndex++;
            //    }
            //    iRowIndex++;
            //} 
            #endregion
        }

        private static DataGridViewCell GetStartCell(DataGridView dgView)
        {
            //get the smallest row,column index
            if (dgView.SelectedCells.Count == 0)
                return null;

            int rowIndex = dgView.Rows.Count - 1;
            int colIndex = dgView.Columns.Count - 1;

            foreach (DataGridViewCell dgvCell in dgView.SelectedCells)
            {
                if (dgvCell.RowIndex < rowIndex)
                    rowIndex = dgvCell.RowIndex;
                if (dgvCell.ColumnIndex < colIndex)
                    colIndex = dgvCell.ColumnIndex;
            }

            return dgView[colIndex, rowIndex];
        }

        private static Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
        {
            Dictionary<int, Dictionary<int, string>> copyValues = new Dictionary<int, Dictionary<int, string>>();

            String[] lines = clipboardValue.Split('\n');

            for (int i = 0; i <= lines.Length - 1; i++)
            {
                copyValues[i] = new Dictionary<int, string>();
                String[] lineContent = lines[i].Split('\t');

                //if an empty cell value copied, then set the dictionay with an empty string
                //else Set value to dictionary
                if (lineContent.Length == 0)
                    copyValues[i][0] = string.Empty;
                else
                {
                    for (int j = 0; j <= lineContent.Length - 1; j++)
                        copyValues[i][j] = lineContent[j];
                }
            }
            return copyValues;
        }
    }
}
