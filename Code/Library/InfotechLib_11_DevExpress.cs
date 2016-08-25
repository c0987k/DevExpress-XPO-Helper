using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSpellChecker;

// must have an XtraGrid dll solution and a XtraSpell checker dll in references

namespace ExpressHelper1011.Library
{
    public struct sGridColumnSort
    {
        public string Name { get; set; }
        public ColumnSortOrder Order { get; set; }
    }

    public static partial class MyLib
    {
        // sender is a Context Menu
        public static ComboBoxEdit GetSelectedComboBox(object contextMenu)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)contextMenu;
            ContextMenuStrip strip = (ContextMenuStrip)item.Owner;
            var cbt = strip.SourceControl.Parent;
            var cb = cbt as ComboBoxEdit;
            return cb;
        }
#if !xDATALAYER
        /// <summary>
        /// /////////////////////////////
        /// </summary>
        /// Spell Checker must have 

        public static void spellChecker1_CheckCompleteFormShowing(object sender, DevExpress.XtraSpellChecker.FormShowingEventArgs e)
        {
            e.Handled = true;
        }
        private static void spellChecker1_OptionsFormShowing(object sender, DevExpress.XtraSpellChecker.FormShowingEventArgs e)
        {
            e.Handled = true;
        }
        private static void spellChecker1_SpellingFormShowing(object sender, DevExpress.XtraSpellChecker.SpellingFormShowingEventArgs e)
        {
            e.Handled = true;
        }
        /// defined

        /// <param name="textedit"></param>
        /// <param name="spellChecker"></param>
        public static void csSpellCheck(this TextEdit textedit, SpellChecker spellChecker)
        {
            spellChecker.SpellCheckMode = SpellCheckMode.AsYouType;
            spellChecker.CheckAsYouTypeOptions.Color = Color.Magenta;
            spellChecker.Check(textedit);
            spellChecker.SetShowSpellCheckMenu(textedit, true);
        }
        public static void csSpellCheck(this TextBox tb, SpellChecker spellChecker)
        {
            spellChecker.SpellCheckMode = SpellCheckMode.AsYouType;
            spellChecker.CheckAsYouTypeOptions.Color = Color.Magenta;
            spellChecker.Check(tb);
            spellChecker.SetShowSpellCheckMenu(tb, true);
        }
        public static void LoadSpellCheckDictionaries(SpellChecker sc, string currentDirectory)
        {
            sc.csLoadSpellCheckDictionaries(currentDirectory);
        }
        public static void csLoadSpellCheckDictionaries(this SpellChecker sc, string currentDirectory = "")
        {
            //    new DevExpress.XtraSpellChecker.FormShowingEventHandler
            //sc.SpellingFormShowing += new DevExpress.XtraSpellChecker.SpellingFormShowingEventHandler(spellChecker1_SpellingFormShowing);
            //sc.OptionsFormShowing += new DevExpress.XtraSpellChecker.FormShowingEventHandler(spellChecker1_OptionsFormShowing);
            //sc.CheckCompleteFormShowing += new DevExpress.XtraSpellChecker.FormShowingEventHandler(spellChecker1_CheckCompleteFormShowing);
            sc.SpellingFormShowing += spellChecker1_SpellingFormShowing;
            sc.OptionsFormShowing += spellChecker1_OptionsFormShowing;
            sc.CheckCompleteFormShowing += spellChecker1_CheckCompleteFormShowing;
            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = MyLib.GetRelativeFolder() + @"\Spelling";
            }
            SpellCheckerOpenOfficeDictionary openOfficeDictionaryEnglish = new SpellCheckerOpenOfficeDictionary();
            string filename = currentDirectory + @"\en_US.dic";
            if (!File.Exists(filename)) "Bad Name".csTell();
            openOfficeDictionaryEnglish.DictionaryPath = filename;

            filename = currentDirectory + @"\en_US.aff";
            if (!File.Exists(filename)) "Bad Name".csTell();
            openOfficeDictionaryEnglish.GrammarPath = filename;

            openOfficeDictionaryEnglish.Culture = new CultureInfo("en-US");
            sc.Dictionaries.Add(openOfficeDictionaryEnglish);

            SpellCheckerCustomDictionary customDictionary = new SpellCheckerCustomDictionary();
            filename = currentDirectory + @"\EnglishAlphabet.txt";
            if (!File.Exists(filename)) "Bad Name".csTell();
            customDictionary.AlphabetPath = filename;

            customDictionary.Culture = CultureInfo.InvariantCulture;
            sc.Dictionaries.Add(customDictionary);

            sc.Culture = CultureInfo.InvariantCulture;
            sc.SpellCheckMode = SpellCheckMode.AsYouType;

        }
#endif
        public class TUpdownZoom
        {
            protected DomainUpDown UpDown;
            protected ZoomTrackBarControl TrackBar;
            public TUpdownZoom(DomainUpDown ud, ZoomTrackBarControl tbar)
            {
                UpDown = ud;
                UpDown.SelectedItemChanged += UpDown_SelectedItemChanged;
                UpDown.Leave += UpDown_Leave;
                TrackBar = tbar;
                TrackBar.Properties.Maximum = UpDown.Items.Count - 1;
                TrackBar.Properties.Minimum = 0;
                TrackBar.ValueChanged += TrackBar_ValueChanged;
                if (UpDown.Items.Count > 0) UpDown.SelectedIndex = 0;
            }
            protected void UpDown_Leave(object sender, EventArgs e)
            {
                int value = UpDown.Text.csToInteger();
                for (int index = 0; index < UpDown.Items.Count; index++)
                {
                    if (value == (int)UpDown.Items[index])
                    {
                        UpDown.SelectedIndex = index;
                        UpDown_SelectedItemChanged(null, null);
                        break;
                    }
                }
            }
            protected bool Working = false;
            protected virtual void UpDown_SelectedItemChanged(object sender, EventArgs e)
            {
                if (Working) return;
                Working = true;
                int index = TrackBar.Properties.Minimum + UpDown.SelectedIndex;
                TrackBar.Value = ((index <= TrackBar.Properties.Maximum) && (index > TrackBar.Properties.Minimum))
                                     ? index - 1
                                     : TrackBar.Properties.Minimum;
                Working = false;
            }
            protected virtual void TrackBar_ValueChanged(object sender, EventArgs e)
            {
                if (Working) return;
                Working = true;
                int index = TrackBar.Value;
                UpDown.SelectedIndex = index < UpDown.Items.Count ? index : -1;
                Working = false;
            }

        }
        public class TUpdownZoomDecending : TUpdownZoom
        {
            public TUpdownZoomDecending(DomainUpDown ud, ZoomTrackBarControl tbar)
                : base(ud, tbar)
            {
                //UpDown = ud;
                //UpDown.SelectedItemChanged += UpDown_SelectedItemChanged;
                //UpDown.Leave += UpDown_Leave;
                //TrackBar = tbar;
                //TrackBar.ValueChanged += TrackBar_ValueChanged;
            }
            protected override void UpDown_SelectedItemChanged(object sender, EventArgs e)
            {
                if (Working) return;
                Working = true;
                int index = TrackBar.Properties.Maximum - UpDown.SelectedIndex;
                TrackBar.Value = ((index <= TrackBar.Properties.Maximum) && (index >= TrackBar.Properties.Minimum))
                                     ? index
                                     : TrackBar.Properties.Minimum;
                Working = false;
            }
            protected override void TrackBar_ValueChanged(object sender, EventArgs e)
            {
                if (Working) return;
                Working = true;
                int index = TrackBar.Properties.Maximum - TrackBar.Value;
                UpDown.SelectedIndex = index < UpDown.Items.Count ? index : -1;
                Working = false;
            }
        }

        public static void csSetUpGrid(this GridControl grid)
        {
            grid.Visible = true;
            SetUpGrid(grid.DefaultView as GridView);

        }
        public static void csInitialize(this GridView view)
        {
            view.BestFitMaxRowCount = 100;
            view.BestFitColumns();
        }
        public static List<T> csGetAllRows<T>(this GridControl grid)
        { 
            List<T>list = new List<T>();

            GridView view = (GridView)grid.Views[0];
            for (int i = 0; i < view.RowCount; i++)
            {
                list.Add((T)view.GetRow(i));
            }
            return list;
        }
        public static List<T> csGetSelectedRows<T>(this GridControl grid)
        {
            GridView view = (GridView)grid.Views[0];
            int[] sr = view.GetSelectedRows();
            List<object> list = sr.Select(view.GetRow).ToList();
            return list.Select(k => (T)k).ToList();
        }
        public static object csGetCurrentRow(this GridControl grid)
        {
            return GetCurrentRow(grid);
        }
        public static T csGetCurrentRow<T>(this GridControl grid)
        {
            return (T)GetCurrentRow(grid);
        }
        //public static void SetDevViewSortSeq(GridView view, string colName, ColumnSortOrder columnSortOrder)
        //{
        //    csSetDevViewSortSeq(view, colName, columnSortOrder);
        //}
        public static void csSetDevViewSortSeq(this GridControl grid, string colName, ColumnSortOrder columnSortOrder)
        {
            var temp = (GridView)grid.DefaultView;
            temp.csSetDevViewSortSeq(colName, columnSortOrder);
        }

        public static void csSetDevViewSortSeq(this GridView view, string colName, ColumnSortOrder columnSortOrder)
        {
            var temp = new List<GridColumnSortInfo>()
                           {
                               new GridColumnSortInfo(view.Columns[colName], columnSortOrder)
                           }
                  .ToArray();
            view.SortInfo.ClearAndAddRange(temp);
        }
        public static void csSetDevViewSortSeq(this GridControl grid, List<sGridColumnSort> columns)
        {
            var temp = new List<GridColumnSortInfo>();
            var view = (GridView)grid.DefaultView;
            foreach (var col in columns)
            {
                temp.Add(new GridColumnSortInfo(view.Columns[col.Name], col.Order));
            }
            view.SortInfo.ClearAndAddRange(temp.ToArray());
        }
        public static object GetCurrentRow(GridControl grid, int viewNumber)
        {
            GridView view;
            if (grid.Views.Count > viewNumber)
            {
                view = (GridView)grid.Views[viewNumber];
            }
            else
            {
                view = (GridView)grid.Views[0];
            }
            if (view.FocusedRowHandle > -1)
            {
                return view.GetRow(view.FocusedRowHandle);
            }
            else
            {
                return null;
            }
        }
        public static object GetCurrentRowbyRH(GridView view, int rowHandle)
        {

            return view.GetRow(view.FocusedRowHandle);

        }

        public static string LoadGridLayout(string p, GridView gridView1)
        {
            OpenFileDialog form = new OpenFileDialog();
            string result = string.Empty;
            form.AddExtension = true;
            form.DefaultExt = "XML";
            form.Filter = "XML File|*.xml|All Files|*.*";
            form.InitialDirectory = p;
            form.Title = "Load Grid Layout File";
            if (form.ShowDialog() == DialogResult.OK)
            {
                result = Path.GetDirectoryName(form.FileName);
                gridView1.RestoreLayoutFromXml(form.FileName);
            }
            return result;
        }
        public static string cSaveGridLayout(string p, GridView gridView1)
        {
            SaveFileDialog form = new SaveFileDialog();
            string result = string.Empty;
            form.AddExtension = true;
            form.DefaultExt = "XML";
            form.Filter = "XML File|*.xml|All Files|*.*";
            form.InitialDirectory = p;
            form.Title = "Save Grid Layout";
            if (form.ShowDialog() == DialogResult.OK)
            {
                result = Path.GetDirectoryName(form.FileName);
                gridView1.SaveLayoutToXml(form.FileName);
            }
            return result;
        }
        public static List<string> csGridToCSV(this GridControl grid)
        {
            return GridToCSV(grid);
        }

        public static List<string> GridToCSV(DevExpress.XtraGrid.GridControl grid)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view =
                (DevExpress.XtraGrid.Views.Grid.GridView)grid.DefaultView;
            Dictionary<int, string> Titles = new Dictionary<int, string>();
            List<string> CSV = new List<string>();
            StringBuilder sb = new StringBuilder();
            Dictionary<int, int> rowSequence = new Dictionary<int, int>();
            for (int i = 0; i < view.Columns.Count; i++)
            {
                if (view.Columns[i].Visible)
                {
                    int k = view.Columns[i].VisibleIndex;
                    rowSequence[k] = i;
                }
            }
            for (int key = 0; key < rowSequence.Count; key++)
            {
                int i = rowSequence[key];
                Titles.Add(i, view.Columns[i].Caption);
                sb.Append(string.Format("\"{0}\",", Titles[i]));

            }
            CSV.Add(RemoveComma(sb.ToString()));
            int dataRowCount = view.DataRowCount;
            for (int i = 0; i < dataRowCount; i++)
            {
                sb.Length = 0;
                for (int key = 0; key < rowSequence.Count; key++)
                {
                    int k = rowSequence[key];
                    DevExpress.XtraGrid.Columns.GridColumn col = view.Columns[k];
                    var cellValue = view.GetRowCellValue(i, col); //GetRowCellDisplayText(i, col,);
                    if (cellValue != null)
                    {
                        if (cellValue is DateTime)
                        {
                            DateTime dt = (DateTime)cellValue;
                            sb.Append(string.Format("\"{0}\",", dt.ToShortDateString()));

                        }
                        else
                        {
                            sb.Append(string.Format("\"{0}\",", cellValue.ToString()));
                        }
                    }
                    else
                    {
                        sb.Append(",");
                    }
                }
                CSV.Add(RemoveComma(sb.ToString()));
            }
            return CSV;
        }

        public static string RemoveComma(string p)
        {
            return p.Substring(0, p.Length - 1);
        }
        public static string WriteStringListToFile(string filename, List<string> CSV)
        {
            File.Delete(filename);
            StreamWriter file = new StreamWriter(filename);
            foreach (string s in CSV)
            {
                file.WriteLine(s);
            }
            file.Flush();
            file.Dispose();
            return Path.GetDirectoryName(filename);
        }
        public static void SetGridFont(GridControl control, Font font)
        {
            GridView view = (GridView)control.DefaultView;
            foreach (DevExpress.Utils.AppearanceObject ap in view.Appearance)
                ap.Font = font;
        }
        //public static string csSaveGridtoCSV(this GridView view, string ExcelPath, string fileNameAdditive,
        //                                     bool printTitles)
        //{
        //    return SaveGridtoCSV(ExcelPath, fileNameAdditive, view, printTitles);
        //}

        //public static string SaveGridtoCSV(string ExcelPath, string fileNameAdditive, GridView view, bool printTitles)
        //{
        //    string fn = DateTime.Now.csToFilename() + fileNameAdditive;
        //    string filename = MyLib.GetSaveFileName(ExcelPath, "csv", "CSV", "Save Error CSV", fn);
        //    if (filename == string.Empty) return ExcelPath;
        //    ExcelPath = Path.GetDirectoryName(filename);
        //    File.Delete(filename);
        //    TCSVWriter.WriteGridToCSV(filename, view, printTitles);
        //    return ExcelPath;
        //}
        public static string CHART_DATA_to_String(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in table.AsEnumerable())
            {
                sb.AppendLine(string.Format("{0,5} {1,5} {2,5} {3,5} {4,5} {5,5}"
                                            , t[0]
                                            , t[1]
                                            , t[2]
                                            , t[3]
                                            , t[4]
                                            , t[5]));
            }
            sb.ToString().csTell();
            return sb.ToString();

        }

        public static GridColumn csColumn(this GridControl grid, string colname)
        {
            GridView gv = (GridView)grid.DefaultView;
            return csColumn(gv, colname);
        }
        public static List<DataRow> csFilteredRows(this GridControl grid)
        {
            GridView v = (GridView)grid.DefaultView;
            List<DataRow> list = new List<DataRow>();
            for (int h = 0; h < v.DataRowCount; h++)
            {
                list.Add(v.GetDataRow(h));
            }
            return list;
        }
        public static List<DataRow> csVisibleRows(this GridView view)
        {
            List<DataRow> list = new List<DataRow>();
            for (int h = 0; h < view.DataRowCount; h++)
            {
                list.Add(view.GetDataRow(h));
            }
            return list;
        }
        public static GridColumn csColumn(this ColumnView gv, string colname)
        {
            return gv.Columns[colname];
        }
        public static void csClear(this ComboBoxEdit cb)
        {
            cb.SelectedIndex = -1;
        }

        public static void csSetComboList<T>(this ComboBoxEdit cb, List<T> ObjectList)
        {
            var temp = cb.Properties.Items;
            temp.Clear();
            foreach (var o in ObjectList)
            {
                temp.Add(o);
            }
            cb.SelectedIndex = -1;
        }

        public static void csSetUpGrid(this GridView view)
        {
            view.BestFitMaxRowCount = 100;
            view.BestFitColumns();
        }
        public static void SetUpGrid(GridView view)
        {
            view.BestFitMaxRowCount = 100;
            view.BestFitColumns();
        }
        public static void csBestFit(this GridControl grid, int rowChecked = 15)
        {
            var view = (GridView)grid.DefaultView;
            view.BestFitMaxRowCount = rowChecked;
            view.BestFitColumns();
        }

        public static object GetCurrentRow(GridView view)
        {
            if (view.FocusedRowHandle > -1)
            {
                return view.GetRow(view.FocusedRowHandle);
            }
            return null;
        }
        public static object GetCurrentRow(GridControl grid)
        {
            GridView view = (GridView)grid.Views[0];
            object v = view.GetRow(view.FocusedRowHandle);
            if (view.FocusedRowHandle > -1)
            {
                if (v is DataRowView)
                {
                    v = (DataRow)((DataRowView)v).Row;
                }
            }
            else
            {
                v = null;
            }

            return v;
        }
        public static List<object> csGetSelectedObjects(this GridView gv)
        {
            int[] rowHandles = gv.GetSelectedRows();
            List<object> rows = new List<object>();
            foreach (int r in rowHandles)
            {
                rows.Add(gv.GetRow(r));
            }
            return rows;
        }
        public static List<T> csGetSelectedObjects<T>(this GridView gv)
        {
            int[] rowHandles = gv.GetSelectedRows();
            List<T> rows = new List<T>();
            foreach (int r in rowHandles)
            {
                rows.Add((T)gv.GetRow(r));
            }
            return rows;
        }
        public static T csGetObject<T>(this GridView gv, int handle)
        {
            return (T)gv.GetRow(handle);
        }
        public static string csGetSortString(this GridView gv)
        {
            string ret = string.Empty;
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gv.SortedColumns)
            {
                if (ret != string.Empty)
                {
                    ret += ", ";
                }
                ret += string.Format("[{0}] {1}", column.FieldName,
                                     column.SortOrder == DevExpress.Data.ColumnSortOrder.Ascending ? "ASC" : "DESC");
            }
            return ret;
        }
        public static void csGridMoveToRow(this GridControl grid, int pid, string colName)
        {
            ColumnView vw = (ColumnView)grid.DefaultView;
            for (int i = 0; i < vw.DataRowCount; i++)
            {
                if ((int)vw.GetRowCellValue(i, colName) == pid)
                {
                    vw.FocusedRowHandle = i;
                    break;
                }
            }
        }
        public static string csGridToXLS(this GridControl grid, string folder, string fileName)
        {
            var temp = MyLib.GetSaveFileName(folder
                                            , "xls"
                                            , "xls"
                                            , "Save Grid"
                                            , fileName);
            grid.ExportToXls(temp);
            return Path.GetDirectoryName(temp);
        }

        public static void csGridMoveToRow(this GridControl grid, string value, string colName)
        {
            ColumnView vw = (ColumnView)grid.DefaultView;
            for (int i = 0; i < vw.DataRowCount; i++)
            {
                if ((string)vw.GetRowCellValue(i, colName) == value)
                {
                    vw.FocusedRowHandle = i;
                    break;
                }
            }
        }

        public static void csSetLayout(this GridView gv, string s64)
        {
            if (string.IsNullOrEmpty(s64)) return;
            byte[] b = TCipher.Base64toBytes(s64);
            MemoryStream ms = new MemoryStream(b);
            gv.RestoreLayoutFromStream(ms);
        }
        public static string csGetLayout(this GridView gv)
        {
            MemoryStream ms = new MemoryStream();
            gv.SaveLayoutToStream(ms);
            var b = ms.ToArray();
            return TCipher.BytesToBase64String(b);
        }

        public static void csSort(this ColumnView gv, string colname, bool Ascending = true)
        {
            gv.ClearSorting();
            GridColumn col = gv.csColumn(colname);
            col.SortOrder = Ascending
                                ? DevExpress.Data.ColumnSortOrder.Ascending
                                : DevExpress.Data.ColumnSortOrder.Descending;
        }
        public static void csSort(this ColumnView gv, int colNo)
        {
            gv.ClearSorting();
            gv.Columns[colNo].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gv.Columns[colNo].SortIndex = 0;
        }
        public static int csGetRid(this ComboBoxEdit cb)
        {
            TRID_NAME temp = cb.csGetComboBox<TRID_NAME>();
            return temp == null ? 0 : temp.RID;
        }
        public static T csGetComboBox<T>(this ComboBoxEdit devCombobox)
        {
            if (devCombobox.SelectedIndex > -1) return (T)devCombobox.SelectedItem;
            return default(T);
        }
        public static bool csEditInProgress(this ComboBoxEdit cb)
        {
            if (string.IsNullOrEmpty(cb.Text)) return false;
            if (cb.SelectedIndex < 0) return true;
            return cb.Text != cb.SelectedItem.ToString();
        }
       
        public static void csSetIndexByRid(this DevExpress.XtraEditors.ComboBoxEdit cb, int rid)
        {
            cb.SelectedIndex = -1;
            for (int i = 0; i < cb.Properties.Items.Count; i++)
            {
                TRID_NAME t = cb.Properties.Items[i] as TRID_NAME;
                if (t == null) break;
                if (t.RID == rid)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }
        public static void csSetComboBox<T>(this ComboBoxEdit devCombobox, T value) where T : class
        {
            devCombobox.SelectedIndex = -1;

            //   var v = value as T;
            if (value == null)
            {
                return;
            }
            for (int i = 0; i < devCombobox.Properties.Items.Count; i++)
            {
                var temp = devCombobox.Properties.Items[i] as T;
                if (temp == null) break;
                if (temp.Equals(value))
                {
                    devCombobox.SelectedIndex = i;
                    break;
                }
            }
            //      devCombobox.Text = value;
        }
        public static void csSetDevComboBox<T>(this ComboBoxEdit devCombobox, string value) where T : class
        {
            //   var v = value as T;
            if (string.IsNullOrEmpty(value))
            {
                devCombobox.SelectedIndex = -1;
                return;
            }
            for (int i = 0; i < devCombobox.Properties.Items.Count; i++)
            {
                var temp = devCombobox.Properties.Items[i] as T;
                if (temp == null) break;
                string s = temp.ToString();
                if (s == value)
                {
                    devCombobox.SelectedIndex = i;
                    break;
                }
            }
            devCombobox.Text = value;
        }
        public static void csSetDevComboList<T>(this ComboBoxEdit cbProblemType, List<T> ObjectList)
        {
            var temp = cbProblemType.Properties.Items;
            temp.Clear();
            foreach (var o in ObjectList)
            {
                temp.Add(o);
            }
        }
        public static void csSetComboBox(this ComboBoxEdit devCombobox, string value)
        {
            SetComboBox(devCombobox, value);
        }
        public static void SetComboBox(ComboBoxEdit devCombobox, string value)
        {
            ComboBoxItemCollection items = devCombobox.Properties.Items;
            if (value.Length > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ToString() == value)
                    {
                        devCombobox.SelectedIndex = i;
                        return;
                    }
                }
            }
            devCombobox.SelectedIndex = -1;
            devCombobox.Text = value;
        }
        // 130501
        //public static string csExportSelected(this GridControl grid, string DirectoryPath, string _filename)
        //{
        //    try
        //    {
        //        string filename = MyLib.GetSaveFileName(DirectoryPath, "xls", "Excel | *.xls", "Save Grid To Excel",
        //                                                _filename);
        //        if (filename != string.Empty)
        //        {
        //            if (File.Exists(filename))
        //            {
        //                File.Delete(filename);
        //            }
        //            //            string.Format("Got Here {0}", filename).csTell();
                    
        //            grid.ExportToXls(filename);
        //            return Path.GetDirectoryName(filename);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = "valid";
        //        if (grid == null) msg = "null";
        //        msg = string.Format("Message: {0} Directory: {1} FileName: {2}", ex.Message, msg, DirectoryPath,
        //                            _filename);
        //        msg.csTell();
        //    }
        //    "File Not Saved".csTell();
        //    return DirectoryPath;
        //}
        
        public static string csExport(this GridControl grid, string DirectoryPath, string _filename)
        {
            try
            {
                string filename = MyLib.GetSaveFileName(DirectoryPath, "xls", "Excel | *.xls", "Save Grid To Excel",
                                                        _filename);
                if (filename != string.Empty)
                {
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
                    //            string.Format("Got Here {0}", filename).csTell();
                    grid.ExportToXls(filename);
                    return Path.GetDirectoryName(filename);
                }
            }
            catch (Exception ex)
            {
                string msg = "valid";
                if (grid == null) msg = "null";
                msg = string.Format("Message: {0} Directory: {1} FileName: {2}", ex.Message, msg, DirectoryPath,
                                    _filename);
                msg.csTell();
            }
            "File Not Saved".csTell();
            return DirectoryPath;
        }
        public static string csExport(this GridControl grid, string DirectoryPath)
        {
            string fn = grid.Name.Substring(4);
            return csExport(grid, DirectoryPath, fn,true);
        }
        public static string csExport(this GridControl grid, string DirectoryPath, string filename, bool addDate)
        {
            if (!Directory.Exists(DirectoryPath)) DirectoryPath = @"C:\";
            string fn = addDate
                            ? string.Format("{3} {2}_{0}_{1}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year,
                                            filename)
                            : filename;
            return csExport(grid, DirectoryPath, fn);
        }
      
    }

}