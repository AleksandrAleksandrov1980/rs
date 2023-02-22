using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace genkeys;

class Program
{
    public static byte[]? HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
    {
        try
        {
            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAalg.ImportParameters(Key); // PRIVATE KEY
            // Hash and sign the data. Pass a new instance of SHA256
            // to specify the hashing algorithm.
            return RSAalg.SignData(DataToSign, SHA256.Create());
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
    {
        try
        {
            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            RSAalg.ImportParameters(Key); // PUBLIC KEY
            // Verify the data using the signature.  Pass a new instance of SHA256
            // to specify the hashing algorithm.
            return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static void tst_crypto()
    {
        try
        {
            // Create a UnicodeEncoder to convert between byte array and string.
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            string dataString = "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd"+
            "Data to Sign fghfgdhd ghghgfhfghddghfghdghfgdhfghfd";
            // Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] originalData = ByteConverter.GetBytes(dataString);
            byte[] signedData;
            // Create a new instance of the RSACryptoServiceProvider class
            // and automatically create a new key-pair.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            // Export the key information to an RSAParameters object.
            // You must pass true to export the private key for signing.
            // However, you do not need to export the private key
            // for verification.
            RSAParameters Key = RSAalg.ExportParameters(true);
            RSAParameters Key1 = RSAalg.ExportParameters(true);
            byte[] b_priv_key = RSAalg.ExportRSAPrivateKey();
            byte[] b_priv_key1 = RSAalg.ExportRSAPrivateKey();
            byte[] b_publ_key = RSAalg.ExportRSAPublicKey();
            byte[] b_publ_key2 = RSAalg.ExportRSAPublicKey();


            string str_xml1 = RSAalg.ToXmlString(true);
            string str_xml2 = RSAalg.ToXmlString(false);

            XDocument doc1 = XDocument.Parse(str_xml1);
            string s1 = doc1.ToString();

            XDocument doc2 = XDocument.Parse(str_xml2);
            string s2 = doc2.ToString();

            doc1.Save("key_prv.xml");
            doc2.Save("key_pub.xml");


            XDocument doc_prv_2 = XDocument.Load("key_prv.xml");
            XDocument doc_pub_2 = XDocument.Load("key_pub.xml");

            RSAalg.FromXmlString(doc_prv_2.ToString());
            //RSAalg.FromXmlString(doc_pub_2.ToString());
           
            string str_xml4 = RSAalg.ToXmlString(false);
            string str_xml3 = RSAalg.ToXmlString(true);
            
            //RSAParameters Key = RSAalg.ExportParameters(false);
            // Hash and sign the data.
            signedData = HashAndSignBytes(originalData, Key);
            // Verify the data and display the result to the
            // console.
            if(VerifySignedHash(originalData, signedData, Key))
            {
                Console.WriteLine("The data was verified.");
            }
            else
            {
                Console.WriteLine("The data does not match the signature.");
            }
        }
        catch(ArgumentNullException)
        {
            Console.WriteLine("The data was not signed or verified");
        }
    }

    public static void save_xml_str(string str_name_f, string str_xml)
    {
        XDocument doc = XDocument.Parse(str_xml);
        string str_xml_pretty = doc.ToString();
        doc.Save(str_name_f);
        Console.WriteLine($"writed file: {str_name_f}");
    }

    public static void save_keys()
    {
        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        // Export the key information to an RSAParameters object.
        // You must pass true to export the private key for signing.
        // However, you do not need to export the private key
        // for verification.
        RSAParameters Key = RSAalg.ExportParameters(true);
        Console.WriteLine($"ПРИВАТНЫЙ ключ!");
        save_xml_str( "key_prv.xml", RSAalg.ToXmlString(true));
        Console.WriteLine($"публичный ключ!");
        save_xml_str( "key_pub.xml", RSAalg.ToXmlString(false));
    }


    //dotnet publish -r Win-x64 -p:PublishSingleFile=true --self-contained true
    //dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -p:PublishTrimmed=true
    
    static void Main(string[] args)
    {
        try
        {
            save_keys();
        }
        catch(CryptographicException e)
        {
            Console.WriteLine(e.Message);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("ok!");
    }
}
