using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace ExpressHelper1011.Library
{
   
    public static partial class MyLib
    {
        public enum eDBType
        {
            none = 0,
            Firebird = 1,
            SQLServer = 2,
            SQLite = 3,
            SQLServerTrusted = 4
        }
           public static class ConnectionHelper
        {
            private static eDBType _dbType = eDBType.SQLite;
            private static string _database = "CollectionManagementSystem";
            private static bool _updateDatabase = false;
            private static string DataSource { get; set; }
            public static string Server { get { return DataSource; } set { DataSource = value; } }
            public static string UserID { get; set; }
            public static string Password { get; set; }
            public static bool UpdateDatabase
            {
                get { return _updateDatabase; }
                set { _updateDatabase = value; }
            }

            public static string Database
            {
                get { return _database; }
                set { _database = value; }
            }

            public static eDBType DBType
            {
                get { return _dbType; }
                set { _dbType = value; }
            }
            public static bool TestDatabaseParameters()
            {
                Session session_sql = new Session
                {
                    ConnectionString = MyLib.ConnectionHelper.ConnectionString,
                    AutoCreateOption = UpdateDatabase ? AutoCreateOption.DatabaseAndSchema : AutoCreateOption.SchemaAlreadyExists
                };
                try
                {
                    session_sql.Connect();
                }
                catch (Exception e)
                {
                  //  e.csTell();
                    return false;
                }
                return session_sql.DataLayer != null;
            }
            public static string FirebirdConnectionString
            {
                get
                {
                    return string.Format(
                        @"XpoProvider=Firebird;DataSource={0};User={1};Password={2};Database={3};ServerType=0;Charset=NONE"
                        , DataSource, UserID, Password, Database);
                }
            }
            public static string ConnectionString
            {
                get
                {
                    switch (DBType)
                    {
                        case eDBType.Firebird:
                            return FirebirdConnectionString;
                        case eDBType.SQLServer:
                            return SQLServerConnectionString;
                        case eDBType.SQLite:
                            return SQLiteConnectionString;
                        case eDBType.SQLServerTrusted:
                            return SQLServerTrustedConnectionString;
                        default:
                            throw new DataException("Connection String Is Not Defined.");
                            break;
                    }
                }
            }

            public static string SQLServerExpressConnectionString
            {
                get
                {
                    return string.Format(
                        @"XpoProvider=MSSqlServer;data source={0};initial catalog={1};Trusted_Connection=True",
                        DataSource, Database);
                }
            }
            public static string SQLServerTrustedConnectionString
            {
                get
                {
                    return string.Format(
                        @"XpoProvider=MSSqlServer;data source={0};initial catalog={1};Trusted_Connection=True",
                        DataSource, Database);
                }
            }
            public static string SQLServerConnectionString
            {
                get
                {
                    return string.Format(
                        @"XpoProvider=MSSqlServer;data source={0};User ID={1};Password={2};initial catalog={3};Persist Security Info=true",
                        DataSource, UserID, Password, Database);
                }
            }

            public static string SQLiteConnectionString
            {
                get
                {
                    return string.Format(
                        @"XpoProvider=SQLite;Data Source={0}.DB", Database);
                }
            }
            public static string publicConnectionString()
            {
                switch (DBType)
                {
                    case eDBType.SQLServer:
                    case eDBType.Firebird:
                        return string.Format(
@"Server: {0};
Database: {1};
UserID: {2}",
              DataSource, Database, UserID);
                    case eDBType.SQLite:
                        return string.Format(
@"Database: {0};",
              Database);
                    case eDBType.SQLServerTrusted:
                        return string.Format(
@"Server: {0};
Database: {1}",
              DataSource, Database);

                    default:
                        throw new DataException("Connection String Is Not Defined.");
                        break;
                }
            }
            public static UnitOfWork Connect()
            {
                    AutoCreateOption opt = UpdateDatabase
                                               ? AutoCreateOption.DatabaseAndSchema
                                               : AutoCreateOption.SchemaAlreadyExists;
                    XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, opt);
                    XpoDefault.Session = null;
                    if (XpoDefault.DataLayer == null) return null;
                    return new UnitOfWork();
            }

            public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(
                DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
            {
                return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
            }

            public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(
                DevExpress.Xpo.DB.AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
            {
                return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption,
                                                        out objectsToDisposeOnDisconnect);
            }

            public static IDataLayer GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
            {
                return XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
            }
        }


        public static void csAddList<T>(this XPCollection collection, List<T> list)
        {
            collection.LoadingEnabled = false;
            while (collection.Count > 0)
            {
                collection.Remove(collection[0]);
            }
            collection.AddRange(list);
        }

        public static void csClearCollectionSetUOW(this XPCollection collection, UnitOfWork uow,
                                                   bool LoadingEnabled = false)
        {
            while (collection.Count > 0)
            {
                collection.Remove(collection[0]);
            }
            collection.LoadingEnabled = LoadingEnabled;
            collection.Session = uow;
            //  collection.Count.csTell();
        }

    }


        //Firebird 
//        public static class ConnectionHelper
//        {
//            private static eDBType _dbType = eDBType.SQLite;
//            private static string _database = "CollectionManagementSystem";
//            public static string DataSource { get; set; }
//            public static string UserID { get; set; }
//            public static string Password { get; set; }
//            public static string Database
//            {
//                get { return _database; }
//                set { _database = value; }
//            }

//            public static eDBType DBType
//            {
//                get { return _dbType; }
//                set { _dbType = value; }
//            }

//            public static string FirebirdConnectionString
//            {
//                get
//                {
//                    return string.Format(
//                        @"XpoProvider=Firebird;DataSource={0};User={1};Password={2};Database={3};ServerType=0;Charset=NONE"
//                        , DataSource, UserID, Password, Database);
//                }
//            }
//            public static string ConnectionString
//            {
//                get
//                {
//                    switch (DBType)
//                    {
//                        case eDBType.Firebird:
//                            return FirebirdConnectionString;
//                        case eDBType.SQLServer:
//                            return SQLServerConnectionString;
//                        case eDBType.SQLite:
//                            return SQLiteConnectionString;
//                        case eDBType.SQLServerTrusted:
//                            return SQLServerTrustedConnectionString;
//                        default:
//                            throw new DataException("Connection String Is Not Defined.");
//                            break;
//                    }
//                }
//            }

//            public static string SQLServerExpressConnectionString
//            {
//                get
//                {
//                    return string.Format(
//                        @"XpoProvider=MSSqlServer;data source={0};initial catalog={1};Trusted_Connection=True",
//                        DataSource, Database);
//                }
//            }
//            public static string SQLServerTrustedConnectionString
//            {
//                get
//                {
//                    return string.Format(
//                        @"XpoProvider=MSSqlServer;data source={0};initial catalog={1};Trusted_Connection=True",
//                        DataSource, Database);
//                }
//            }
//            public static string SQLServerConnectionString
//            {
//                get
//                {
//                    return string.Format(
//                        @"XpoProvider=MSSqlServer;data source={0};User ID={1};Password={2};initial catalog={3};Persist Security Info=true",
//                        DataSource, UserID, Password, Database);
//                }
//            }

//            public static string SQLiteConnectionString
//            {
//                get
//                {
//                    return string.Format(
//                        @"XpoProvider=SQLite;Data Source={0}.DB", Database);
//                }
//            }
//            public static string publicConnectionString()
//            {
//                switch (DBType)
//                {
//                    case eDBType.SQLServer:
//                    case eDBType.Firebird:
//                        return string.Format(
//@"Server: {0};
//Database: {1};
//UserID: {2}", 
//              DataSource, Database, UserID);
//                    case eDBType.SQLite:
//                        return string.Format(
//@"Database: {0};", 
//              Database);
//                    case eDBType.SQLServerTrusted:
//                        return string.Format(
//@"Server: {0};
//Database: {1}", 
//              DataSource, Database);

//                    default:
//                        throw new DataException("Connection String Is Not Defined.");
//                        break;
//                }
//            }
//            public static UnitOfWork Connect(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
//            {
//                try
//                {
//                    XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
//                    XpoDefault.Session = null;
//                    if (XpoDefault.DataLayer == null) return null;
//                    return new UnitOfWork();
//                }
//                catch (Exception ex)
//                {
//                    ex.ToString().csTell();
//                    throw;
//                }

//            }

//            public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(
//                DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
//            {
//                return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
//            }

//            public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(
//                DevExpress.Xpo.DB.AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
//            {
//                return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption,
//                                                        out objectsToDisposeOnDisconnect);
//            }

//            public static IDataLayer GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
//            {
//                return XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
//            }
//        }


    //    public static void csAddList<T>(this XPCollection collection, List<T> list)
    //    {
    //        collection.LoadingEnabled = false;
    //        while (collection.Count > 0)
    //        {
    //            collection.Remove(collection[0]);
    //        }
    //        collection.AddRange(list);
    //    }

    //    public static void csClearCollectionSetUOW(this XPCollection collection, UnitOfWork uow,
    //                                               bool LoadingEnabled = false)
    //    {
    //        while (collection.Count > 0)
    //        {
    //            collection.Remove(collection[0]);
    //        }
    //        collection.LoadingEnabled = LoadingEnabled;
    //        collection.Session = uow;
    //        //  collection.Count.csTell();
    //    }

    //}

    //public static class FirebirdConnectionHelper
    //{
    //    public static string DataSource { get; set; }
    //    public static string UserID { get; set; }
    //    public static string Password { get; set; }
    //    public static string Database { get; set; }
    //    public static string ConnectionString
    //    {
    //        get
    //        {
    //            return string.Format(@"XpoProvider=Firebird;DataSource={0};User={1};Password={2};Database={3};ServerType=0;Charset=NONE"
    //                    , DataSource, UserID, Password, Database);
    //        }
    //    }
    //    public static void Connect(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
    //    {
    //        XpoDefault.DataLayer = XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
    //        XpoDefault.Session = null;
    //    }
    //    public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
    //    {
    //        return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption);
    //    }
    //    public static DevExpress.Xpo.DB.IDataStore GetConnectionProvider(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
    //    {
    //        return XpoDefault.GetConnectionProvider(ConnectionString, autoCreateOption, out objectsToDisposeOnDisconnect);
    //    }
    //    public static IDataLayer GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
    //    {
    //        return XpoDefault.GetDataLayer(ConnectionString, autoCreateOption);
    //    }
    //}
    //public interface IChartData
    //{
    //    int INDEX { get; set; }
    //    DateTime DATE { get; set; }
    //    string LABEL { get; set; }
    //    double LEFT { get; set; }
    //    double CENTER { get; set; }
    //    double RIGHT { get; set; }
    //    double X_DOUBLE { get; set; }
    //}
    //public class TChartDataXPO : XPObject, IChartData
    //{
    //    private DateTime _date;
    //    public DateTime DATE
    //    {
    //        get { return _date; }
    //        set { SetPropertyValue("DATE", ref _date, value); }
    //    }
    //    private string _label;
    //    public string LABEL
    //    {
    //        get { return _label; }
    //        set { SetPropertyValue("LABEL", ref _label, value); }
    //    }
    //    private int _index;
    //    public int INDEX
    //    {
    //        get { return _index; }
    //        set { SetPropertyValue("INDEX", ref _index, value); }
    //    }
    //    private double _x_double;
    //    public double X_DOUBLE
    //    {
    //        get { return _x_double; }
    //        set { SetPropertyValue("X_DOUBLE", ref _x_double, value); }
    //    }
    //    private double _left;
    //    public double LEFT
    //    {
    //        get { return _left; }
    //        set { SetPropertyValue("LEFT", ref _left, value); }
    //    }
    //    private double _center;
    //    public double CENTER
    //    {
    //        get { return _center; }
    //        set { SetPropertyValue("CENTER", ref _center, value); }
    //    }
    //    private double _right;
    //    public double RIGHT
    //    {
    //        get { return _right; }
    //        set { SetPropertyValue("RIGHT", ref _right, value); }
    //    }
    //    public TChartDataXPO(Session session)
    //        : base(session)
    //    {
    //        // This constructor is used when an object is loaded from a persistent storage.
    //        // Do not place any code here.
    //    }
    //    public override void AfterConstruction()
    //    {
    //        base.AfterConstruction();
    //        // Place here your initialization code.
    //    }

    //}
}