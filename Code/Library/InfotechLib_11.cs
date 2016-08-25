using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Security.Principal;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using DevExpress.XtraEditors;
using ComboBox = System.Windows.Forms.ComboBox;

namespace ExpressHelper1011.Library
{
    public enum enmScreenCaptureMode
    {
        Screen,
        Window
    }

    class ScreenCapturer
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Bitmap Capture(int Top,int Left,int Width,int Height)
        {
            Rectangle bounds;


            var foregroundWindowsHandle = GetForegroundWindow();
            var rect = new Rect();
            GetWindowRect(foregroundWindowsHandle, ref rect);
            bounds = new Rectangle(rect.Left + Left, rect.Top + Top, Width, Height); //rect.Right - rect.Left, rect.Bottom - rect.Top);
            CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);


            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }

        public Point CursorPosition
        {
            get;
            protected set;
        }
    }
    public static partial class MyLib
    {
        public static BackgroundWorker CreateBackgroundWorker(DoWorkEventHandler doWork,
                                                                     RunWorkerCompletedEventHandler completedWork,
          ProgressChangedEventHandler reportProgress = null)
        {
            var bgw = new BackgroundWorker();
            bgw.DoWork += doWork;
            bgw.RunWorkerCompleted += completedWork;
            if (reportProgress != null)
            {
                bgw.WorkerReportsProgress = true;
                bgw.ProgressChanged += reportProgress;
            }
            return bgw;
        }
        public static List<T> GetEnumList<T>()
        {
            if ((typeof(T)).BaseType.Name != typeof(Enum).Name) return null;
            List<T> codes = new List<T>();
            foreach (T code in Enum.GetValues(typeof(T)))
            {
                codes.Add(code);
            }
            return codes;
        }


        //public static LeastSquare ComputeLeastSq(double Retention, List<double> SeriesX, List<double> SeriesY)
        //{
        //    if(SeriesX.Count != SeriesY.Count) return null;
        //    double sumX = SeriesX.Sum();
        //    double sumY = SeriesY.Sum();
        //    double sumX2 = SeriesX.Select(t => t * t).Sum();
        //    double sumY2 = SeriesY.Select(t => t * t).Sum();
        //    double sumXY = 0.0;
        //    for(int i = 0; i < SeriesX.Count;i++)
        //    {
        //        sumXY += SeriesX[i]*SeriesY[i];

        //    }
        //    int n = SeriesX.Count();
        //    double aveX = sumX / n;
        //    double aveY = sumY / n;
        //   double Slope = (n * sumXY - (sumX * sumY)) /
        //                  ((n * sumX2) - (sumX * sumX));

        //  double  YIntercept = aveY - Slope * aveX;


        //    r = (n * sumXY - sumX * sumY) /
        //           Math.Sqrt(((n * sumX2) - (sumX * sumX)) * ((n * sumY2) - (sumY * sumY)));

        //    double TotVar = SeriesY.Select(t => computeTotVarPoint(t, aveY)).Sum();
        //    double ExpVar = q.Select(t => ComputeExpVarPoint(YIntercept, Slope, t.GWI, aveY)).Sum();
        //    r2 = ExpVar / TotVar;
        //    return new LeastSquare(n, YIntercept, Slope, r, r2, Retention);
        //}
        ////private static double computeTotVarPoint(double Y, double aveY)
        ////{
        ////    double p = Y - aveY;
        ////    return p * p;
        ////}
        //private static double ComputeExpVarPoint(double YIntercept, double Slope, double X, double aveY)
        //{
        //    double p = YIntercept + (Slope * X) - aveY;
        //    return p * p;
        //}

        //// Add System.ServiceProcess to references
        //public static string GetDiskID(string X_Disk)
        //{
        //    string disk = string.Format(@"win32_logicaldisk.deviceid=""{0}:""", X_Disk);
        //    ManagementObject dsk = new ManagementObject(disk);
        //    dsk.Get();
        //    return dsk["VolumeSerialNumber"] == null ? "N/A" : dsk["VolumeSerialNumber"].ToString();
        // }
        public static long TicksPerSlot = TimeSpan.TicksPerMinute * 10;
        public static long StartingZulu = new DateTime(2009, 1, 1).Ticks;

        public static long ZuluIndex(DateTime dt, int TimeSlot)
        {
            return ((dt.Date.Ticks - StartingZulu) / TicksPerSlot) + TimeSlot;
        }

        public static long ZuluIndex(DateTime dt)
        {
            return ((dt.Ticks - StartingZulu) / TicksPerSlot);
        }

        public static DateTime ZuluToDateTime(long zulu)
        {
            return new DateTime((zulu * TicksPerSlot) + StartingZulu);
        }

        public static void InstallUpdateSyncWithInfo(Form form = null)
        {
            UpdateCheckInfo info = null;

            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                "This application was not installed from a newwork".csTell();
                return;
            }
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            try
            {
                info = ad.CheckForDetailedUpdate();
            }
            catch (DeploymentDownloadException dde)
            {
                MessageBox.Show(
                    "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " +
                    dde.Message);
                return;
            }
            catch (InvalidDeploymentException ide)
            {
                MessageBox.Show(
                    "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " +
                    ide.Message);
                return;
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(
                    "This application cannot be updated. It is likely not a ClickOnce application. Error: " +
                    ioe.Message);
                return;
            }

            if (info.UpdateAvailable)
            {
                Boolean doUpdate = true;

                if (!info.IsUpdateRequired)
                {
                    DialogResult dr =
                        MessageBox.Show("An update is available. Would you like to update the application now?",
                                        "Update Available", MessageBoxButtons.OKCancel);
                    if (DialogResult.OK != dr)
                    {
                        doUpdate = false;
                    }
                }
                else
                {
                    // Display a message that the app MUST reboot. Display the minimum required version.
                    MessageBox.Show("This application has detected a mandatory update from your current " +
                                    "version to version " + info.MinimumRequiredVersion.ToString() +
                                    ". The application will now install the update and restart.",
                                    "Update Available", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }

                if (doUpdate)
                {
                    try
                    {
                        if (form != null)
                        {
                            form.Show();
                            form.TopMost = true;
                        }
                        ad.Update();
                        if (form != null) form.Hide();
                        MessageBox.Show("The application has been upgraded, and will now restart.");
                        Application.Restart();
                    }
                    catch (DeploymentDownloadException dde)
                    {
                        MessageBox.Show(
                            "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " +
                            dde);
                        return;
                    }
                }
            }
            else
            {
                "You have the latest update installed".csTell();
            }
        }

        public static T csToEnum<T>(this string value)
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }
            if (Enum.IsDefined(typeof(T), value) == false)
            {
                throw new InvalidCastException();
            }
            return (T)Enum.Parse(typeof(T), value);
        }

        public static DateTime csTo15th(this DateTime dt)
        {
            return csToDay(dt, 15);
        }

        public static DateTime csToDay(this DateTime dt, int day)
        {
            if (day < 1) day = 1;
            return dt.Date.AddDays(day - dt.Day);
        }

        public static DateTime csToEOM(this DateTime dt)
        {
            return dt.Date.AddMonths(1).csToDay(1).AddTicks(-1);
        }

        public static DateTime csToBOM(this DateTime dt)
        {
            return dt.csToDay(1);
        }

        public static DateTime csToSOD(this DateTime dt)
        {
            return dt.Date;
        }

        public static DateTime csToEOD(this DateTime dt)
        {
            return dt.Date.AddDays(1).AddMinutes(-1);
        }

        public static DateTime csToBOY(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime csToBOQ(this DateTime dt)
        {
            int q = (dt.Month / 3) * 3;
            var b = dt.csToBOY();
            return b.AddMonths(q);
        }

        public static DateTime csToEOY(this DateTime dt)
        {
            return new DateTime(dt.Year + 1, 1, 1).AddSeconds(-1);
        }

        public static DateTime csToBOW(this DateTime dt)
        {
            int k = (int)dt.DayOfWeek;
            return dt.Date.AddDays(-k);
        }

        public static DateTime csToEOW(this DateTime dt)
        {
            return dt.Date.AddDays(6 - (int)dt.DayOfWeek);
        }

        public static int csToDOW(this DateTime dt)
        {
            return (int)dt.Date.DayOfWeek;
        }

        public static string csYMD(this DateTime dt)
        {
            return string.Format(@"{0}/{1}/{2}", dt.Year, dt.Month, dt.Day);
        }

        public static TimeSpan csToTimeSpan(this string s)
        {
            TimeSpan ts;
            bool valid = TimeSpan.TryParse(s, out ts);
            if (valid) return ts;
            return new TimeSpan();
        }

        public static string csTruncate(this string s, int len)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= len ? s : s.Substring(0, len);
        }

        public static int csToTimeSlot(this DateTime dt)
        {
            return (60 * dt.Hour + dt.Minute) / 10;
        }

        public static int csToJulianDate(this DateTime dt)
        {
            return ((dt.Year % 100) * 1000) + dt.DayOfYear;
        }

        public static string csToFilename(this DateTime dt)
        {
            return string.Format("{0}{1}{2}", dt.Year, dt.Month, dt.Day);
        }

        public static DateTime csConvertToDT(this TimeSpan ts)
        {
            return DateTime.Now.Date + ts;
        }

        public static TimeSpan csConvertToTS(this DateTime dt)
        {
            return dt.TimeOfDay;
        }

        public static string ToStringArray(this List<int> list)
        {
            int[] ia = list.ToArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ia.Length - 1; i++)
            {
                sb.Append(string.Format("{0},", ia[i]));
            }
            sb.Append(ia[ia.Length - 1]);
            return sb.ToString();
        }

        public static void csSetWaitCursor(this Button btn, Form form, int c = 0)
        {
            Color color = Color.Red;
            Cursor cur = Cursors.WaitCursor;
            if (c != 0)
            {
                color = Color.FromArgb(c);
                cur = Cursors.Default;
            }
            btn.BackColor = color;
            btn.Refresh();
            if (form != null)
            {
                form.Cursor = cur;
            }

        }

        public static void csSetWaitCursor(this object btnObject, Form form, int normalColor = 0)
        {
            Button btn = btnObject as Button;
            if (btn == null) return;
            btn.csSetWaitCursor(form, normalColor);
        }

        public static void csSetColor(this Button btn, Form form = null)
        {
            btn.csSetColor(Color.LightGreen);
            if (form != null) form.Cursor = Cursors.Default;
        }

        public static void csSetColor(this Button btn, Color c)
        {
            btn.BackColor = c;
        }

        public static object csSelectedItem(this ComboBox box)
        {
            return box.Items[box.SelectedIndex];
        }

        public static void csSetComboBoxString(this ComboBox cb, string value)
        {
            //   var v = value as T;
            cb.SelectedIndex = -1;
            for (int i = 0; i < cb.Items.Count; i++)
            {
                var temp = cb.Items[i].ToString();
                if (temp == value)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }
        public static void csSetComboBox<T>(this ComboBox cb, T value) where T : class
        {
            //   var v = value as T;
            if (value == null)
            {
                cb.SelectedIndex = -1;
                return;
            }
            for (int i = 0; i < cb.Items.Count; i++)
            {
                var temp = cb.Items[i] as T;
                if (temp == null) break;
                if (temp == value)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }

        public static void csSetIndexByRID(this ComboBox cb, int rid)
        {
            cb.SelectedIndex = -1;
            for (int i = 0; i < cb.Items.Count; i++)
            {
                TRID_NAME t = cb.Items[i] as TRID_NAME;
                if (t == null) break;
                if (t.RID == rid)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }

        public static string csGetLast(this ComboBox cb)
        {
            int count = cb.Items.Count;
            if (count <= 0) return string.Empty;
            TRID_NAME temp = cb.Items[count - 1] as TRID_NAME;
            return temp == null ? string.Empty : temp.Name;
        }

        public static bool csEditInProgress(this ComboBox cb)
        {
            if (string.IsNullOrEmpty(cb.Text)) return false;
            if (cb.SelectedIndex < 0) return true;
            return cb.Text != cb.SelectedItem.ToString();
        }

        public static T csGetComboBox<T>(this ComboBox cb)
        {
            if (cb.SelectedIndex > -1) return (T)cb.SelectedItem;
            return default(T);
        }

        public static void csSetComboBox<T>(this ComboBox cb, string value) where T : class
        {
            //   var v = value as T;
            if (string.IsNullOrEmpty(value))
            {
                cb.SelectedIndex = -1;
                return;
            }
            for (int i = 0; i < cb.Items.Count; i++)
            {
                var temp = cb.Items[i] as T;
                if (temp == null) break;
                string s = temp.ToString();
                if (s == value)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
            cb.Text = value;
        }

        public static int csGetRid(this System.Windows.Forms.ComboBox cb)
        {
            TRID_NAME t = cb.SelectedItem as TRID_NAME;
            return t == null ? 0 : t.RID;
        }

        public static void SetComboBox(ComboBox cb, string value)
        {
            ComboBox.ObjectCollection items = cb.Items;
            if (value.Length > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].ToString() == value)
                    {
                        cb.SelectedIndex = i;
                        return;
                    }
                }
            }
            cb.SelectedIndex = -1;
            cb.Text = value;
        }

        public static void csSetComboBoxList<T>(this ComboBox cbProblemType, List<T> ObjectList)
        {
            var temp = cbProblemType.Items;
            temp.Clear();
            foreach (var o in ObjectList)
            {
                temp.Add(o);
            }
        }
        public static void csSetComboList<T>(this ComboBox cbProblemType, List<T> ObjectList)
        {
            var temp = cbProblemType.Items;
            temp.Clear();
            foreach (var o in ObjectList)
            {
                temp.Add(o);
            }
        }

        public static string csGetMonthName(this DateTime dt)
        {
            DateTimeFormatInfo temp = new DateTimeFormatInfo();
            return temp.GetMonthName(dt.Month);

        }

        public static string csGetDayName(this DateTime dt)
        {
            return dt.DayOfWeek.ToString();
        }

        public static List<object> csGetCheckedItems(this CheckedListBox box)
        {
            List<object> temp = new List<object>();
            foreach (object itemChecked in box.CheckedItems)
            {
                temp.Add(itemChecked);
            }
            return temp;
        }

        public static string csGetDigits(this string s)
        {
            return StrToNumeric(s);
        }

        public static List<string> csGetCheckedItemsAsString(this CheckedListBox box)
        {
            return (from object itemChecked in box.CheckedItems select itemChecked.ToString()).ToList();
        }

        public static void csClearCheckedItems(this CheckedListBox Box, bool newstate)
        {
            for (int i = 0; i < Box.Items.Count; i++)
            {
                Box.SetItemChecked(i, newstate);
            }
        }

        public static int csToInteger(this string s)
        {
            return MyLib.intParseString(s);
        }

        public static string csNormalize(this string s)
        {
            s = s.ToLower().Trim();
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {

                if (((c >= 'a') && (c <= 'z')) || "0123456789".Contains(c)) sb.Append(c);
            }
            return sb.ToString();
        }

        public static StreamWriter csOpenStreamWriter(this string filename)
        {
            File.Delete(filename);
            return new StreamWriter(filename);
        }

        public static StreamReader csOpenStreamReader(this string filename)
        {
            if (!File.Exists(filename)) return null;
            return new StreamReader(filename);
        }

        public static string csPlusSub(this string mapAddress)
        {
            return mapAddress.Replace(" ", "+");
        }

        public static Point csToPoint(this string s)
        {
            string[] ps = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (ps.Length != 2)
            {
                return new Point(0, 0);
            }
            return new Point(ps[0].csToInteger(), ps[1].csToInteger());
        }

        public static string csToString(this Point p)
        {
            return string.Format("{0},{1}", p.X, p.Y);
        }

        public static string csToString(this Size p)
        {
            return string.Format("{0},{1}", p.Width, p.Height);
        }

        public static string csToString(this byte[] input)
        {
            return ByteArrayToString(input);
        }

        public static string ByteArrayToString(byte[] input)
        {
            UTF8Encoding enc = new UTF8Encoding();
            string str = enc.GetString(input);
            return str;
        }

        public static void SetTexboxBackground(Control.ControlCollection controlCollection, Color color)
        {
            foreach (Control c in controlCollection)
            {
                if (c is TextBox)
                {
                    if ((c.Tag == null) || (c.Tag.ToString() != "-1"))
                    {
                        c.BackColor = color;
                    }
                }
                else
                {
                    SetTexboxBackground(c.Controls, color);
                }
            }
        }

        public static Size csToSize(this string s)
        {
            string[] ps = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (ps.Length != 2)
            {
                return new Size(100, 100);
            }
            return new Size(ps[0].csToInteger(), ps[1].csToInteger());
        }

        public static Color csToColor(this int i)
        {
            if (i == int.MinValue || i == 0) return Color.LightSteelBlue;
            return Color.FromArgb(i);
        }

        public static Color csToColor(this string c)
        {
            return Color.FromArgb(c.csToInteger());
        }

        public static string csToString(this Color c)
        {
            return c.ToArgb().ToString();
        }

        public static int csToInt(this Color c)
        {
            return c.ToArgb();
        }

        public static int csToInteger(this TextBox tb)
        {
            return tb.Text.csToInteger();
        }

        public static List<string> csToList(this ListBox lb)
        {
            return (from object s in lb.Items select s.ToString()).ToList();
        }

        public static decimal csToDecimal(this string s)
        {
            return MyLib.ToDecimal(s);
        }

        public static decimal csToDecimal(this TextBox t)
        {
            return t.Text.csToDecimal();
            ;
        }

        public static DataRow csDataRow(this BindingSource bs)
        {
            return (DataRow)((DataRowView)(bs.Current)).Row;
        }

        public static DataRow csDataRow(this DataRowView rv)
        {
            return (DataRow)rv.Row;
        }

        public static DateTime csToDateTime(this string s)
        {
            return MyLib.StrToDate(s);
        }

        public static double csToDouble(this string s)
        {
            return MyLib.ToDouble(s);
        }

        public static double csToDouble(this TextBox tb)
        {
            return tb.Text.csToDouble();
        }

        public static float csToFloat(this string s)
        {
            return MyLib.ToFloat(s);
        }

        public static float csToFloat(this TextBox t)
        {
            return t.Text.csToFloat();
        }

        public static string csBoolToStr(this bool value)
        {
            return value ? "T" : "F";
        }

        public static Point csBound(this Point p, int k, Form form)
        {
            Rectangle r = Screen.GetWorkingArea(form);
            int h = r.Bottom;
            int w = r.Right;
            int x = p.X;
            int y = p.Y;
            if (x < 0) x = k;
            if (x > (w - k)) x = k;
            if (y < 0) y = k;
            if (y > (h - k)) y = k;
            return new Point(x, y);
        }

        public static Size csBound(this Size p, int k)
        {
            int x = p.Width;
            int y = p.Height;
            if (x < k) x = k;
            if (y < k) y = k;
            return new Size(x, y);
        }

        public static string csToStr(this bool value)
        {
            return value ? "T" : "F";
        }

        public static int csBoolToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static void csReseqTabs(this TabControl tabControl, string tss)
        {
            char[] ca = ",".ToCharArray();
            string[] tabnames = tss.Split(ca, StringSplitOptions.RemoveEmptyEntries);
            List<string> namelist = new List<string>();
            foreach (string s in tabnames)
            {
                namelist.Add(s);
            }
            Dictionary<string, TabPage> tabs = new Dictionary<string, TabPage>();
            foreach (TabPage tab in tabControl.TabPages)
            {
                tabs[tab.Text] = tab;
            }
            tabControl.csReseqTabs(namelist, tabs);
        }
        public static List<T> csRemove<T>(this List<T> listMain, List<T> listExtra)
        {
            List<T> temp = new List<T>(listMain);
            foreach (var val in listExtra)
            {
                temp.Remove(val);
            }
            return temp;
        }
        public static void csReseqTabs(this TabControl tabControl, List<string> tabSeqList,
                                       Dictionary<string, TabPage> tabPageDictionary)
        {

            tabControl.TabPages.Clear();
            foreach (string tab in tabSeqList)
            {
                try
                {
                    if (!tabControl.TabPages.Contains(tabPageDictionary[tab]))
                        tabControl.TabPages.Add(tabPageDictionary[tab]);
                }
                catch
                {
                }
            }
        }

        public static string csGetPages(this TabControl tabControl)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TabPage tab in tabControl.TabPages)
            {
                sb.Append(string.Format("{0},", tab.Text));
            }
            string s = sb.ToString();
            s = s.Remove(s.LastIndexOf(','));
            return s;
        }

        public static void csSetTabPages(this TabControl tabControl, string pages) //csv tab page names 
        {
            Dictionary<string, TabPage> currentPages = new Dictionary<string, TabPage>();
            foreach (TabPage tab in tabControl.TabPages)
            {
                currentPages[tab.Text] = tab;
            }
            tabControl.TabPages.Clear();
            string[] names = pages.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in names)
            {
                tabControl.TabPages.Add(currentPages[s]);
            }
        }

        public static bool csToBool(this string s)
        {
            return !string.IsNullOrEmpty(s) && s.Substring(0, 1).ToUpper() == "T";
        }

        public static bool csToBool(this int n)
        {
            return n != 0;
        }

        public static void csSetIndexString(this ComboBox box, string value)
        {
            box.SelectedIndex = -1;
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (box.Items[i].ToString() != value) continue;
                box.SelectedIndex = i;
                return;
            }
        }


        public const string RegString_EMail =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const string RegString_SSN = @"\d\d\d-\d\d-\d\d\d\d$";

        public static string CheckForApros(string name)
        {
            string n = name;
            int index = n.IndexOf("\'");
            if (index > -1)
            {
                n = n.Substring(0, index) + @"'" + n.Substring(index);
            }
            return n;
        }
        //public static void CheckForUpdate()
        //{
        //    if (!ApplicationDeployment.IsNetworkDeployed)
        //    {
        //        "Not a network deployed application".csTell();
        //    }
        //    else
        //    {
        //        ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
        //        UpdateCheckInfo info = null;
        //        try
        //        {
        //            info = ad.CheckForDetailedUpdate();
        //        }
        //        catch (DeploymentDownloadException dde)
        //        {
        //            MessageBox.Show(
        //                "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " +
        //                dde.Message);
        //            return;
        //        }
        //        catch (InvalidDeploymentException ide)
        //        {
        //            MessageBox.Show(
        //                "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " +
        //                ide.Message);
        //            return;
        //        }
        //        catch (InvalidOperationException ioe)
        //        {
        //            MessageBox.Show(
        //                "This application cannot be updated. It is likely not a ClickOnce application. Error: " +
        //                ioe.Message);
        //            return;
        //        }

        //        if (info.UpdateAvailable)
        //        {
        //            Boolean doUpdate = true;

        //            if (!info.IsUpdateRequired)
        //            {
        //                DialogResult dr =
        //                    MessageBox.Show("An update is available. Would you like to update the application now?",
        //                                    "Update Available", MessageBoxButtons.OKCancel);
        //                if (!(DialogResult.OK == dr))
        //                {
        //                    doUpdate = false;
        //                }
        //            }
        //            else
        //            {
        //                 Display a message that the app MUST reboot. Display the minimum required version.
        //                MessageBox.Show("This application has detected a mandatory update from your current " +
        //                                "version to version " + info.MinimumRequiredVersion.ToString() +
        //                                ". The application will now install the update and restart.",
        //                                "Update Available", MessageBoxButtons.OK,
        //                                MessageBoxIcon.Information);
        //            }

        //            if (doUpdate)
        //            {
        //                try
        //                {
        //                    ad.Update();
        //                    MessageBox.Show("The application has been upgraded, and will now restart.");
        //                    Application.Restart();
        //                }
        //                catch (DeploymentDownloadException dde)
        //                {
        //                    MessageBox.Show(
        //                        "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " +
        //                        dde);
        //                    return;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            "You have the latest version".csTell();
        //        }
        //    }
        //}
        public static string intListToString(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int n in list)
            {
                sb.Append(string.Format("{0},", n));
            }
            string s = sb.ToString();

            return s.Length > 0 ? s.Substring(0, s.Length - 1) : string.Empty;
        }

        public static DateTime IntToTime(int p)
        {
            return DateTime.Now.Date.AddMinutes(p);
        }

        public static string IntToHex(int n)
        {
            return n.ToString("X");
        }

        public static int HexToInt(string p)
        {
            int k = 0;
            int.TryParse(p, System.Globalization.NumberStyles.HexNumber, null, out k);
            return k;
        }

        public static DateTime IntToTime(DateTime dt, int p)
        {
            return dt.Date.AddMinutes(p);
        }

        public static int TimeToInt(DateTime dateTime)
        {
            return dateTime.Minute;
        }

        public static List<int> intStringToList(string strList)
        {
            char[] dl = new char[1];
            dl[0] = ',';
            string[] digits = strList.Split(dl, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>();
            foreach (string s in digits)
            {
                int i = 0;
                int.TryParse(s, out i);
                list.Add(i);
            }
            return list;
        }

        public static void csPositionSizeForm(this Form form)
        {
            Rectangle r = Screen.GetWorkingArea(form);
            int h = r.Bottom;
            int w = r.Right;
            form.Top = form.Top < 0 ? 100 : form.Top < (h - 100) ? form.Top : h - 100;
            form.Left = form.Left < 0 ? 100 : form.Left < w ? form.Left : w - 100;
            form.Height = form.Height > h ? h : form.Height > 100 ? form.Height : 100;
            form.Width = form.Width > r.Right ? r.Right : form.Width > 100 ? form.Width : 100;
        }

        public static string GetMachineName()
        {
            return System.Windows.Forms.SystemInformation.ComputerName;
        }

        public static string GetWindowsUserID()
        {
            return UID;
        }

        public static string UID
        {
            get
            {
                string s = WindowsIdentity.GetCurrent().Name;
                int loc = s.IndexOf('\\');
                if (loc > -1)
                {
                    s = s.Length > (loc + 1) ? s.Substring(loc + 1) : string.Empty;
                }
                return s;
            }
        }

        public static void RunExternal(string programName, string arguement1)
        {
            Process.Start(programName, arguement1);
        }

        public static void RunExternal(string fileName)
        {
            Process.Start(fileName);
        }

        public enum eMonth
        {
            NONE = 0,
            Jan = 1,
            Feb,
            Mar,
            Apr,
            May,
            Jun,
            Jul,
            Aug,
            Sep,
            Oct,
            Nov,
            Dec
        }

        public static string csMonthNameZeroBased(this int month)
        {
            return ((eMonth)(month + 1)).ToString();
        }

        public static string csMonthName(this int month)
        {
            return ((eMonth)month).ToString();
        }

        public static string csMonthName(this DateTime dt)
        {
            return ((eMonth)dt.Month).ToString();
        }

        public static string[] MonthLabel =
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov"
                , "Dec"
            };

        public static string GetFullMonthName(int month)
        {
            return new DateTime(1942, month, 1).ToString("MMMM");
        }

        public static string GetFullName(string firstName, string lastName)
        {
            return string.Format("{0} {1}", firstName, lastName).Trim();
        }

        public static string GetMonthLabel(int i)
        {
            return MonthLabel[i];
        }

        public static int GetMonthIndex(string s)
        {
            for (int i = 0; i < 12; i++)
            {
                if (MonthLabel[i].ToLower() == s.ToLower())
                {
                    return i + 1;
                }
            }
            return -1;
        }

        private static Object csLock = new Object();

        public static string CreateCSV(ArrayList al) // al string elements
        {
            string s = "";
            foreach (object n in al)
            {
                string ts = n.ToString();
                int i = ts.IndexOf('"');
                int j = ts.IndexOf(',');
                if ((i > -1) || (j > -1))
                {
                    ts = '"' + ts + '"';
                }
                s += ts + ',';
            }
            if (s.Length > 0)
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s + Environment.NewLine;
        }

        public static void WriteHashTable(Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb.Append(key + "=" + ht[key].ToString() + Environment.NewLine);
            }
            csTell(sb.ToString());
        }

        public static void dateParseString(string s, ref DateTime v)
        {
            DateTime.TryParse(s.Trim(), null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out v);
        }

        public static int intParseString(string s)
        {
            int v = 0;
            intParseString(s, ref v);
            return v;

        }

        public static void intParseString(string s, ref int v)
        {
            int.TryParse(s, System.Globalization.NumberStyles.Any, null, out v);
        }

        public static void doubleParseString(string s, ref double v)
        {
            double.TryParse(s, System.Globalization.NumberStyles.Any, null, out v);
        }

        public static void decimalParseString(string s, ref decimal v)
        {
            decimal.TryParse(s, System.Globalization.NumberStyles.Any, null, out v);
        }

        public static void ShowIntArray(int[] a)
        {
            string s = string.Empty;
            for (int i = 0; i < a.Length; i++)
            {
                s += i.ToString() + ":" + a[i].ToString() + " ";
            }
            csTell(s);
        }

        public static void ShowGraphic(byte[] data, PictureBox pb)
        {
            MemoryStream gr = new MemoryStream(data);
            pb.Image = System.Drawing.Image.FromStream(gr);
            pb.Show();
        }

        public static ArrayList ParseCSV(string s) // csv line
        {
            ArrayList sList = new ArrayList();
            bool inQuotes = false;
            do
            {
                bool Processed = false;
                bool found = false;
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    switch (c)
                    {
                        case ',':
                            {
                                if (!inQuotes)
                                {
                                    string ts = s.Substring(0, i);
                                    s = s.Substring(i + 1);
                                    if ((ts.Length > 0) && ts[0] == '"')
                                    {
                                        ts = ts.Substring(1);
                                    }
                                    if ((ts.Length > 0) && ts[ts.Length - 1] == '"')
                                    {
                                        ts = ts.Substring(0, ts.Length - 1);
                                    }
                                    sList.Add(ts);
                                    Processed = true;
                                    found = true;
                                    if (s.Length == 0)
                                    {
                                        sList.Add("");
                                    }
                                }
                                break;
                            }
                        case '"':
                            {
                                inQuotes = !inQuotes;
                                break;
                            }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (!Processed && inQuotes && !found)
                {
                    if ((s.Length > 0) && s[0] == '"')
                    {
                        s = s.Substring(1);
                    }
                    if ((s.Length > 0) && s[s.Length - 1] == '"')
                    {
                        s = s.Substring(0, s.Length - 1);
                    }
                    sList.Add(s);
                    s = "";
                }
                else if (!Processed)
                {
                    string ts = s;
                    s = "";
                    if ((ts.Length > 0) && ts[0] == '"')
                    {
                        ts = ts.Substring(1);
                    }
                    if ((ts.Length > 0) && ts[ts.Length - 1] == '"')
                    {
                        ts = ts.Substring(0, ts.Length - 1);
                    }
                    sList.Add(ts);
                }

            } while (s.Length > 0);
            return sList;
        }

        public static string FilterAddTerm(string rowFilter, string ft, bool IS_AND_Condition)
        {
            if (ft.Length > 0)
            {
                if (rowFilter.Length > 0)
                {
                    // must add to rf
                    if (IS_AND_Condition)
                    {
                        rowFilter = string.Format("({0}) AND {1}", rowFilter, ft);
                    }
                    else
                    {
                        rowFilter = string.Format("({0}) OR {1}", rowFilter, ft);
                    }

                }
                else
                {
                    rowFilter = ft;
                }
            }
            //       rowFilter.csTell();
            return rowFilter;
        }

        public static DialogResult csTell(object Message, string caption, MessageBoxButtons yn)
        {
            return MessageBox.Show(Message.ToString(), caption, yn);
        }

        public static DialogResult csTell(Form f, object Message, string caption, MessageBoxButtons yn)
        {
            return MessageBox.Show(f, Message.ToString(), caption, yn);
        }

        public static void csTell(this object temp)
        {
            string s = temp.ToString();
            try
            {
                if (s.Length > 0) Clipboard.SetText(s);
            }
            catch
            {
            }
            MessageBox.Show(s);
        }

        public static string csToCSV(this List<int> list)
        {
            string s = string.Empty;
            foreach (int i in list)
            {
                s += string.Format("{0},", i);
            }
            if (s.Length > 0) s = s.Substring(0, s.Length - 1);
            return s;
        }

        public static string csList2Clipboard(this List<string> temp, int columns)
        {
            string s = string.Empty;
            int i = 1;
            foreach (string ss in temp)
            {
                if ((i++ % columns) == 0)
                {
                    s += (ss + Environment.NewLine);
                }
                else
                {
                    s += (ss + ", ");
                }
            }
            Clipboard.SetText(s.Replace(",", ((char)9).ToString()));
            return s;
        }

        public static string csList2String(this List<string> temp)
        {
            return temp.Aggregate(string.Empty, (current, ss) => current + (ss + Environment.NewLine));
        }

        public static string csList2CSV(this List<string> temp)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in temp)
            {
                sb.Append(string.Format(@"""{0}"",", s));
            }
            string result = sb.ToString();
            return result.Substring(0, result.Length - 1);
        }

        public static string csList2Clipboard(this List<string> temp, int columns, string Title)
        {
            string s = Title + Environment.NewLine;
            int i = 1;
            foreach (string ss in temp)
            {
                if ((i++ % columns) == 0)
                {
                    s += (ss + Environment.NewLine);
                }
                else
                {
                    s += (ss + ", ");
                }
            }
            Clipboard.SetText(s.Replace(",", ((char)9).ToString()));
            return s;
        }

        public static void csTell(this object temp, string comment)
        {
            MyLib.csTell(string.Format("{0} - {1}", temp.ToString(), comment.ToString()));
        }

        public static void csTell(Form f, string comment)
        {
            MessageBox.Show(f, comment);
        }

        public static DialogResult csTell(this string s, string caption, MessageBoxButtons btns)
        {
            return MessageBox.Show(s, caption, btns);
        }

        //public static void exportToExcel(DataTable source, string fileName)
        //{

        //    System.IO.StreamWriter excelDoc;

        //    excelDoc = new System.IO.StreamWriter(fileName);
        //    const string startExcelXML = "<xml version>\r\n<Workbook " +
        //                                 "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
        //                                 " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
        //                                 "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
        //                                 "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
        //                                 "office:spreadsheet\">\r\n <Styles>\r\n " +
        //                                 "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
        //                                 "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
        //                                 "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
        //                                 "\r\n <Protection/>\r\n </Style>\r\n " +
        //                                 "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
        //                                 "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
        //                                 "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
        //                                 " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
        //                                 "ss:ID=\"Decimal\">\r\n <NumberFormat " +
        //                                 "ss:Format=\"0.0000\"/>\r\n </Style>\r\n " +
        //                                 "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
        //                                 "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
        //                                 "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
        //                                 "ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n " +
        //                                 "</Styles>\r\n ";
        //    const string endExcelXML = "</Workbook>";

        //    int rowCount = 0;
        //    int sheetCount = 1;
        //    /*
        //   <xml version>
        //   <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
        //   xmlns:o="urn:schemas-microsoft-com:office:office"
        //   xmlns:x="urn:schemas-microsoft-com:office:excel"
        //   xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
        //   <Styles>
        //   <Style ss:ID="Default" ss:Name="Normal">
        //     <Alignment ss:Vertical="Bottom"/>
        //     <Borders/>
        //     <Font/>
        //     <Interior/>
        //     <NumberFormat/>
        //     <Protection/>
        //   </Style>
        //   <Style ss:ID="BoldColumn">
        //     <Font x:Family="Swiss" ss:Bold="1"/>
        //   </Style>
        //   <Style ss:ID="StringLiteral">
        //     <NumberFormat ss:Format="@"/>
        //   </Style>
        //   <Style ss:ID="Decimal">
        //     <NumberFormat ss:Format="0.0000"/>
        //   </Style>
        //   <Style ss:ID="Integer">
        //     <NumberFormat ss:Format="0"/>
        //   </Style>
        //   <Style ss:ID="DateLiteral">
        //     <NumberFormat ss:Format="mm/dd/yyyy;@"/>
        //   </Style>
        //   </Styles>
        //   <Worksheet ss:Name="Sheet1">
        //   </Worksheet>
        //   </Workbook>
        //   */
        //    excelDoc.Write(startExcelXML);
        //    excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
        //    excelDoc.Write("<Table>");
        //    excelDoc.Write("<Row>");
        //    for (int x = 0; x < source.Columns.Count; x++)
        //    {
        //        excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
        //        excelDoc.Write(source.Columns[x].ColumnName);
        //        excelDoc.Write("</Data></Cell>");
        //    }
        //    excelDoc.Write("</Row>");
        //    foreach (DataRow x in source.Rows)
        //    {
        //        rowCount++;
        //        //if the number of rows is > 64000 create a new page to continue output
        //        if (rowCount == 64000)
        //        {
        //            rowCount = 0;
        //            sheetCount++;
        //            excelDoc.Write("</Table>");
        //            excelDoc.Write(" </Worksheet>");
        //            excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
        //            excelDoc.Write("<Table>");
        //        }
        //        excelDoc.Write("<Row>"); //ID=" + rowCount + "
        //        for (int y = 0; y < source.Columns.Count; y++)
        //        {
        //            System.Type rowType;
        //            rowType = x[y].GetType();
        //            switch (rowType.ToString())
        //            {
        //                case "System.String":
        //                    string XMLstring = x[y].ToString();
        //                    XMLstring = XMLstring.Trim();
        //                    XMLstring = XMLstring.Replace("&", "&");
        //                    XMLstring = XMLstring.Replace(">", ">");
        //                    XMLstring = XMLstring.Replace("<", "<");
        //                    excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
        //                                   "<Data ss:Type=\"String\">");
        //                    excelDoc.Write(XMLstring);
        //                    excelDoc.Write("</Data></Cell>");
        //                    break;
        //                case "System.DateTime":
        //                    //Excel has a specific Date Format of YYYY-MM-DD followed by  
        //                    //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
        //                    //The Following Code puts the date stored in XMLDate 
        //                    //to the format above
        //                    DateTime XMLDate = (DateTime)x[y];
        //                    string XMLDatetoString = ""; //Excel Converted Date
        //                    XMLDatetoString = XMLDate.Year.ToString() +
        //                                      "-" +
        //                                      (XMLDate.Month < 10 ? "0" +
        //                                                            XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
        //                                      "-" +
        //                                      (XMLDate.Day < 10 ? "0" +
        //                                                          XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
        //                                      "T" +
        //                                      (XMLDate.Hour < 10 ? "0" +
        //                                                           XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
        //                                      ":" +
        //                                      (XMLDate.Minute < 10 ? "0" +
        //                                                             XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
        //                                      ":" +
        //                                      (XMLDate.Second < 10 ? "0" +
        //                                                             XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
        //                                      ".000";
        //                    excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
        //                                   "<Data ss:Type=\"DateTime\">");
        //                    excelDoc.Write(XMLDatetoString);
        //                    excelDoc.Write("</Data></Cell>");
        //                    break;
        //                case "System.Boolean":
        //                    excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
        //                                   "<Data ss:Type=\"String\">");
        //                    excelDoc.Write(x[y].ToString());
        //                    excelDoc.Write("</Data></Cell>");
        //                    break;
        //                case "System.Int16":
        //                case "System.Int32":
        //                case "System.Int64":
        //                case "System.Byte":
        //                    excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
        //                                   "<Data ss:Type=\"Number\">");
        //                    excelDoc.Write(x[y].ToString());
        //                    excelDoc.Write("</Data></Cell>");
        //                    break;
        //                case "System.Decimal":
        //                case "System.Double":
        //                    excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
        //                                   "<Data ss:Type=\"Number\">");
        //                    excelDoc.Write(x[y].ToString());
        //                    excelDoc.Write("</Data></Cell>");
        //                    break;
        //                case "System.DBNull":
        //                    excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
        //                                   "<Data ss:Type=\"String\">");
        //                    excelDoc.Write("");
        //                    excelDoc.Write("</Data></Cell>");
        //                    break;
        //                default:
        //                    throw (new Exception(rowType.ToString() + " not handled."));
        //            }
        //        }
        //        excelDoc.Write("</Row>");
        //    }
        //    excelDoc.Write("</Table>");
        //    excelDoc.Write(" </Worksheet>");
        //    excelDoc.Write(endExcelXML);
        //    excelDoc.Close();
        //}
        /// <summary>
        /// ////////////////////////
        /// Process Tab Key to move an enter key to next tab
        /// <param name="FileName"></param>
        /// <returns></returns>

        public static byte[] ReadFileToBytes(string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] b = new byte[fs.Length];
            int i = (int)fs.Length;
            b = br.ReadBytes(i);
            br.Close();
            fs.Close();
            return b;
        }

        public static string[] csFileToStringArray(this string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    return File.ReadAllLines(fileName);
                }
            }
            catch //(Exception)
            {
            }
            return new string[0];
        }

        public static string ReadFileToString(string FileName)
        {
            return ReadFile(FileName);
        }

        public static string csReadFile(this string fileName)
        {
            return ReadFile(fileName);
        }

        public static string ReadFile(string FileName)
        {
            string s = string.Empty;
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs);
                    s = sr.ReadToEnd();
                }
            }
            catch
            {
                MyLib.csTell(string.Format("File {0} is Missing.", FileName));
            }

            return s;
        }

        public static string RelativeFilePath(string FileName) // returns a path relative to executeable
        {
            return GetRelativeFilePath(FileName);
        }

        public static string GetRelativeFilePath(string FileName) // returns a path relative to executeable
        {
            string temp = Path.GetDirectoryName(Application.ExecutablePath);
            if (string.IsNullOrEmpty(temp)) return string.Empty;
            return Path.Combine(temp, FileName);
        }
        public static string GetAppFolder()
        {
            return GetRelativeFolder();
        }
        public static string GetRelativeFolder()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        //public static string GetLocationFile(string link, string desc)
        //{ // returns a file name that can be run external
        //    string fms =
        //        "<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\"><html><head><meta content=\"text/html; charset=ISO-8859-1\"http-equiv=\"content-type\"><title></title></head><body><img style=\"width: 640px; height: 640px;\" alt=\"{1}\"src=\"{0}\"><br>\r\n{1}</body></html>";
        //    fms = string.Format(fms, link, desc);
        //    string fn = RelativeFilePath("E4661C4D-80F8-49F6-8D65-CBC41C66A082.html");
        //    File.Delete(fn);
        //    WriteFile(fn, fms);
        //    return fn;
        //}
        ////        private const string showJPGImage =
        ////        @"<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">
        ////<html>
        ////<head>
        ////  <meta content=""text/html; charset=ISO-8859-1""
        //// http-equiv=""content-type"">
        ////  <title>::TITLE::</title>
        ////</head>
        ////<body>
        ////<img
        //// src=::FILENAME::
        //// alt=""""><br>
        ////Document Description - ::DESCRIPTION::
        ////</body>
        ////</html>
        ////";
        ////  public static void Show
        //public static void GetMapList(List<sAddress> list, int zoom, string color)
        //{
        //    int len = 0;
        //    int n = list.Count;
        //    n = Math.Min(50, n);
        //    string link;
        //    do
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < n; i++)
        //        {
        //            string s = string.Format("{0} {1} {2}|", list[i].no, list[i].street, GetCity()).Trim();
        //            s = s.Replace(" ", "+");
        //            if (sb.Length == 0) sb.Append("|");
        //            sb.Append(s);
        //        }
        //        string al = sb.ToString();
        //        al = al.Substring(0, al.Length - 1);
        //        link =
        //            string.Format(
        //                @"http://maps.google.com/maps/api/staticmap?zoom={4}&size={1}x{2}&maptype={3}&markers=color:{5}|label:X{0}&sensor=false"
        //                , al // tbAddressNo.Text //0
        //                , 640 //1
        //                , 640 //2
        //                , "roadmap" //3
        //                , zoom // 4
        //                , color //5

        //                );
        //        link = link.Replace(" ", "+");
        //        len = link.Length;
        //        n--;
        //    } while (len > 2048);
        //    //       len.csTell();
        //    link = link.Replace(" ", "+");
        //    Clipboard.SetText(link);

        //    link = MyLib.GetLocationFile(link, string.Empty/*string.Format("{0} {1}", address, street).Trim()*/);
        //  MyLib.RunExternal(link);
        //}
        //public static void GetMap(string labelText, double Lat, double Lng, int zoom = 14, string color = "blue")
        //{
        //    string link =
        //        string.Format(@"http://maps.google.com/maps/api/staticmap?zoom={5}&size={2}x{3}&maptype={4}&markers=color:{6}|label:X|{0},{1}|&sensor=false"
        //                      , Lat // tbAddressNo.Text //0
        //                      , Lng //cbStreets.Text //1
        //                      , 640 //2
        //                      , 640 //3
        //                      , "roadmap"  //4
        //                      , zoom // zoom //5
        //                      , color //6 "blue"

        //            );
        //    DrawMap(link, labelText);
        //}
        //public static void GetMap(string address, string street, int zoom, string color)
        //{
        //    string link =
        //        string.Format(@"http://maps.google.com/maps/api/staticmap?zoom={6}&size={3}x{4}&maptype={5}&markers=color:{7}|label:X|{0}+{1}+{2}|&sensor=false"
        //                      , address // tbAddressNo.Text //0
        //                      , street //cbStreets.Text //1
        //                      , GetCity() //2
        //                      , 640 //3
        //                      , 640 //4
        //                      , "roadmap"                   // GetMapType() //5
        //                      , zoom // zoom //6
        //                      , color //"blue" // 7 color

        //            );
        //    DrawMap(link, address, street);
        //}
        //// path map
        ////public static void GetMap(string p, sGeoPoint p1, sGeoPoint p2, int zoom, string color)
        ////{
        ////    string path = string.Format("path=color:red|{0},{1}|{2},{3}", p1.Lat, p1.Lng, p2.Lat, p2.Lng);
        ////    string link =
        ////        string.Format(@"http://maps.google.com/maps/api/staticmap?zoom={5}&size={2}x{3}&maptype={4}&markers=color:{6}|label:U|{0},{1}&{7}&sensor=false"
        ////                      , p1.Lat // tbAddressNo.Text //0
        ////                      , p1.Lng //cbStreets.Text //1
        ////                      , 640 //2
        ////                      , 640 //3
        ////                      , "roadmap"  //4
        ////                      , zoom // zoom //5
        ////                      , color //6 "blue"
        ////                      , path

        ////            );
        ////    DrawMap(link, p1.Name, p2.Name);
        ////}

        //        private static void DrawMap(string link, string startingtext, string endingtext = "")
        //        {
        //            link = link.Replace(" ", "+");
        //            link = MyLib.GetLocationFile(link, string.Format("{0} {1}", startingtext, endingtext).Trim());
        //           MyLib.RunExternal(link);
        //        }

        //        private static string GetCity()
        //        {
        //            string s = string.Empty;
        //#if FAIRFIELD
        //            s = "Fairfield+ohio";// "45014";// "Fairfield+ohio";
        //            return s;
        //#endif
        //#if WILMINGTON
        //            s = "wilmington+ohio";
        //#endif
        //            return s;
        //        }
        public static void WriteFile(string FileName, byte[] b)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(b);
            bw.Flush();
            bw.Close();
            fs.Dispose();
        }

        public static void WriteFile(string filename, string content)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            WriteFile(filename, encoding.GetBytes(content));
        }

        public static string StrToNumeric(string s) // removes all but digits
        {
            for (int i = s.Length - 1; i > -1; i--)
            {
                char c = s[i];
                bool good = char.IsDigit(c);
                if (!good)
                {
                    s = s.Remove(i, 1);
                }
            }
            return s;
        }

        public static string csFormatPhone(this string s)
        {
            return FormatPhone(s);
        }

        public static string FormatPhone(string p)
        {
            // find first non phone character
            p = p.Trim();
            int index = p.Length;
            for (int i = 0; i < p.Length; i++)
            {
                char c = p.ToCharArray()[i];
                if (System.Char.IsDigit(c) || "()- ".Contains(c)) continue;
                index = i;
                break;
            }
            if (index < p.Length)
            {
                return string.Format("{0} {1}", FormatPhone(p.Substring(0, index)), p.Substring(index));
            }
            List<string> chars = new List<string>() { "(", ")", "-" };

            p = StrToNumeric(p);
            if (p.Length == 10)
            {
                return string.Format("({0}) {1}-{2}", p.Substring(0, 3), p.Substring(3, 3), p.Substring(6));
            }
            else if (p.Length == 7)
            {
                return string.Format("{0}-{1}", p.Substring(0, 3), p.Substring(3));
            }
            else
            {
                if ((p.Length > 10) && (p[0] == '1'))
                {
                    p = p.Substring(1);
                    return FormatPhone(p);
                }
                else
                {
                    return p;
                }
            }
        }

        public static string csToPhone(this string s)
        {
            return FormatPhone(s);
        }

        public static string csFileNames(string App_or_User)
        {
            switch (App_or_User)
            {
                case "App":
                    return Application.StartupPath;
                case "User":
                    return Application.LocalUserAppDataPath; //CommonAppDataPath;
                default:
                    return Application.StartupPath;
            }
        }

        public static string csFileNames(string App_or_User, string FileName)
        {
            return string.Format(@"{0}\{1}", csFileNames(App_or_User), FileName);
        }

        public static DateTime StartOfYear(DateTime d)
        {
            return d.AddDays(-d.DayOfYear + 1).Date;
        }

        public static DateTime EndOfYear(DateTime d)
        {
            return StartOfYear(d).AddYears(1).AddMilliseconds(-1);
        }

        public static DateTime StartOfMonth(DateTime d)
        {
            return d.AddDays(-d.Day + 1).Date;
        }

        public static DateTime EndOfMonth(DateTime d)
        {
            DateTime t = StartOfMonth(d).AddMonths(1);
            return t.AddMilliseconds(-1);
        }

        public static void SetCheckBoxList(List<CheckBox> DataFieldList, int value)
        {
            foreach (CheckBox c in DataFieldList)
            {
                if (c.Tag != null)
                {
                    int k;
                    string s = (string)c.Tag;
                    int.TryParse(s, System.Globalization.NumberStyles.Any, null, out k);
                    c.Checked = ((value & (1 << k)) != 0);
                }
            }
        }

        public static int CheckBoxListToInt(List<CheckBox> DataFieldList)
        {
            int i = 0;
            foreach (CheckBox c in DataFieldList)
            {
                if (c.Checked)
                {
                    int k;
                    int.TryParse((string)c.Tag, System.Globalization.NumberStyles.Any, null, out k);
                    int index = (1 << k);
                    i |= index;
                }
            }
            i |= (1 << 9); // add the date field
            return i;
        }


        /// <summary>
        /// Converts an image file (e.g. 32*32 pixel JPEG) to an icon file
        /// </summary>
        /// <param name="filename"></param>
        public static void ImageToIcon(string filename)
        {
            Bitmap bm = new Bitmap(filename);
            Icon icon = Icon.FromHandle(bm.GetHicon());
            filename = Path.ChangeExtension(filename, ".ico");
            Stream stream = new FileStream(filename, FileMode.Create);
            icon.Save(stream);
        }

        public static string GetUserDataFileName(string filename)
        {
            return RelativeFilePath(filename);
        }

        public static string GetLocalUserPrefFile(string folder, string filename)
        {
            return GetLocalUserDataFileName(folder, filename);
        }

        public static string GetLocalUserDataFileName(string folder, string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fn = string.Format(@"{0}\{1}\{2}", path, folder, filename);
            path = Path.GetDirectoryName(fn);
            if (path == null) return string.Empty;
            Directory.CreateDirectory(path);
            return fn;
        }

        public static string GetFolderPath(string path = @"C:\Temp")
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.SelectedPath = path;
            d.ShowDialog();
            return d.SelectedPath;
        }

        public static string GetDayOfWeek(int i)
        {
            i = i % 7;
            string[] dow = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return dow[i];
        }

        public static Color GetColor(Color ic)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = ic;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                return cd.Color;
            }
            else
            {
                return ic;
            }
        }

        public static string GetOpenFilename(string startingPath, string default_extension3chars,
                                             string FFF_OR_ASKPointExt)
        {
            return GetOpenFilename("Open File", startingPath, default_extension3chars, FFF_OR_ASKPointExt);
        }

        public static string GetFileName(string startingPath, string default_extension3chars,
                                         string FFF_OR_ASKPointExt = "")
        {
            return GetOpenFilename("Open File", startingPath, default_extension3chars, FFF_OR_ASKPointExt);
        }

        public static StreamWriter GetTempFile(string path = @"D:\temp")
        {
            string fn = string.Format(@"{3}\{0}{1:00}{2:00}.txt", DateTime.Now.Year, DateTime.Now.Month,
                                      DateTime.Now.Day, path);
            File.Delete(fn);
            return new StreamWriter(fn);
        }

        public static string GetOpenFilename(string caption, string startingPath, string default_extension,
                                             string filter = "")
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = caption;
            if (filter.Length > 0)
            {

                d.Filter = filter;
            }
            else
            {
                d.Filter = "ALL | *.*";
            }
            d.FilterIndex = 1;
            d.DefaultExt = default_extension;
            d.InitialDirectory = startingPath;
            if (d.ShowDialog() == DialogResult.OK)
            {
                return d.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetOpenFileName(string path, string fileextension, string filetypefilter, string boxtitle,
                                             string filename)
        {
            List<string> filter = new List<string>();
            string ft = filetypefilter.ToLower();
            if (ft.Contains("excel"))
            {
                filter.Add("Excel|*.xls");
            }
            if (ft.Contains("word"))
            {
                filter.Add("Word|*.doc");
            }
            if (ft.Contains("text"))
            {
                filter.Add("Text|*.txt");
            }
            if (ft.Contains("csv"))
            {
                filter.Add("CSV|*.csv");
            }
            ft = string.Empty;
            foreach (string s in filter)
            {
                ft += (s + "|");
            }
            ft += "All|*.*";

            OpenFileDialog dl = new OpenFileDialog();
            dl.AddExtension = true;
            dl.CheckFileExists = false;
            dl.DefaultExt = fileextension;
            dl.Filter = ft;
            dl.FilterIndex = 0;
            dl.InitialDirectory = path;
            dl.RestoreDirectory = false;
            dl.Title = boxtitle;
            dl.FileName = filename;
            if (dl.ShowDialog() == DialogResult.OK)
            {
                return dl.FileName;
            }
            else
            {
                return string.Empty;
            }
        }


        public static string GetSaveFileName(string path, string fileextension, string filetypefilter, string boxtitle,
                                             string filename)
        {
            List<string> filter = new List<string>();
            string ft = filetypefilter.ToLower();
            if (ft.Contains("excel"))
            {
                filter.Add("Excel|*.xls");
            }
            if (ft.Contains("word"))
            {
                filter.Add("Word|*.doc");
            }
            if (ft.Contains("text"))
            {
                filter.Add("Text|*.txt");
            }
            if (ft.Contains("csv"))
            {
                filter.Add("CSV|*.csv");
            }
            if (ft.Contains("png"))
            {
                filter.Add("PNG|*.png");
            }
            ft = string.Empty;
            foreach (string s in filter)
            {
                ft += (s + "|");
            }
            ft += "All|*.*";

            SaveFileDialog dl = new SaveFileDialog();
            dl.AddExtension = true;
            dl.CheckFileExists = false;
            dl.DefaultExt = fileextension;
            dl.Filter = ft;
            dl.FilterIndex = 0;
            dl.InitialDirectory = path;
            dl.OverwritePrompt = true;
            dl.RestoreDirectory = false;
            dl.Title = boxtitle;
            dl.FileName = filename;
            if (dl.ShowDialog() == DialogResult.OK)
            {
                return dl.FileName;
            }
            else
            {
                return string.Empty;
            }
        }
        public static void TabForward(object sender)
        {
            Control c = sender as Control;
            if (c == null) return;
            int index = c.TabIndex;
            Dictionary<int, Control> list = new Dictionary<int, Control>();
            foreach (Control k in c.Parent.Controls)
            {
                if (k.Visible && (k.TabIndex > index) && (k.TabStop))
                {
                    if (!list.ContainsKey(k.TabIndex)) list.Add(k.TabIndex, k);
                }
            }
            if (list.Count > 0)
            {
                List<int> t = new List<int>(list.Keys);
                t.Sort();
                list[t[0]].Focus();
            }
        }

        private static Cursor cursorState = Cursor.Current;

        public static void csDragEnter(this Control tb, Cursor cursor, DragEventArgs e)
        {
            cursorState = cursor;
            e.Effect = DragDropEffects.Copy;
        }

        public static void csDragEnter(this TextBox tb, Cursor cursor, DragEventArgs e)
        {
            cursorState = cursor;
            e.Effect = DragDropEffects.Copy;
        }

        public static void csDragEnter(object picLogo, Cursor cursor, DragEventArgs e)
        {
            cursorState = cursor;
            e.Effect = DragDropEffects.Link;
        }

        public static void csDragDropLeave(this Control tb)
        {
            tb.Cursor = cursorState;
        }

        public static void csDragDropLeave(this TextBox tb)
        {
            tb.Cursor = cursorState;
        }

        public static void csDragDropLeave(Form form)
        {
            form.Cursor = cursorState;
        }

        public static void csDragDropString(this TextBox tb, DragEventArgs e)
        {
            tb.Text = DragDropString(e);

        }

        public static string DragDropString(DragEventArgs e, bool AddToClipBoard = false)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                return (string)e.Data.GetData(DataFormats.FileDrop);
            return string.Empty;
        }

        public static string csDragDropFileName(object sender, DragEventArgs e)
        {
            return DragDropFileName(e);
        }

        public static void csDisplayList(this TextBox tb, List<string> list)
        {
            tb.Clear();
            tb.Text = string.Join(Environment.NewLine, list);
        }

        public static void csDragDropFileName(this TextBox tb, DragEventArgs e)
        {
            tb.Text = DragDropFileName(e);
        }

        public static string DragDropFileName(DragEventArgs e, bool AddToClipBoard = false)
        {
            try
            {
                List<string> temp = new List<string>();
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files.Length > 0)
                    {
                        string fn = files[0];
                        //         fn.csTell();
                        return fn;
                    }
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
#else
            catch
            {
            }
#endif
            return string.Empty;
        }

        public static List<string> csDragDropFileList(this DragEventArgs e, bool AddToClipBoard = false)
        {
            return csDragDropGetFileNameList(e, AddToClipBoard);
        }

        public static List<string> csDragDropGetFileNameList(this DragEventArgs e, bool AddToClipBoard = false)
        {
            try
            {
                List<string> temp = new List<string>();
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    return files.ToList();
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
#else
            catch
            {
            }
#endif
            return new List<string>();
        }

        public static List<string> DragDropTextboxProcessing(DragEventArgs e, bool AddToClipBoard = false)
        {
            try
            {
                List<string> temp = new List<string>();
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    temp.AddRange(files.Select(s => s));
                }
                if (AddToClipBoard) temp.csList2Clipboard(1);
                return temp;
            }
#if DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
#else
            catch
            {
                return null;
            }
#endif
        }

        public static float ToFloat(string s)
        {
            float f = 0.0F;
            float.TryParse(s, System.Globalization.NumberStyles.Any, null, out f);
            return f;
        }

        public static int ToInt(string s)
        {
            int i;
            int.TryParse(s, System.Globalization.NumberStyles.Any, null, out i);
            return i;
        }

        public static decimal ToDecimal(string s)
        {
            decimal d = 0.0M;
            decimal.TryParse(s, System.Globalization.NumberStyles.Any, null, out d);
            return d;
        }

        public static double ToDouble(string s)
        {
            double d = 0.0;
            double.TryParse(s, System.Globalization.NumberStyles.Any, null, out d);
            return d;
        }

        public static DateTime MJCDate()
        {
            return new DateTime(1942, 2, 23);
        }

        public static DateTime StandardDate
        {
            get { return MJCDate(); }
        }

        public static string NewGUID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public static DateTime MJCDate(int yr)
        {
            return new DateTime(yr, 1, 1);
        }

        public static DateTime TicksToTime(long ticks)
        {
            return MJCDate(2000).AddTicks(ticks);
        }

        public static long TimeToTicks()
        {
            return DateTime.Now.Ticks - MJCDate(2000).Ticks;
        }

        public static DateTime TicksToTime(long ticks, int yr)
        {
            return new DateTime(yr, 1, 1).AddTicks(ticks);
            ;
        }

        public static long TimeToTicks(DateTime n, int year)
        {
            return n.Ticks - MJCDate(year).Ticks;
        }

        public static long TimeToTicks(int year)
        {
            return DateTime.Now.Ticks - MJCDate(year).Ticks;
        }

        public static string UserFolder(string fileName)
        {
            string fn = csFileNames("User");
            int loc = fn.IndexOf(@"\Data");
            //    MyLib.csTell(string.Format("{0} -- loc {1}",fn,loc));
            if (loc > 3)
            {
                fn = fn.Substring(0, loc);
            }
            return Path.Combine(fn, fileName);
        }

        public static int GetMJCDays()
        {
            TimeSpan ts = DateTime.Now - MJCDate();
            return ts.Days;
        }

        public static void ShowStoreSplash()
        {
#if(DESKTOP)
            {
            SplashStoreScteen form = new SplashStoreScteen();
            form.ShowDialog();
            }
#endif
        }

        public static int GetNewElapseTime()
        {
            Random r = new Random();
            return r.Next(30, 60);
        }

        public static string GetNameLastFirst(string firstname, string lastname)
        {
            string s;
            if (firstname.Length > 0)
            {
                s = string.Format("{0}, {1}", lastname, firstname);
            }
            else
            {
                s = lastname;
            }
            return s.Trim();
        }

        public static string GetSaveFileName(string path, string ext, string boxtitle)
        {
            SaveFileDialog dl = new SaveFileDialog();
            dl.AddExtension = true;
            dl.CheckFileExists = false;
            dl.DefaultExt = ext;
            dl.Filter = string.Format("{0}|*.{0}|All|*.*", ext);
            dl.FilterIndex = 0;
            dl.InitialDirectory = path;
            dl.OverwritePrompt = true;
            dl.RestoreDirectory = false;
            dl.Title = boxtitle;
            if (dl.ShowDialog() == DialogResult.OK)
            {
                return dl.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetSaveFileName(string path, List<string> exts, string boxtitle)
        {
            SaveFileDialog dl = new SaveFileDialog();
            if (exts.Count < 1)
            {
                exts.Add("txt");
            }
            dl.DefaultExt = exts[0];
            StringBuilder sb = new StringBuilder();
            foreach (string s in exts)
            {
                sb.Append(string.Format("{0}|*.{0}|", s));
            }
            sb.Append("All|*.*");
            try
            {
                dl.Filter = sb.ToString();
                dl.FilterIndex = 0;
                dl.InitialDirectory = path;
                dl.OverwritePrompt = true;
                dl.RestoreDirectory = false;
                dl.Title = boxtitle;
                dl.AddExtension = true;
                dl.CheckFileExists = false;
                if (dl.ShowDialog() == DialogResult.OK)
                {
                    return dl.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                MyLib.csTell(sb.ToString() + Environment.NewLine + e.Message);
                return string.Empty;
            }
        }

        public static string FormatSSN(string p)
        {
            return string.Format("{0}-{1}-{2}", p.Substring(0, 3), p.Substring(3, 2), p.Substring(5, 4));
        }

        public static string RemoveChar(string s, List<string> chars)
        {
            foreach (var c in chars)
            {
                s = s.Replace(c, string.Empty);
            }
            return s;
        }

        public static string GetOnlyDigits(string p)
        {
            string s = string.Empty;
            for (int i = 0; i < p.Length; i++)
            {
                if (System.Char.IsDigit(p, i))
                {
                    s += p[i];
                }
            }
            return s;
        }

        public static DateTime StrToDate(string ds)
        {
            DateTime d;
            if (!DateTime.TryParse(ds, out d) || (d < MJCDate()))
            {
                d = DateTime.MinValue;
            }
            return d;
        }

        public static void SetButtonColor(Control.ControlCollection controlCollection, Color color)
        {
            foreach (Control c in controlCollection)
            {
                if (c is Button)
                {
                    if ((c.Tag == null) || (c.Tag.ToString() != "-1"))
                    {
                        (c as Button).csSetColor(color);
                        Font f = new Font(c.Font.FontFamily, c.Font.SizeInPoints, FontStyle.Bold);
                        c.Font = f;
                    }
                    c.Refresh();
                }
                else
                {
                    SetButtonColor(c.Controls, color);
                }
            }
        }

        public static void SetButtonColor(Control.ControlCollection controlCollection)
        {
            SetButtonColor(controlCollection, Color.WhiteSmoke);
        }

        public static void csSetButtonsGreen(this Control.ControlCollection controlCollection)
        {
            SetButtonsGreen(controlCollection);
        }

        public static void csSetButtonsGreen(this Form form)
        {
            SetButtonColor(form.Controls, Color.LightGreen);
        }

        public static void SetButtonsGreen(Control.ControlCollection controlCollection)
        {
            SetButtonColor(controlCollection, Color.LightGreen);
        }

        public static void SetButtonColor(object sender, Color color)
        {
            var btn = (sender as Button);
            if (btn == null) return;
            btn.BackColor = color;
        }

        public static void SetUpTextboxLabels(Control.ControlCollection Controls)
        {
            foreach (Control c in Controls)
            {
                TextBox t = c as TextBox;
                if (t == null)
                {
                    SetUpTextboxLabels(c.Controls);
                }
                else
                {
                    string s = t.Text;
                    Label l = new Label();
                    l.Parent = t.Parent;
                    l.AutoSize = true;
                    l.Text = s;
                    l.Left = t.Left;
                    l.Top = t.Top - l.Height;
                }
            }
        }

        public static string GetXML(XElement doc, string name)
        {
            var temp = doc.Elements().Where(t => t.Name == name).FirstOrDefault();
            return temp == null ? string.Empty : temp.Value;
        }

        public static string csGetXML(this XElement doc, string name, string defaultvalue = null)
        {
            if (defaultvalue == null) defaultvalue = string.Empty;
            return GetXML(doc, name, defaultvalue);
        }

        public static List<XElement> csGetXMLElements(this XElement doc, string name)
        {
            return doc.Elements().Where(t => t.Name == name).ToList();
        }

        public static string GetXML(XElement doc, string name, string defaultvalue)
        {
            string s = GetXML(doc, name);
            return s == string.Empty ? defaultvalue : s;
        }

        public static string GetXML(XElement doc, string name, string value, ref bool resave)
        {
            string s = GetXML(doc, name);
            if (s == string.Empty && value != string.Empty)
            {
                SetXML(doc, name, value);
                resave = true;
                s = value;
            }
            return s;
        }

        public static void SetXML(XElement doc, string name, string value)
        {
            var temp = doc.Elements().Where(t => t.Name == name).FirstOrDefault();
            if (temp == null)
            {
                doc.Add(new XElement(name, value));
            }
            else
            {
                temp.Value = value;
            }
        }

        public static void csSetXML(this XElement doc, string name, string value)
        {
            SetXML(doc, name, value);
        }

        public static void csMakeXML(this XElement element, string name, string value)
        {
            element.Add(new XElement(name, value));
        }

        public static bool csIncludes(this DateTime current, DateTime lower, DateTime upper)
        {
            return (current >= lower) && (current < upper);
        }
        public static void csImport(this DataTable table, DataTable source)
        {
            CopyTable(table, source);
        }
        public static void CopyTable(DataTable dataTable, DataTable sourcetable)
        {
            CopyTable(dataTable, sourcetable, false);
        }
        public static void CopyTable(DataTable dataTable, DataView sourcedv, bool maintainorder)
        {
            dataTable.Clear();
            AddToTable(dataTable, sourcedv, maintainorder);
        }
        public static void CopyTable(DataTable table, List<DataRow> temp)
        {
            table.Clear();
            DataRow row;
            foreach (DataRow r in temp.AsEnumerable())
            {
                row = table.NewRow();
                row.ItemArray = r.ItemArray;
                table.Rows.Add(row);
            }
        }
        public static void AddToTable(DataTable dataTable, DataTable sourcetable)
        {
            if (sourcetable == null) return;
            foreach (DataRow r in sourcetable.Rows)
            {
                DataRow row = dataTable.NewRow();
                row.ItemArray = r.ItemArray;
                dataTable.Rows.Add(row);
            }
            dataTable.AcceptChanges();
        }
        public static void AddToTable(DataTable dataTable, DataView sourcedv, bool maintainorder)
        {
            if ((sourcedv == null) || (sourcedv.Count == 0)) return;
            DataRow r = sourcedv[0].Row;
            if (!r.Table.Columns.Contains("SortOrder")) maintainorder = false;
            int i = 1;
            for (int k = 0; k < sourcedv.Count; k++)
            {
                r = sourcedv[k].Row;
                if (r.RowState == DataRowState.Deleted) continue;
                DataRow row = dataTable.NewRow();
                row.ItemArray = r.ItemArray;

                if (maintainorder)
                {
                    row.BeginEdit();
                    row["SortOrder"] = i++;
                    row.EndEdit();
                }
                dataTable.Rows.Add(row);
            }
            dataTable.AcceptChanges();
        }
        public static void CopyTable(DataTable dataTable, DataTable sourcetable, bool maintainorder)
        {
            dataTable.Clear();
            if (sourcetable == null) return;
            int i = 1;
            foreach (DataRow r in sourcetable.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                DataRow row = dataTable.NewRow();
                row.ItemArray = r.ItemArray;
                if (maintainorder)
                {
                    row.BeginEdit();
                    row["RID"] = i++;
                    row.EndEdit();
                }
                dataTable.Rows.Add(row);
            }
            dataTable.AcceptChanges();
        }
        public static void CopyTableRxR(DataTable dataTable, DataTable sourcetable,
                                        Dictionary<string, string> ColumnMap = null)
        {
            dataTable.Clear();
            if (sourcetable == null) return;
            int columnCount = dataTable.Columns.Count;
            int[] rowconv = new int[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                rowconv[i] = -1;
                string toCol = dataTable.Columns[i].ColumnName;
                if (sourcetable.Columns.Contains(toCol))
                {
                    rowconv[i] = sourcetable.Columns[toCol].Ordinal;
                }
            }

            if (ColumnMap != null)
            {
                foreach (
                    string k in
                        ColumnMap.Keys.Where(
                            k => (dataTable.Columns.Contains(k)) && (sourcetable.Columns.Contains(ColumnMap[k]))))
                {
                    rowconv[dataTable.Columns[k].Ordinal] = sourcetable.Columns[ColumnMap[k]].Ordinal;
                }
            }
            foreach (DataRow r in sourcetable.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                DataRow row = dataTable.NewRow();
                row.BeginEdit();
                for (int i = 0; i < columnCount; i++)
                {
                    if (rowconv[i] == -1) continue;
                    row[i] = r[rowconv[i]];
                }
                row.EndEdit();
                dataTable.Rows.Add(row);
            }
        }
        public static void CopyTableRxR(DataTable table, List<DataRow> rowList)
        {
            if (rowList.Count == 0) return;
            var temp = rowList[0];
            int columnCount = table.Columns.Count;
            int[] rowconv = new int[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                rowconv[i] = -1;
                string toCol = table.Columns[i].ColumnName;
                if (temp.Table.Columns.Contains(toCol))
                {
                    rowconv[i] = temp.Table.Columns[toCol].Ordinal;
                }
            }
            foreach (DataRow r in rowList)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                DataRow row = table.NewRow();
                row.BeginEdit();
                for (int i = 0; i < columnCount; i++)
                {
                    if (rowconv[i] == -1) continue;
                    row[i] = r[rowconv[i]];
                }
                row.EndEdit();
                table.Rows.Add(row);
            }
        }
        public static string csAddTrailingChar(this string path, string final = @"\")
        {
            if (!path.EndsWith(final)) path += final;
            return path;
        }
        public static string csGetFileName(this string path)
        {
            return (new FileInfo(path)).Name;
        }
        public static string csChangeCase(this string s)
        {
            if (s.Length < 1) return s;
            s = s.ToLower();
            string f = s.ToUpper();
            if (f.Length == 1) return f;
            return f[0] + s.Substring(1);
        }
        public static string GetCurrentServer()
        {
            return System.Environment.MachineName;
        }
        public static void ShowFBServerConnectionString(string constr)
        {
            int first = constr.IndexOf("password=");
            string s = constr;
            if (first > -1)
            {
                s = constr.Substring(0, first);
                string temp = constr.Substring(first);
                first = temp.IndexOf(";");
                if (first > -1)
                    s = s + temp.Substring(first + 1);
            }
            s.csTell();
        }
        public static void SetMain(Form main)
        {
            if (main.Width < 200) main.Width = 200;
            if (main.Height < 200) main.Height = 200;
        }
        public static void csSetFormS_L(this Form main, int width = 300, int heigth = 300)
        {
            SetFormS_L(main, width, heigth);
        }
        public static void SetFormS_L(Form main, int width = 300, int heigth = 300)
        {
            var screen = System.Windows.Forms.Screen.PrimaryScreen;
            bool move = false;
            int x = main.Location.X;
            int y = main.Location.Y;
            if ((x < 0) || (x > screen.Bounds.Width / 2))
            {
                x = screen.Bounds.Width / 4;
                move = true;
            }
            if ((y < 0) || (y > screen.Bounds.Height / 2))
            {
                y = screen.Bounds.Height / 4;
                move = true;
            }
            if (move) main.Location = new Point(x, y);
            x = main.Size.Width;
            y = main.Size.Height;
            if ((x < width) || (y < heigth))
            {
                main.Size = new Size(width, heigth);
            }
        }
        public static string csBytesToString(this byte[] bytes)
        {
            return ConvertBytesToString(bytes);
        }
        public static string ConvertBytesToString(byte[] buf)
        {
            buf = Encoding.Convert(Encoding.GetEncoding( /*"iso-8859-1"*/"Windows-1252"), Encoding.UTF8, buf);
            int count = buf.Length;
            return Encoding.UTF8.GetString(buf, 0, count);
        }
        ///////////////////////////////////////////////////////
        /// 20130421
        /// ///////////////////////////////////////////////////
        public static bool IsDevelopmentMachine
        {
            get { return GetCurrentServer().ToLower().Contains("neptune"); }
        }
        public static void csClearControls(this Control control)
        {
            foreach (Control c in control.Controls)
            {
                csClearControls(c);
                var box = c as TextBox;
                if (box != null)
                {
                    box.Text = string.Empty;
                    continue;
                }
                var comboBox = c as ComboBoxEdit;
                if (comboBox != null)
                {
                    comboBox.SelectedIndex = -1;
                    continue;
                }
                var cb = c as ComboBox;
                if (cb != null)
                {
                    cb.SelectedIndex = -1;
                }
            }
        }
        public static string csToString(this double v, string fmt)
        {
            string format = "{0:" + fmt + "}";
            return string.Format(format, v);
        }
        // 130508
        public static string GetUserID()
        {
            return UID;
        }
        public static void csSetColorAndCursor(this Button btn, Form form, int c = 0)
        {
            Color color = Color.Red;
            Cursor cur = Cursors.WaitCursor;
            if (c != 0)
            {
                color = Color.FromArgb(c);
                cur = Cursors.Default;
            }
            btn.BackColor = color;
            btn.Refresh();
            if (form != null)
            {
                form.Cursor = cur;
            }

        }
        public static void csSetColorAndCursor(this object _btn, Form form, int c = 0)
        {

            Button btn = _btn as Button;
            if (btn == null) return;
            Color color = Color.Red;
            Cursor cur = Cursors.WaitCursor;
            if (c != 0)
            {
                color = Color.FromArgb(c);
                cur = Cursors.Default;
            }
            btn.BackColor = color;
            btn.Refresh();
            if (form != null)
            {
                form.Cursor = cur;
            }
        }

        public static void csSetColorAndCursor(this object _btn, Form form, bool isWait)
        {

            Button btn = _btn as Button;
            if (btn == null) return;
            btn.BackColor = isWait ? Color.Red : Color.Yellow;
            btn.Refresh();
            form.Cursor = isWait ? Cursors.WaitCursor : Cursors.Default;
        }

        //public static void csSetColor(this Button btn, Form form = null)
        //{
        //    btn.csSetColor(Color.LightGreen);
        //    if (form != null) form.Cursor = Cursors.Default;
        //}
        //public static void csSetColor(this Button btn, Color c)
        //{
        //    btn.BackColor = c;
        //}

    }

    public class sAddress
    {
        public string no;
        public string street;

        public sAddress()
        {
            no = string.Empty;
            street = string.Empty;
        }
    }

    public static class TWriteGrid
    {
        public static void writeGridtoCSV(string FileName, DataGridView Grid, string Title)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create);
            StreamWriter f = new StreamWriter(fs);
            f.WriteLine("," + Title);
            writeGridtoCSV(f, Grid);
        }

        private static DataGridView DG = null;

        public static void writeGridtoCSV(StreamWriter stream, DataGridView Grid)
        {
            DG = Grid;
            StreamWriter f = stream;
            StringBuilder sb = new StringBuilder();
            sb.Length = 0;
            string s;
            List<int> Printcols = new List<int>();
            for (int i = 0; i < Grid.ColumnCount; i++)
            {
                if (Grid.Columns[i].Visible)
                {
                    Printcols.Add(i);
                }
            }
            Printcols.Sort(CompareColDisplayValues);
            foreach (int col in Printcols)
            {
                s = Grid.Columns[col].HeaderText;
                sb.Append("\"" + s + "\",");
            }
            s = sb.ToString();
            f.WriteLine(s);

            foreach (DataGridViewRow row in Grid.Rows)
            {
                sb.Length = 0;
                foreach (int col in Printcols)
                {
                    s = "";
                    if (row.Cells[col].Value != null)
                    {
                        s = (string)row.Cells[col].FormattedValue;
                    }
                    sb.Append("\"" + s + "\",");
                }
                s = sb.ToString();
                f.WriteLine(s);
            }
            f.Flush();
            f.Close();
            f.Dispose();
        }

        private static int CompareColDisplayValues(int a, int b)
        {
            int p1 = DG.Columns[a].DisplayIndex;
            int p2 = DG.Columns[b].DisplayIndex;
            return p1 - p2;
        }

        public static void writeGridtoCSV(string FileName, DataGridView Grid)
        {
            /*if (File.Exists(FileName))
            {
                File.Delete(FileName);
            } */
            FileStream fs = new FileStream(FileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            writeGridtoCSV(sw, Grid);
        }
    }

    public class TCSVReader : IEnumerator
    {
        private StreamReader FFile;

        public TCSVReader(StreamReader File, bool HasHeaders)
        {
            FFile = File;
            FHasHeaders = HasHeaders;
            ProcessFile();
        }

        private int maxfieldLength = 100;

        public int MaxLength
        {
            get { return maxfieldLength; }
            set { maxfieldLength = value; }
        }

        public bool FieldLimit { get; set; }

        private bool FHasHeaders;
        private List<string> Headers = new List<string>();

        public List<string> FieldHeaders
        {
            get { return Headers; }
        }

        public List<List<string>> Records { get; private set; }
        public List<string> Record { get; private set; }
        private int position;

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        private List<string> ReadLine(StreamReader file)
        {
            char[] eol = new char[2];
            file.Read(eol, 0, 2);
            bool inQuote = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(eol);
            string seol = sb.ToString();
            if (seol.Contains('"'))
            {
                if (seol[0] == '"')
                {
                    if (seol[1] != '"')
                    {
                        inQuote = true;
                    }
                }
                else if (seol[1] == '"')
                {
                    inQuote = true;
                }
            }
            sb.Length = 0;
            while ((!file.EndOfStream))
            {
                if (!inQuote && (seol != Environment.NewLine))
                {
                    seol = MoveTape(file, eol, sb);
                    if (eol[1] == '"') inQuote = true;
                }
                else if (inQuote)
                {
                    seol = MoveTape(file, eol, sb);
                    if (eol[1] == '"') inQuote = false;
                }
                else // must be a new line
                {
                    return ParseCSV(sb.ToString());
                }
            }
            return ParseCSV(sb.ToString());
        }

        public static List<string> ParseCSV(string line) // csv line
        {
            List<string> sList = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            sb.Append('"');
            string dq = sb.ToString();
            sb = new StringBuilder();
            bool inQuotes = false;
            string temp;
            for (int i = 0; i < line.Length; i++)
            {
                char tapehead = line[i];
                if (tapehead == '"') inQuotes = !inQuotes;
                if ((tapehead == ',') && !inQuotes)
                {
                    temp = ProcessFieldForQuotes(sb.ToString(), dq);
                    sList.Add(temp);
                    sb.Length = 0;
                    continue;
                }
                sb.Append(tapehead);
            }
            temp = ProcessFieldForQuotes(sb.ToString(), dq);
            sList.Add(temp);
            return sList;
        }

        private static string ProcessFieldForQuotes(string s, string dq)
        {
            if (IsQuoted(s))
            {
                s = s.Substring(1, s.Length - 2);
                if (s.Contains(dq))
                {
                    s = s.Replace(dq, dq[1].ToString());
                }
            }
            else
            {
                s = s.Trim();
            }
            return s;
        }

        private static bool IsQuoted(string s)
        {
            return (s.Length > 1) && (s[0] == s[s.Length - 1]) && (s[0] == (char)34);
        }

        private string MoveTape(StreamReader file, char[] eol, StringBuilder sb)
        {
            sb.Append(eol[0]);
            eol[0] = eol[1];
            eol[1] = (char)file.Read();
            sb = new StringBuilder();
            sb.Append(eol);
            return sb.ToString();
        }


        private void ProcessFile()
        {
            Records = new List<List<string>>();
            if (FHasHeaders)
            {
                ReadHeaders();
            }
            while (!FFile.EndOfStream)
            {
                List<string> s = ReadLine(FFile);
                Records.Add(s);
            }
            FFile.Close();
            FFile.Dispose();
            position = -1;
            //StreamWriter temp = MyLib.GetTempFile();
            //temp.Flush();
            //temp.Close();
        }

        public TCSVReader(string filename, bool HasHeaders)
        {
            Stream st = File.OpenRead(filename);
            FFile = new StreamReader(st);
            FHasHeaders = HasHeaders;
            ProcessFile();
        }

        public int FieldIndex(string fieldName)
        {
            int index = -1;
            if (FHasHeaders)
            {
                index = Headers.IndexOf(fieldName);
            }
            else
            {
                // parses the excel column names
                string s = fieldName.ToUpper();
                Byte[] x = new Byte[2];
                if (s.Length == 1)
                {
                    x[0] = (Byte)s[0];
                    x[0] -= (Byte)'@';
                    index = (int)x[0] - 1;

                }
                else if (fieldName.Length == 2)
                {
                    x[0] = (Byte)s[0];
                    x[0] -= (Byte)'@';
                    x[1] = (Byte)s[1];
                    x[1] -= (Byte)'@';
                    index = ((int)x[0] * 26) + (int)x[1] - 1;
                }
            }
            return index;
        }

        public string this[string fieldName]
        {
            get { return this[fieldName, maxfieldLength].Trim(); }
        }

        public string this[int Index]
        {
            get { return this[Index, maxfieldLength].Trim(); }

        }

        public string this[int Index, int len] // returns a blank if field not found
        {
            get
            {
                string v = string.Empty;
                if ((Index < Record.Count) && (Index > -1))
                {
                    v = Record[Index].Trim();
                    if (FieldLimit)
                    {
                        v = (v.Length <= len) ? v : v.Substring(0, len);
                    }
                }
                return v;
            }
        }

        public string this[string fieldname, int l]
        {
            get
            {
                if (FHasHeaders)
                {
                    int i = Headers.IndexOf(fieldname);
                    return this[i, l];
                }
                else
                {
                    // parses the excel column names
                    string s = fieldname.ToUpper();
                    Byte[] x = new Byte[2];
                    if (s.Length == 1)
                    {
                        x[0] = (Byte)s[0];
                        x[0] -= (Byte)'@';
                        int i = (int)x[0] - 1;
                        return this[i, l];
                    }
                    else if (fieldname.Length == 2)
                    {
                        x[0] = (Byte)s[0];
                        x[0] -= (Byte)'@';
                        x[1] = (Byte)s[1];
                        x[1] -= (Byte)'@';
                        int i = ((int)x[0] * 26) + (int)x[1] - 1;
                        return this[i, l];
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        public object Current
        {
            get { return Record; }
        }

        public List<string> value
        {
            get { return Record; }
        }

        public int Count
        {
            get { return Records.Count; }
        }

        public int Progress
        {
            get { return (int)((double)position / Count * 100); }
        }

        public bool MoveNext()
        {
            position++;
            if (position < Records.Count)
            {
                Record = Records[position];
                return true;
            }
            else
            {

                return false;
            }
        }

        private void ReadHeaders()
        {
            if (!FFile.EndOfStream)
            {
                Headers = ReadLine(FFile);
                // A header line always has a column 0 string with length > 0
                // else it is a comment line
                bool found = (Headers.Count > 0) && (Headers[0].Length > 0);
                if (!found) ReadHeaders();
            }
        }

        public void Reset()
        {
            position = -1;
        }

        public void Rewind()
        {
            position = -1;
        }

        public void Dispose()
        {
            FFile.Dispose();
        }
    }

    public struct ColNames
    {
        public string Name;
        public string Field;
    }

    public class TlibStopWatches
    {
        public List<Stopwatch> Watches;

        public Stopwatch this[int index]
        {
            get
            {
                while (Watches.Count < (index + 1))
                {
                    Watches.Add(new Stopwatch());
                }
                return Watches[index];
            }
        }

        public TlibStopWatches()
        {
            Watches = new List<Stopwatch>();
        }
    }

    public class TLogWriter : IDisposable
    {
        private List<string> FLines = new List<string>();
        private Object csLock = new Object();
        private string FFileName;

        public string FileName
        {
            get { return FFileName; }
            set { FFileName = value; }
        }

        public TLogWriter()
        {
            FFileName = GetLogFileName();
        }

        public void Clear()
        {
            File.Delete(FileName);
        }

        public TLogWriter(string fn)
        {
            FFileName = fn;
        }

        ~TLogWriter()
        {
            Dispose();
        }

        public void WriteToLog(string s)
        {
            FLines.Add(s);
        }

        public void WriteToLogDate(string s)
        {
            DateTime dt = DateTime.Now;
            this.WriteToLog(s + " " + dt.ToShortDateString() + ":" + dt.ToShortTimeString());
        }

        public List<string> stack
        {
            get { return FLines; }
        }

        public void Reverse()
        {
            FLines.Reverse();
        }

        public void Publish()
        {
            lock (csLock)
            {
                if (FLines.Count > 0)
                {
                    if (File.Exists(FileName))
                    {
                        String[] reader = File.ReadAllLines(FileName);
                        int i = 0;
                        foreach (string s in reader)
                        {
                            FLines.Add(s);
                            i++;
                            if (i > 1000)
                            {
                                break;
                            }
                        }
                    }
                    StreamWriter writer = new StreamWriter(FileName, false);
                    for (int i = 0; i < FLines.Count; i++)
                    {
                        writer.WriteLine(FLines[i]);
                    }
                    writer.Dispose();
                    FLines.Clear();
                }
            }
        }

        public void Dispose()
        {
            if (FLines.Count > 0)
            {
                Publish();
            }
        }

        public static string GetLogFileName()
        {
            FileInfo fi = new FileInfo(Application.ExecutablePath);
            return GetLogFileName(fi.Name.Substring(0, fi.Name.Length - 3) + "log");
        }

        public static string GetLogFileName(string FileName)
        {
            return System.IO.Path.Combine(Application.StartupPath, FileName);
        }

    }

    public enum PhoneTypes
    {
        none,
        Home,
        Work,
        Cell
    };

    public class TPhoneNumber
    {
        public string AC;
        public string Prefix;
        public string Body;
        public PhoneTypes PhoneType;

        public virtual string TypeValue
        {
            get
            {
                string s = Value;
                string t = TPhoneNumber.PhoneTypeString(this.PhoneType);
                return string.Format("{0,-4} {1,14}", t, s);
            }
        }

        public virtual string Value
        {
            get
            {
                if (Prefix != string.Empty)
                {
                    if (AC == string.Empty)
                    {
                        return string.Format("{0}-{1}", Prefix, Body);
                    }
                    else
                    {
                        return string.Format("({0}) {1}-{2}", AC, Prefix, Body);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public TPhoneNumber(string a, string p, string b)
        {
            StringsToPhone(a, p, b);
        }

        private void StringsToPhone(string a, string p, string b)
        {
            AC = a.Length > 3 ? a.Substring(0, 3) : a;
            Prefix = p.Length > 3 ? p.Substring(0, 3) : p;
            Body = b.Length > 4 ? b.Substring(0, 4) : b;
        }

        public TPhoneNumber(int a, int p, int b)
        {
            IntegersToPhone(a, p, b);
        }

        private void IntegersToPhone(int a, int p, int b)
        {
            if ((a < 1000) && (p < 1000) && (b < 10000))
            {
                AC = a.ToString();
                Prefix = b.ToString();
                Body = b.ToString();
            }
            else
            {
                AC = string.Empty;
                Prefix = string.Empty;
                Body = string.Empty;
            }
        }

        /* public TPhoneNumber(PhoneTypes t, int a, int p, int b)
         {
             IntegersToPhone(a, p, b);
             this.PhoneType = t;
         }*/

        public TPhoneNumber(PhoneTypes t, string a, string p, string b)
        {
            StringsToPhone(a, p, b);
            this.PhoneType = t;
        }

        public TPhoneNumber(string pn)
        {
            NormalizePhone(pn);
        }

        public TPhoneNumber()
        {
            Initialize();
        }

        private void Initialize()
        {
            AC = string.Empty;
            Prefix = string.Empty;
            Body = string.Empty;
            PhoneType = PhoneTypes.none;
        }

        public bool NormalizePhone(string p)
        {
            Initialize();
            bool result = false;
            string s = MyLib.StrToNumeric(p);
            switch (s.Length)
            {
                case 7:
                    Prefix = s.Substring(0, 3);
                    Body = s.Substring(3, 4);
                    result = true;
                    break;
                case 10:
                    AC = s.Substring(0, 3);
                    Prefix = s.Substring(3, 3);
                    Body = s.Substring(6, 4);
                    result = true;
                    break;
                case 11:
                    if (s[0] == '1')
                    {
                        s = s.Substring(1, 10);
                        return NormalizePhone(s);

                    }
                    break;
            }
            return result;
        }

        public static string PhoneTypeString(PhoneTypes t)
        {
            string s = string.Empty;
            switch (t)
            {
                case (PhoneTypes.Home):
                    s = "Home";
                    break;
                case (PhoneTypes.Work):
                    s = "Work";
                    break;
                case (PhoneTypes.Cell):
                    s = "Cell";
                    break;
            }
            return s;
        }
    }

    public class TRID_NAME : IComparable
    {
        public int RID { get; set; }
        public string Name { get; set; }

        public TRID_NAME(int r, string name)
        {
            RID = r;
            Name = name;
        }

        public TRID_NAME()
        {
            RID = 0;
            Name = string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Value
        {
            get { return Name; }
        }

        public int CompareTo(object obj)
        {
            TRID_NAME temp = obj as TRID_NAME;
            return temp == null ? -1 : string.CompareOrdinal(this.Name, temp.Name);
        }

        public static List<int> Keys(List<TRID_NAME> list)
        {
            List<int> keys = new List<int>();
            foreach (var x in list)
            {
                keys.Add(x.RID);
            }
            return keys;
        }
    }

    public static class TCheckPrevious
    {
        private static IntPtr GetHWndOfPrevInstance(string ProcessName)
        {
            //get the current process
            Process CurrentProcess = Process.GetCurrentProcess();
            //get a collection of the currently active processes with he same name
            Process[] Ps = Process.GetProcessesByName(ProcessName);
            //if only one exists then there is no previous instance
            if (Ps.Length > 1)
            {
                foreach (Process P in Ps)
                {
                    if (P.Id != CurrentProcess.Id) //ignore this process
                    {
                        //weed out apps that have the same exe name but are started from a different filename.
                        if (P.ProcessName == ProcessName)
                        {
                            IntPtr hWnd = IntPtr.Zero;
                            try
                            {
                                //if process does not have a MainWindowHandle then an exception will be thrown
                                //so catch and ignore the error.
                                hWnd = P.MainWindowHandle;
                            }
                            catch
                            {
                            }
                            //return if hWnd found.
                            if (hWnd.ToInt32() != 0) return hWnd;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

        public static bool CheckPrevInstance()
        {
            string name = Process.GetCurrentProcess().ProcessName;
            IntPtr hWnd =
                GetHWndOfPrevInstance(Process.GetCurrentProcess().ProcessName);
            if (hWnd != IntPtr.Zero)
            {
                MyLib.csTell(name + " is already running.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class TWebFile
    {
        private WebClient wc = new WebClient();
        private byte[] data;

        public TWebFile(string link)
        {
            try
            {
                data = wc.DownloadData(link);
            }
            catch
            {
                data = null;
            }

        }

        public byte[] response()
        {
            return data;
        }
    }

    public class TWebRequest
    {
        private WebRequest request;

        public TWebRequest(string requestString)
        {

            // Create a request for the URL.   
            try
            {
                request = WebRequest.Create(requestString);
                request.Timeout = 20000;
            }
            catch (Exception ex)
            {
                ex.Message.csTell();
            }
        }

        public string Headers
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in request.Headers)
                {
                    sb.AppendLine((string)s);
                }

                return sb.ToString();
            }
        }

        public string Response
        {
            get { return response(); }
        }

        private string response()
        {
            // Get the response.
            string responseFromServer = "No Response";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = null;
                Stream dataStream = null;
                try
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        reader = new StreamReader(dataStream);
                        // Read the content.
                        responseFromServer = reader.ReadToEnd();
                    }
                    else
                    {
                        responseFromServer = "Error in response from server";
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                    if (dataStream != null) dataStream.Close();
                    if (response != null) response.Close();
                }
            }
            catch (Exception ex)
            {
                responseFromServer = ex.Message;
            }
            return responseFromServer;
        }
    }
}