Express Helper is a program I use when working with Develexpress XPO databasess.  

It creates database entries that use the following string extension

  public static string csLeft(this string s, int len)
        {
            if (String.IsNullOrEmpty(s)) return s;
            return new string(s.Take(len).ToArray());                                         
    	}
		
Enjoy
Marv