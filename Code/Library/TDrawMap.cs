using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;

namespace ExpressHelper1011.Library
{

    public class TGeoPoint
    {
        public decimal LATITUDE;
        public decimal LONGITUDE;
        public TGeoPoint()
        {
        }
        public override string ToString()
        {
            return string.Format("{0:##0.0000},{1:##0.0000}", LATITUDE, LONGITUDE);
        }
        public bool IsValid
        {
            get { return !((Math.Abs(LATITUDE) < (decimal)0.0001) || (Math.Abs(LONGITUDE) < (decimal)0.0001)); }
        }
    }
    public struct sGeoPoint
    {
        public string Name;
        public double Lat;
        public double Lng;
    }

    public enum eMapColors
    {
        blue, red, green, yellow, white
    }
    public class TGeoMarker
    {
        public TGeoPoint POINT { get; set; }
        public char MARKER { get; set; }
        public eMapColors COLOR { get; set; }
    }

    public class TGeoAddress
    {
        public string NAME { get; set; }
        public char MARKER { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public TGeoPoint POINT { get; set; }
        public string MARKER_COLOR;
        public TGeoAddress()
        {
            NAME = string.Empty;
            MARKER = 'X';
            MARKER_COLOR = "blue";
        }

    }
    public class TGeoPath
    {
        public string PATH_COLOR;
        public int WEIGHT { get; set; }
        public TGeoPoint[] POINTS { get; set; }
        public TGeoPath()
        {
            POINTS = new TGeoPoint[2];
            PATH_COLOR = "red";
            WEIGHT = 5;
        }
    }
    public class TDrawMap
    {
        //    string path = string.Format("path=color:red|{0},{1}|{2},{3}", p1.Lat, p1.Lng, p2.Lat, p2.Lng);
        //string link =
        //              string.Format(@"http://maps.google.com/maps/api/staticmap?zoom={5}&size={2}x{3}&maptype={4}&markers=color:{6}|label:U|{0},{1}&{7}&sensor=false"
        //                            , p1.Lat // tbAddressNo.Text //0
        //                            , p1.Lng //cbStreets.Text //1
        //                            , 640 //2
        //                            , 640 //3
        //                            , "roadmap"  //4
        //                            , zoom // zoom //5
        //                            , color //6 "blue"
        //                            , path

        //                  );
        private const string WebString =
              @"<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"""">
<html>
<head>
  <meta content="""" text/html="""" charset=""""ISO-8859-1&quot;&quot;""""
 http-equiv="""" content-type="""">
  <title>::TITLE::</title>
</head>
<body>
<div style=""left: 2px; height: 640px Width= 640px"">
<img src=::FILENAME:: <br>
</div>
<div>
Description:
<br>
::DESCRIPTION::
</div>
</body>
</html> ";
        private const string mapUrlFormatString = "https://maps.google.com/maps/api/staticmap?zoom={2}&sensor=false&size={0}x{1}&maptype=roadmap";
        private const string mapUrlFormatStringCenter = "https://maps.google.com/maps/api/staticmap?center={3}&zoom={2}&sensor=false&size={0}x{1}&maptype=roadmap";
        public Size MAPSIZE { get; set; }
        public int ZOOM { get; set; }
        public List<TGeoPath> PATHS { get; set; }
        public List<TGeoAddress> ADDRESSES { get; set; }
        public List<TGeoMarker> MARKERS { get; set; }
        public StringBuilder DESCRIPTION { get; set; }
        public TDrawMap(int ZoomLevel)
        {
            ADDRESSES = new List<TGeoAddress>();
            PATHS = new List<TGeoPath>();
            MARKERS = new List<TGeoMarker>();
            MAPSIZE = new Size(640, 640);
            ZOOM = ZoomLevel;
            DESCRIPTION = new StringBuilder();
        }

        public void AddPath(TGeoPath p)
        {
            PATHS.Add(p);
        }
        public void AddAddresses(TGeoAddress a)
        {
            ADDRESSES.Add(a);
        }
        public void AddMarker(TGeoMarker m)
        {
            MARKERS.Add(m);
        }

        private const int linkLimit = 2048;
        public void Plot()
        {

            string link = string.Format(mapUrlFormatString, MAPSIZE.Height, MAPSIZE.Width, ZOOM);
            foreach (var m in MARKERS)
            {
                var s =
                    string.Format("&markers=color:{0}%7Clabel:{1}%7C{2}", m.COLOR.ToString(), m.MARKER, m.POINT).csPlusSub
                        ();
                if ((link.Length + s.Length) < linkLimit) link = link + s;
            }
            foreach (var p in PATHS)
            {
                string s = string.Empty;
                // Now Create Path
                string spath = string.Format("&path=color:{0}%7Cweight:{1}", p.PATH_COLOR, p.WEIGHT);
                foreach (var point in p.POINTS)
                {
                    if (point.LATITUDE < 20 || point.LONGITUDE > -30)
                    {
                        s = string.Empty;
                        spath = string.Empty;
                        break;
                    }
                    spath += string.Format("%7C{0}", point);
                }
                //        logwriter.WriteToLogDate(spath);
                s += spath;
                if (link.Length + s.Length > linkLimit)
                {
                    //             logwriter.WriteToLogDate("////////////// Exceeded the line length /////////////////////////////////");
                    break;
                }
                else
                {
                    link += s;

                }
            }
            //  logwriter.WriteToLogDate(link);
            //   link.Length.ToString().csTell();
            string webstring = WebString.Replace("::FILENAME::", link);
            webstring = webstring.Replace("::DESCRIPTION::", DESCRIPTION.ToString());
            webstring = webstring.Replace("::TITLE::", "Map");
            //    logwriter.WriteToLogDate(webstring);
            //     logwriter.Publish();
            //     logwriter.Dispose();
            //  link.Length.csTell();
            const string fn = "287B5866-0D9D-4D59-A360-0D3113ABBC20.html";
            File.Delete(fn);
            StreamWriter writer = new StreamWriter(fn);
            writer.Write(webstring);
            writer.Flush();
            writer.Close();
            MyLib.RunExternal(fn);
            //   webstring.csTell();
        }

    }
}
