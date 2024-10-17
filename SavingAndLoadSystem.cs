using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

public class SavingAndLoadSystem : MonoBehaviour
{
    private string path = Path.GetFullPath("./") + "\\GameProgress.txt";
    private string keyPath = Path.GetFullPath("./") + "\\key.key";
    private string vectorPath = Path.GetFullPath("./") + "\\vector.vector";
    private PlayerData data;
    RijndaelManaged myRijndael = new RijndaelManaged();

    private void Start()
    {
        myRijndael.GenerateKey();
        myRijndael.GenerateIV();
        Load();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.F)) Load();
    }

    public void Save()
    {
        data = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerData>();
        string dataToStore = JsonUtility.ToJson(data);
        byte[] encrypted = EncryptStringToBytes(dataToStore, myRijndael.Key, myRijndael.IV);
        File.WriteAllBytes(path, encrypted);

        using(FileStream stream = new FileStream(keyPath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, myRijndael.Key);
        }

        using (FileStream stream = new FileStream(vectorPath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, myRijndael.IV);
        }

        string builder = "";
        for (int i = 0; i < myRijndael.Key.Length; i++) builder+= myRijndael.Key[i].ToString("x2");
        Debug.Log(builder);
        Debug.Log(dataToStore);

        myRijndael.GenerateKey();
        myRijndael.GenerateIV();
    }

    public void Load()
    {
        try {
            if (File.Exists(path) && File.Exists(keyPath) && File.Exists(vectorPath))
            {
                data = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerData>();
                byte[] encrypted = File.ReadAllBytes(path);

                RijndaelManaged oldManaged = new RijndaelManaged();

                using (FileStream stream = new FileStream(keyPath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    oldManaged.Key = formatter.Deserialize(stream) as byte[];
                }

                using (FileStream stream = new FileStream(vectorPath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    oldManaged.IV = formatter.Deserialize(stream) as byte[];
                }

                string dataToLoad = DecryptStringFromBytes(encrypted, oldManaged.Key, oldManaged.IV);
                JsonUtility.FromJsonOverwrite(dataToLoad, data);
                data.Recount();

                string builder = "";
                for (int i = 0; i < oldManaged.Key.Length; i++) builder += oldManaged.Key[i].ToString("x2");
                Debug.Log(builder);
                Debug.Log(dataToLoad);
            }
            FileCreate(path); FileCreate(keyPath); FileCreate(vectorPath);
        }
        catch(Exception ex) { Debug.Log(ex); }
    }

    void FileCreate(string _path)
    {
        if (!File.Exists(_path)) File.Create(_path).Dispose();
    }

    byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        if (plainText == null || plainText.Length <= 0) throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0) throw new ArgumentNullException("IV");

        byte[] encrypted;

        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return encrypted;
    }

    string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        if (cipherText == null || cipherText.Length <= 0) throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0) throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0) throw new ArgumentNullException("IV");
 
        string plaintext = null;

        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}
