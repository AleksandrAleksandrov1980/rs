using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace RastrSrvShare;

public class CSigner
{
    public static string str_fname_prv_xml = "key_prv.xml";
    public static string str_fname_pub_xml = "key_pub.xml";
    public RSACryptoServiceProvider m_rsa_alg = new RSACryptoServiceProvider();

    public byte[]? HashAndSignBytes(byte[] DataToSign) 
    {
        try
        {
            return m_rsa_alg.SignData(DataToSign, SHA256.Create()); // PRIVATE KEY
        }
        catch(CryptographicException e)
        {
            return null;
        }
    }

    public bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData) 
    {
        try
        {
            return m_rsa_alg.VerifyData(DataToVerify, SHA256.Create(), SignedData); // PUBLIC KEY
        }
        catch(CryptographicException e)
        {
            return false;
        }
    }

    public int ReadKey(string str_fname_xml)
    { 
        try
        { 
            XDocument doc = XDocument.Load(str_fname_xml);
            string str_xml = doc.ToString();
            m_rsa_alg.FromXmlString(str_xml.ToString());
        }
        catch
        {
            return -1;
        }
        return 1;
    }
}
