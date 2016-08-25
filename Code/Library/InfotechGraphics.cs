using System;
using System.Drawing;
using System.Collections.Generic;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Printing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;

//using DevExpress.XtraReports.UI;
//using DevExpress.XtraEditors;
//using DevExpress.Utils.Controls;

namespace ExpressHelper1011.Library
{
    public interface IChartData
    {
        int INDEX { get; set; }
        DateTime DATE { get; set; }
        string LABEL { get; set; }
        double LEFT { get; set; }
        double CENTER { get; set; }
        double RIGHT { get; set; }
        double X_DOUBLE { get; set; }
    }
    public class dtoChartdata
    {
        public int INDEX { get; set; }
        public DateTime DATE { get; set; }
        public string LABEL { get; set; }
        public string LEFT { get; set; }
        public string CENTER { get; set; }
        public string RIGHT { get; set; }
        public string X_DOUBLE { get; set; }
    }
    public static class TMyChartExtensions
    {
        public static Axis csYAxis(this ChartControl c)
        {
            return ((XYDiagram)c.Diagram).AxisY;
        }
        public static Axis csXAxis(this ChartControl c)
        {
            var d = c.Diagram as XYDiagram;
            if (d == null) return null;
            return d.AxisX;
        }
        public static void csZoom(this ChartControl c)
        {
            var d = c.Diagram as XYDiagram;
            if (d == null) return;
            d.EnableAxisXZooming = d.EnableAxisYZooming = true;
        }
        public static Axis csXAxis2D(this ChartControl c)
        {
            return csXAxis(c);
        }
        public static Axis3D csXAxis3D(this ChartControl c)
        {
            var d2 = c.Diagram as XYDiagram3D;
            if (d2 == null) return null;
                return d2.AxisX;
        }
        public static void csChartTitleAdd(this ChartControl c, string text, bool clear = true)
        {
            if (clear) c.Titles.Clear();
            ChartTitle t = new ChartTitle();
            t.Text = text;
            c.Titles.Add(t);
        }
        public static void csChartTitleAdd(this ChartControl c, string text, Font f, bool clear = true)
        {
            if (clear) c.Titles.Clear();
            ChartTitle t = new ChartTitle();
            t.Text = text;
            t.Font = f;
            c.Titles.Add(t);
        }
        public static void csChartTitleAdd(this ChartControl c, List<string> text, Font f1,Font f2)
        {
            c.Titles.Clear();
            int i = 0;
            foreach (var v in text)
            {
                c.csChartTitleAdd(v, i++ == 0 ? f1 : f2,false);
            }
        }
        public static void csChartTitleAdd(this ChartControl c, List<string> text)
        {
            c.Titles.Clear();
            foreach (var v in text)
            {
                c.csChartTitleAdd(v);
            }
        }
        public static Series csSeries(this ChartControl c, string name)
        {
            return c.Series[name];
        }
        public static Series csSeries(this ChartControl c, int n)
        {
            return c.Series.Count < n ? null : c.Series[n];
        }
       public enum LegendLocations 
        {LeftTop
            ,RightTop
                ,LeftBottom
                    ,RightBottom
                        ,LeftTopOut
                            ,RightTopOut
                                ,LeftBottomOut
                                    ,RightBottomOut
        }
         public static void SetLegendLocation(LegendLocations l, ChartControl c)
         {
             SetLegendLocation(((int) l).ToString(), c);
         }

        public static void SetLegendLocation(string location, ChartControl c)
        {
            switch (location)
            {
                case "0":
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
                    break;
                case "1":
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
                    break;
                case "2":
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.Bottom;
                    break;
                case "3":
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.Bottom;
                    break;
                case "4":
                    c.Legend.Visible = !c.Legend.Visible;
                    break;
                case "5":  // top left
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.LeftOutside;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
                    break;
                case "6": // top right 
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.RightOutside;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
                    break;
                case "7":  //bottom left
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.LeftOutside;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
                    break;
                case "8":  // bottom right
                    c.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.RightOutside;
                    c.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
                    break;

            }
        }
  
        public static Image ChartToImage(ChartControl chart)
        {
            Image image = null;

            // Create an image of the chart.
            using (MemoryStream s = new MemoryStream())
            {
                chart.ExportToImage(s, ImageFormat.Png);
                image = Image.FromStream(s);
            }
            return image;
        }
        public static void PrintChartInForm(ChartControl c, Form f)
        {
            Size s = f.Size;
            if ((s.Width < 500) || (s.Height < 500))
            {
                f.Width = 800;
                f.Height = 800;
                f.Refresh();
            }
            c.OptionsPrint.SizeMode = PrintSizeMode.Zoom;
            c.ShowPrintPreview();
            f.Size = s;
        }
        public static Axis GetSecondaryY(ChartControl chart, int n)
        {
            return ((XYDiagram)chart.Diagram).SecondaryAxesY[n];
        }
        // 130429
        public static void csSetChartTitleString(this ChartControl chart, string top, string bottom, int TopSize = 14, int bottomSize = 8)
        {
            string t = string.Format("<size={2}>{0}<br><size={3}>{1}"
                                     , top
                                     , bottom
                                     , TopSize
                                     , bottomSize);
            chart.csChartTitleAdd(t, true);
        }
}  
      
}
