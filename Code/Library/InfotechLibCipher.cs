using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

//using System.Drawing;

namespace ExpressHelper1011.Library
{
    /*
    void CopyData(IXPSimpleObject source, IXPSimpleObject dest) 
    {
      foreach (XPMemberInfo property in source.ClassInfo.PersistentProperties)
      if (!property.IsKey && !(property is ServiceField))
            property.SetValue(dest, property.GetValue(source));
    }
    */
    public static class TCipher
    {
        public static DateTime mjcDate = MyLib.MJCDate();
        public static string DateToHex(DateTime d)
        {
            TimeSpan ts = d - mjcDate;
            int days = ts.Days;
            return MyLib.IntToHex(days);
        }
        public static DateTime HexToDate(string UserId)
        {
            int days = MyLib.HexToInt(UserId);
            return mjcDate.AddDays(days);
        }
        public static string DotKey(string UserID, string UserName, string Password)
        {
            string s = UserID + Password + UserName + Password;
            while (s.Contains(" "))
            {
                int i = s.IndexOf(' ');
                s = s.Remove(i, 1);
            }
            s = s.ToUpper();
            return IntToDotted(TCipher.csHashint32(s));
        }
        public static DateTime ExpireDate(int CSKey)
        {
            return mjcDate.AddDays(CSKey);
        }
        public static void UserID_DotKey(DateTime ExpireDate, string UserName, string Password, out string UserID, out string Key)
        {
            TimeSpan ts = ExpireDate - mjcDate;
            UserID = ts.Days.ToString("X");
            Key = DotKey(UserID, UserName, Password);
        }

        public static int GenerateTumbler(string userid, string username, string key)
        {
            string s = userid + key + username + key;
            return ProcessInput(s);

        }
        public static int GenerateTumbler(int userid, string username, string key)
        {
            string hUserID = MyLib.IntToHex(userid).ToUpper();
            string s = hUserID + key + username + key;
            return ProcessInput(s);
        }
        public static string GetWindowsValidationCode(string uid, string key)
        {
            string domain = Environment.UserDomainName;
            StringBuilder sb = new StringBuilder(key);
            sb.Append(uid);
            sb.Append(key);
            sb.Append(domain);
            sb.Append(key);
            sb.Append(domain);
            sb.Append(uid);
            sb.Append(key);
            string s = TCipher.SliceAndDice(sb.ToString());
            return TCipher.PrintableString(s);
        }
        public static int GetPassword(string p, string key)
        {
            return TCipher.csHashint32(TCipher.SliceAndDice(p + key));
        }

        //public static bool Validate_strSystemRow(strSystemRow row,string password)
        //{
        //    int t = GenerateTumbler(row.USERID, row.NAME, password);
        //    return t == row.TUMBLER;            
        //}     

        public static string BytesToBase64String(byte[] b)
        {
            return Convert.ToBase64String(b);
        }
        public static string BytesToRegularString(byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }
        public static string Base64toString(string Base64)
        {
            byte[] bytes = Convert.FromBase64String(Base64);
            return Encoding.UTF8.GetString(bytes);
        }
        public static byte[] Base64toBytes(string Base64)
        {
            return Convert.FromBase64String(Base64);
        }
        public static byte[] RegularStringToBytes(string RegularString)
        {
            return Encoding.Default.GetBytes(RegularString);
        }
        public static string csStingtoBase64(this string s)
        {
            return StringToBase64(s);
        }
        public static string StringToBase64(string s)
        {
            byte[] b = RegularStringToBytes(s);
            return Convert.ToBase64String(b);
        }
        public static byte[] csToBytes(this string p)
        {
            return StringToBytes(p);
        }
        public static byte[] StringToBytes(string p)
        {
            return RegularStringToBytes(p);
        }

        public static string IntToDotted(int n)
        {
            int i = (n >> 24) & 0xFF;
            string s = i.ToString();
            i = (n >> 16) & 0xFF;
            s += "." + i.ToString();
            i = (n >> 8) & 0xFF;
            s += "." + i.ToString();
            i = n & 0xFF;
            s += "." + i.ToString();
            return s;
        }
        public static int DottedToInt(string s)
        {
            int Value = 0;
            string[] sp = s.Split('.');
            for (int i = 0; i < sp.Length && i < 4; i++)
            {
                s = sp[i];
                if (s.Length < 4)
                {
                    int k = int.Parse(s);
                    if (k < 256)
                    {
                        Value = (Value << 8) + k;
                    }
                }
            }
            return Value;
        }
        public static List<KeySizes> GetPossibleKeySizes()
        {
            List<KeySizes> list = new List<KeySizes>();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            foreach (KeySizes i in rsa.LegalKeySizes)
            {
                list.Add(i);
            }
            return list;
        }
        public static bool CreateKeyPair(string FileName, int KeySize)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize);
            string xml = rsa.ToXmlString(true);
            File.Delete(FileName);
            StreamWriter sw = new StreamWriter(FileName);
            sw.Write(xml);
            sw.Flush();
            sw.Close();
            sw.Dispose();
            return true;
        }
        public static bool ExportpublicKey(string KeyPairFile, string publicKeyFile) // both file names
        {
            if (File.Exists(KeyPairFile))
            {
                StreamReader br = new StreamReader(KeyPairFile);
                string xml = br.ReadToEnd();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xml);
                string publickey = rsa.ToXmlString(false);
                StreamWriter sw = new StreamWriter(publicKeyFile);
                sw.Write(publickey);
                sw.Flush();
                sw.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static byte[] encryptStringToBytes_AES(string plainText, byte[] Key)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
#if(DEBUG)
                {
                    throw new ArgumentNullException("plainText");
                }
#else
                {
                    return null;
                }
#endif
            }
            if (Key == null || Key.Length <= 0)
            {
#if(DEBUG)
                {
                    throw new ArgumentNullException("Key");
                }
#else
                {
                    return null;
                }
#endif
            }
            // Declare the streams used
            // to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;
            int BlockSize = 0;
            byte[] IV;


            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the bytes used to hold the
            // encrypted data.

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                BlockSize = aesAlg.BlockSize / 8;
                IV = RandomBytes(BlockSize);
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                BinaryWriter bwEncrypt = new BinaryWriter(msEncrypt);
                bwEncrypt.Write(IV, 0, BlockSize);
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);

                //Write all data to the stream.
                swEncrypt.Write(plainText);

            }
            finally
            {
                // Clean things up.

                // Close the streams.
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            /*/ Return the encrypted bytes from the memory stream.
            byte[] cipherMessage = msEncrypt.ToArray();
            byte[] ciphertext = new byte[cipherMessage.Length + BlockSize];
            for (int i = 0; i < BlockSize; i++)
            {
                ciphertext[i] = IV[i];
            }
            for (int i = BlockSize; i < ciphertext.Length; i++)
            {
                ciphertext[i] = cipherMessage[i - BlockSize];
            } */
            return msEncrypt.ToArray();
        }
        public static string decryptStringFromBytes_AES(byte[] cipherText, byte[] Key)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            // TDeclare the streams used
            // to decrypt to an in memory
            // array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                try
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = Key;
                    byte[] IV = new byte[aesAlg.BlockSize / 8];
                    for (int i = 0; i < IV.Length; i++)
                    {
                        IV[i] = cipherText[i];
                    }
                    //                    MyLib.csTell(Convert.ToBase64String(IV));
                    aesAlg.IV = IV;
                    byte[] cipherMessage = new byte[cipherText.Length - IV.Length];
                    for (int i = IV.Length; i < cipherText.Length; i++)
                    {
                        cipherMessage[i - IV.Length] = cipherText[i];
                    }
                    //    MyLib.csTell(Convert.ToBase64String(cipherMessage));
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    msDecrypt = new MemoryStream(cipherMessage);
                    csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    srDecrypt = new StreamReader(csDecrypt);

                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd();
                }
                // catch
                // {
                //plaintext = string.Empty;
                //                }
                finally
                {
                    // Clean things up.

                    // Close the streams.
                    if (srDecrypt != null)
                        srDecrypt.Close();
                    if (csDecrypt != null)
                        csDecrypt.Close();
                    if (msDecrypt != null)
                        msDecrypt.Close();

                    // Clear the RijndaelManaged object.
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
            }
            catch { }
            return plaintext;
        }
        public static string PrintableString(string Message)
        {
            return TCipher.StringToBase64(Message);
        }
        public static string csHashStringHEX(string Message)
        {
            byte[] h = csHashBytes(Message);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in h)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
        public static string csHash(this string MessageString) // Secure hash
        {
            return HashString(MessageString);
        }
        public static string csHashString(string p)
        {
            return p.csHash();
        }
        public static string HashStringCompressed(string Message)
        {
            byte[] h = csHashBytes(Message);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in h)
            {
                sb.Append((char)b);
            }
            return StringToBase64(sb.ToString());
        }
        public static string HashString(string Message)
        {
            byte[] h = csHashBytes(Message);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in h)
            {
                sb.Append(b.ToString("X2"));
            }
            return StringToBase64(sb.ToString());
        }
        public static byte[] csHashBytes(string Message)
        {
            byte[] MessageBytes = TCipher.RegularStringToBytes(Message);
            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(MessageBytes);
        }
        public static int csHashint32(this string MessageString) // Secure hash
        {
            if (MessageString == null) return 0;
            //convert the string into an array of Unicode bytes.
            byte[] MessageBytes = TCipher.RegularStringToBytes(MessageString);

            //Create a new instance of the SHA1Managed class to create 
            //the hash value.
            SHA1Managed SHhash = new SHA1Managed();

            //Create the hash value from the array of bytes.
            byte[] HashValue = SHhash.ComputeHash(MessageBytes);
            int v = 0;
            for (int i = 6; i < 10; i++)
            {
                byte b = HashValue[i];
                v = (v << 8) + HashValue[i];
            }
            return v;
        }
        public static byte[] RandomBytes(int len)
        {
            byte[] randomNumber = new byte[len];
            // Create a new instance of the RNGCryptoServiceProvider. 
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);
            return randomNumber;
        }
        private static byte[] CreateKey(string password, int len)
        {
            string s1 = password;
            byte[] csKey = new byte[len];
            int n = 0;
            while (n < len)
            {
                s1 = PrintableString(s1);
                byte[] newkey = csHashBytes(s1); //SHA1
                int j = 0;
                for (int i = 0; ((n + i) < len) && (i < newkey.Length); i++)
                {
                    csKey[n + i] = newkey[i];
                    j++;
                }
                n += j;
            }
            return csKey;
        }
        private static byte[] Parameter(string Password)
        {
            if (Password[0] == '{')
            {
                Password = Password.Substring(1, Password.Length - 2);
            }
            return CreateKey(Password, 32);
        }
        private static byte[] CreateRijndaelKey(string Password)
        {
            if (Password[0] == '{')
            {
                Password = Password.Substring(1, Password.Length - 2);
            }
            return CreateKey(Password, 32);
        }
        private static byte[] CreateRijndaelIV(string Password)
        {
            if (Password[0] == '{')
            {
                Password = Password.Substring(1, Password.Length - 2);
            }
            return CreateKey(Password, 16);
        }
        public static string EncryptStringToBase64(string Value, string key)
        {
            if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(key)) return string.Empty;
            byte[] keyb = Parameter(key);
            byte[] cypher = TCipher.encryptStringToBytes_AES(Value, keyb);
            return BytesToBase64String(cypher);
        }
        public static string csEncryptSD(this string s, string key)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(key)) return string.Empty;
            return EncryptStringSandD(s, key);
        }
        public static string EncryptStringSandD(string Value, string Key)
        {
            if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(Key)) return string.Empty;
            string newKey = SliceAndDice(Key);
            return EncryptString(Value, newKey);
        }
        public static string csEncrypt(this string s, string key)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(key)) return string.Empty;
            return EncryptString(s, key);
        }
        public static string EncryptString(string Value, string Key)
        {
            if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(Key)) return string.Empty;
            if ((Value == string.Empty) || (Key == string.Empty))
            {
                return EncryptStringToBase64(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
            return EncryptStringToBase64(Value, Key);
        }
        public static string csDecryptSD(this string s, string key)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(key)) return string.Empty;
            return DecryptStringSandD(s, key);
        }
        public static string DecryptStringSandD(string Value64, string key)
        {
            if (string.IsNullOrEmpty(Value64) || string.IsNullOrEmpty(key)) return string.Empty;
            key = SliceAndDice(key);
            return DecryptString(Value64, key);
        }
        public static string csDecrypt(this string s, string key)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(key)) return string.Empty;
            return DecryptString(s, key);
        }
        public static string DecryptString(string Value64, string Password)
        {
            if (string.IsNullOrEmpty(Value64) || string.IsNullOrEmpty(Password)) return string.Empty;
                try
                {
                    byte[] key = Parameter(Password);
                    byte[] cypher = TCipher.Base64toBytes(Value64);
                    return decryptStringFromBytes_AES(cypher, key);
                }
                catch
                {
#if(DEBUG)
                    {
                        throw new Exception("Invalid Operation");
                    }
#else
                    { return string.Empty; }
#endif
                }
        }
        static Random r = null;
        public static int csRandom(int maxvalue)
        {
            if (r == null)
            {
                r = new Random();
            }
            return r.Next(maxvalue);
        }
        public static byte[] EncryptByteTobyte(byte[] clearData, string Password)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();
            byte[] Key = CreateRijndaelKey(Password);
            byte[] IV = CreateRijndaelIV(Password);
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        public static byte[] RSADecrypt(byte[] CipherText, string Key)
        {
            RSACryptoServiceProvider sp = new RSACryptoServiceProvider();
            sp.FromXmlString(Key);
            return sp.Decrypt(CipherText, false);                 //(CipherText, false);            
        }
        public static string RSADecrypt(string Base64CipherText, string Key)
        {
            byte[] bEncryptedText = TCipher.Base64toBytes(Base64CipherText);
            byte[] bPlainText = TCipher.RSADecrypt(bEncryptedText, Key);
             return BytesToRegularString(bPlainText);
        }
        public static byte[] RSAEncrypt(byte[] PlainText, string Key)
        {
            RSACryptoServiceProvider sp = new RSACryptoServiceProvider();
            sp.FromXmlString(Key);
            return sp.Encrypt(PlainText, false);
        }
        //public static string RSAEncrypt(string PlainText, string Key)
        //{
        //    byte[] plainBytes = TCipher.RegularStringToBytes(PlainText);
        //    return TCipher.BytesToRegularString(RSAEncrypt(plainBytes, Key));
        //}
        public static string RSAEncryptToBase64(string PlainText, string Key)
        {
            byte[] plainBytes = TCipher.RegularStringToBytes(PlainText);
            return TCipher.BytesToBase64String(RSAEncrypt(plainBytes, Key));
        }

        // Decrypt bytes into bytes using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 
        public static byte[] DecryptByteToByte(byte[] cipherData, string Password)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            byte[] Key = CreateRijndaelKey(Password);
            byte[] IV = CreateRijndaelIV(Password);
            // Now get the key/IV and do the Decryption using the 
            //function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 

            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 
            return Decrypt(cipherData, Key, IV);
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        //
        public static byte[] Decrypt(byte[] cipherData,
                                    byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the next
            // line with something like 
            //     TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

        // Encrypt a file into another file using a password 
        //  returns the IV as base64 string
        public static void EncryptFileIV(string fileIn, string fileOut, string Password)
        {
            // First we are going to open the file streams 
            FileStream fsIn = new FileStream(fileIn,
                FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut,
                FileMode.OpenOrCreate, FileAccess.Write);

            Rijndael alg = Rijndael.Create();
            alg.Key = CreateRijndaelKey(Password); ;
            byte[] IV = RandomBytes(alg.BlockSize / 8);
            alg.IV = IV;
            BinaryWriter bw = new BinaryWriter(fsOut);
            bw.Write(IV, 0, IV.Length);

            // Now create a crypto stream through which we are going
            // to be pumping data. 
            // Our fileOut is going to be receiving the encrypted bytes. 
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be processing
            // the input file in chunks. 
            // This is done to avoid reading the whole file (which can
            // be huge) into memory. 
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                // read a chunk of data from the input file 
                bytesRead = fsIn.Read(buffer, 0, bufferLen);

                // encrypt it 
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);

            // close everything 

            // this will also close the unrelying fsOut stream
            cs.Close();
            fsIn.Close();
        }

        // Decrypt a file into another file using a Key and IV
        //
        public static void DecryptFileIV(string fileIn, string fileOut, string Password)
        {

            byte[] IV;
            // First we are going to open the file streams 
            FileStream fsIn = new FileStream(fileIn,
                        FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut,
                        FileMode.OpenOrCreate, FileAccess.Write);

            Rijndael alg = Rijndael.Create();

            alg.Key = CreateRijndaelKey(Password);
            BinaryReader reader = new BinaryReader(fsIn);
            IV = reader.ReadBytes(alg.BlockSize / 8);
            alg.IV = IV;  // pdb.GetBytes(16);
            // Now create a crypto stream through which we are going
            // to be pumping data. 
            // Our fileOut is going to be receiving the Decrypted bytes. 
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be 
            // processing the input file in chunks. 
            // This is done to avoid reading the whole file (which can be
            // huge) into memory. 

            byte[] buffer = reader.ReadBytes(8192);
            while (buffer.Length > 0)
            {
                cs.Write(buffer, 0, buffer.Length);
                buffer = reader.ReadBytes(8192);
            }
            // close everything 
            cs.Close(); // this will also close the unrelying fsOut stream 
            fsIn.Close();
        }

        // Encrypt a file into another file using a password  : Prepend Cleartext string
        //  returns the IV as base64 string
        public static void EncryptFileCT(string fileIn, string fileOut, string Password, string ClearText)
        {
            // First we are going to open the file streams 
            FileStream fsIn = new FileStream(fileIn,
                FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut,
                FileMode.OpenOrCreate, FileAccess.Write);

            Rijndael alg = Rijndael.Create();
            alg.Key = CreateRijndaelKey(Password); ;
            byte[] IV = RandomBytes(alg.BlockSize / 8);
            alg.IV = IV;
            BinaryWriter bw = new BinaryWriter(fsOut);
            bw.Write(ClearText);
            bw.Write(IV, 0, IV.Length);
            // Now create a crypto stream through which we are going
            // to be pumping data. 
            // Our fileOut is going to be receiving the encrypted bytes. 
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be processing
            // the input file in chunks. 
            // This is done to avoid reading the whole file (which can
            // be huge) into memory. 
            int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int bytesRead;

            do
            {
                // read a chunk of data from the input file 
                bytesRead = fsIn.Read(buffer, 0, bufferLen);

                // encrypt it 
                cs.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);

            // close everything 

            // this will also close the unrelying fsOut stream
            cs.Close();
            fsIn.Close();
        }

        // Decrypt a file into another file using a Password -- return Prepended Cleartext String
        //
        public static string DecryptFileCT(string fileIn, string fileOut, string Password)
        {

            // First we are going to open the file streams 
            FileStream fsIn = new FileStream(fileIn,
                        FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut,
                        FileMode.OpenOrCreate, FileAccess.Write);

            Rijndael alg = Rijndael.Create();

            alg.Key = CreateRijndaelKey(Password);
            BinaryReader reader = new BinaryReader(fsIn);
            string ct = reader.ReadString();
            alg.IV = reader.ReadBytes(alg.BlockSize / 8);
            // Now create a crypto stream through which we are going
            // to be pumping data. 
            // Our fileOut is going to be receiving the Decrypted bytes. 
            CryptoStream cs = new CryptoStream(fsOut,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be 
            // processing the input file in chunks. 
            // This is done to avoid reading the whole file (which can be
            // huge) into memory. 

            byte[] buffer = reader.ReadBytes(8192);
            while (buffer.Length > 0)
            {
                cs.Write(buffer, 0, buffer.Length);
                buffer = reader.ReadBytes(8192);
            }
            // close everything 
            cs.Close(); // this will also close the unrelying fsOut stream 
            fsIn.Close();
            return ct;
        }
        private static int ProcessInput(string s)
        {
            while (s.Contains(" "))
            {
                int i = s.IndexOf(' ');
                s = s.Remove(i, 1);
            }
            s = s.ToUpper();
            int v = TCipher.csHashint32(s);
            return v;
        }
        public static string csSliceAndDice64(this string s)
        {
            return SliceAndDice(s);
        }
        public static string SliceAndDice(string s)
        {

            byte[] data = csHashBytes(s);
            int k = 7;
            for (int i = 7; i < 100; i++)
            {
                if ((data.Length % i) > 0)
                {
                    k = i;
                    break;
                }
            }
            byte[] temp = new byte[data.Length];
            int n = temp.Length / 2;
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = data[n];
                n += k;
                n %= temp.Length;
            }
            return BytesToBase64String(temp);
        }

     
    }
    
//     using System;
//using System.Security.Cryptography;
 
public class Crc32 : HashAlgorithm {
    public const uint DefaultSeed = 0xffffffff;
 
    readonly static uint[] CrcTable = new uint[] {
            0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419,
            0x706AF48F, 0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4,
            0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07,
            0x90BF1D91, 0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE,
            0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7, 0x136C9856,
            0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9,
            0xFA0F3D63, 0x8D080DF5, 0x3B6E20C8, 0x4C69105E, 0xD56041E4,
            0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
            0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3,
            0x45DF5C75, 0xDCD60DCF, 0xABD13D59, 0x26D930AC, 0x51DE003A,
            0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599,
            0xB8BDA50F, 0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924,
            0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D, 0x76DC4190,
            0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F,
            0x9FBFE4A5, 0xE8B8D433, 0x7807C9A2, 0x0F00F934, 0x9609A88E,
            0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
            0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED,
            0x1B01A57B, 0x8208F4C1, 0xF50FC457, 0x65B0D9C6, 0x12B7E950,
            0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3,
            0xFBD44C65, 0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2,
            0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A,
            0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5,
            0xAA0A4C5F, 0xDD0D7CC9, 0x5005713C, 0x270241AA, 0xBE0B1010,
            0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
            0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17,
            0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6,
            0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615,
            0x73DC1683, 0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8,
            0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1, 0xF00F9344,
            0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB,
            0x196C3671, 0x6E6B06E7, 0xFED41B76, 0x89D32BE0, 0x10DA7A5A,
            0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
            0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1,
            0xA6BC5767, 0x3FB506DD, 0x48B2364B, 0xD80D2BDA, 0xAF0A1B4C,
            0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF,
            0x4669BE79, 0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236,
            0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F, 0xC5BA3BBE,
            0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31,
            0x2CD99E8B, 0x5BDEAE1D, 0x9B64C2B0, 0xEC63F226, 0x756AA39C,
            0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
            0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B,
            0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21, 0x86D3D2D4, 0xF1D4E242,
            0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1,
            0x18B74777, 0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C,
            0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45, 0xA00AE278,
            0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7,
            0x4969474D, 0x3E6E77DB, 0xAED16A4A, 0xD9D65ADC, 0x40DF0B66,
            0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
            0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605,
            0xCDD70693, 0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8,
            0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B,
            0x2D02EF8D
        };
 
    uint crcValue = 0;
    public override void Initialize()
    {
        crcValue = 0;
    }
    //public override void Initialize() {
    //    crcValue = 0;
    //}
 
    protected override void HashCore(byte[] buffer, int start, int length) {
        crcValue ^= DefaultSeed;
 
        unchecked {
            while (--length >= 0) {
                crcValue = CrcTable[(crcValue ^ buffer[start++]) & 0xFF] ^ (crcValue >> 8);
            }
        }
 
        crcValue ^= DefaultSeed;
    }
    protected override byte[] HashFinal() {
        this.HashValue = new byte[] { (byte)((crcValue >> 24) & 0xff), 
                                      (byte)((crcValue >> 16) & 0xff), 
                                      (byte)((crcValue >> 8) & 0xff), 
                                      (byte)(crcValue & 0xff) };
        return this.HashValue;
    }
    public uint CrcValue {
        get {
            return (uint)((HashValue[0] << 24) | (HashValue[1] << 16) | (HashValue[2] << 8) | HashValue[3]);
        }
    }
    public override int HashSize {
        get { return 32; }
    }
}
     
    public class TCRC7z
    {
        public static uint[] Table;

        public TCRC7z()
        {
            if (Table == null)
            {
                TCRC7z.Table = new uint[256];
                const uint kPoly = 0xEDB88320;
                for (uint i = 0; i < 256; i++)
                {
                    uint r = i;
                    for (int j = 0; j < 8; j++)
                        if ((r & 1) != 0)
                            r = (r >> 1) ^ kPoly;
                        else
                            r >>= 1;
                    TCRC7z.Table[i] = r;
                }
            }
        }

        static uint _value = 0xFFFFFFFF;

        public void Init() { _value = 0xFFFFFFFF; }

        public static void UpdateByte(byte b)
        {
            _value = Table[(((byte)(_value)) ^ b)] ^ (_value >> 8);
        }
        public static void Update(byte[] data, uint offset, uint size)
        {
            for (uint i = 0; i < size; i++)
                _value = Table[(((byte)(_value)) ^ data[offset + i])] ^ (_value >> 8);
        }
        public static uint GetDigest() { return _value ^ 0xFFFFFFFF; }

        public static uint CalculateDigest(byte[] data, uint offset, uint size)
        {
            TCRC7z crc = new TCRC7z();
            crc.Init();
            TCRC7z.Update(data, offset, size);
            return TCRC7z.GetDigest();
        }
        public static uint CalculateDigest(string inString)
        {
            TCRC7z crc = new TCRC7z();
            crc.Init();
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] data = UE.GetBytes(inString);
            TCRC7z.Update(data, 0, (uint)data.Length);
            return TCRC7z.GetDigest();
        }
        public static int CalculateDigestInt(string instring)
        {
            return (int)CalculateDigest(instring);
        }
        public static bool VerifyDigest(uint digest, byte[] data, uint offset, uint size)
        {
            return (CalculateDigest(data, offset, size) == digest);
        }
        public static bool VerifyDigest(uint digest, string inString)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] data = UE.GetBytes(inString);

            return (CalculateDigest(data, 0, (uint)data.Length) == digest);
        }


    }
    public class TReEventTypetration
    {
        public int USERID;
        public int KEY;
        public string NAME;
        public TReEventTypetration(int Userid, int Key, string Name)
        {
            USERID = Userid;
            KEY = Key;
            NAME = Name;
        }
        public TReEventTypetration(string Userid_HEX, string Key_DD, string Name)
        {
            USERID = MyLib.HexToInt(Userid_HEX);
            KEY = TCipher.DottedToInt(Key_DD);
            NAME = Name;
        }
        public TReEventTypetration(string Userid_HEX, int Key, string Name)
        {
            USERID = MyLib.HexToInt(Userid_HEX);
            KEY = Key;
            NAME = Name;
        }
        public bool Validate(string password)
        {
            return KEY == TCipher.GenerateTumbler(USERID, NAME, password);
        }
    }
}
