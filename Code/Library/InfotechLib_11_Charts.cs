using System;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using DevExpress.XtraCharts;

namespace ExpressHelper1011.Library
{
    public static partial class MyLib
    {
        public static string csSaveChartImageToFile(this ChartControl c, string path, string chartName = null)
        {
            if (string.IsNullOrEmpty(chartName))
            {
                ChartElement[] t = c.Titles.ToArray();
                if (t.Length > 0)
                {
                    foreach (ChartElement chartElement in t)
                    {
                        chartName += (chartElement + " ");
                    }
                }
            }
            chartName = chartName.Replace("\r\n", " ").Replace("/","-").Replace(":","_").Trim();
            // strip out the html
            int loc = chartName.IndexOf('<');
            while(loc > -1)
            {
                // find closing angle bracket
                int floc = chartName.IndexOf('>');
                if(floc > loc)
                {
                    chartName = chartName.Substring(0, loc)+" "+ chartName.Substring(floc + 1);
                    loc = chartName.IndexOf('<');
                }
                else
                {
                    break;
                }
            }
            chartName = chartName.Replace('<', '-').Replace('>', '-');
            var temp = MyLib.GetSaveFileName(path
                                             , "png"
                                             , "png"
                                             , "Save Chart Image"
                                             , chartName);
            if (string.IsNullOrEmpty(temp)) return path;
            c.ExportToImage(temp, ImageFormat.Png);
            return Path.GetDirectoryName(temp);
        }

        public static Image csGetChartImage(this ChartControl chart, ImageFormat format)
        {
            // Create an image.
            Image image = null;

            // Create an image of the chart.
            using (MemoryStream s = new MemoryStream())
            {
                chart.ExportToImage(s, format);
                image = Image.FromStream(s);
            }

            // Return the image.
            return image;
        }
        public static Bitmap CaptureScreen(System.Windows.Forms.Form form, int topMargin = 60, int leftMargin= 8, int rightMargin= 8, int bottomMarbin= 8)
        {
            Graphics myGraphics = form.CreateGraphics();
            Size s = form.Size;
            Bitmap memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(form.Location.X, form.Location.Y, 0, 0, s);

            // Clone a portion of the Bitmap object.
            Rectangle cloneRect = new Rectangle(leftMargin, topMargin, s.Width - leftMargin - rightMargin, s.Height - topMargin - bottomMarbin);
            return memoryImage.Clone(cloneRect, memoryImage.PixelFormat);
        }
  
    
    }
    public class TChartData : IChartData
    {
        public int INDEX { get; set; }
        public DateTime DATE { get; set; }
        public string LABEL { get; set; }
        public double LEFT { get; set; }
        public double CENTER { get; set; }
        public double RIGHT { get; set; }
        public double X_DOUBLE { get; set; }
    }

}
