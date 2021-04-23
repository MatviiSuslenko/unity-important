using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SafePlayerPrefs
{
    private const int salt = 228909675;
    private const string format = "_{0}";

    public static void SetInt(string key, int value)
    {
        int salted = value ^ salt;
        PlayerPrefs.SetInt(HashString(key), salted);
        PlayerPrefs.SetInt(HashString(string.Format(format, key)), HashInt(value));
    }

    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    public static int GetInt(string key, int defaultValue)
    {
        string hashedKey = HashString(key);
        if (!PlayerPrefs.HasKey(hashedKey))
        {
            return defaultValue;
        }

        int salted = PlayerPrefs.GetInt(hashedKey);
        int value = salted ^ salt;

        int loadedHash = PlayerPrefs.GetInt(HashString(string.Format(format, key)));
        if (loadedHash != HashInt(value)){
            return defaultValue;
        }

        return value;
    }

    public static void SetFloat(string key, float value)
    {
        int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);

        int salted = intValue ^ salt;
        PlayerPrefs.SetInt(HashString(key), salted);
        PlayerPrefs.SetInt(HashString(string.Format(format, key)), HashInt(intValue));
    }

    public static float GetFloat(string key)
    {
        return GetFloat(key, 0);
    }

    public static float GetFloat(string key, float defaultValue)
    {
        string hashedKey = HashString(key);
        if (!PlayerPrefs.HasKey(hashedKey))
        {
            return defaultValue;
        }

        int salted = PlayerPrefs.GetInt(hashedKey);
        int value = salted ^ salt;

        int loadedHash = PlayerPrefs.GetInt(HashString(string.Format(format, key)));
        if (loadedHash != HashInt(value))
        {
            return defaultValue;
        }

        return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }

    public static int HashInt(int x)
    {
        x = ((x >> 16) ^ x) * 0x45d9f3b;
        x = ((x >> 16) ^ x) * 0x45d9f3b;
        x = (x >> 16) ^ x;
        return x;
    }

    public static string HashString(string x)
    {
        HashAlgorithm algorithm = SHA256.Create();
        StringBuilder sb = new StringBuilder();

        var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(x));
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(HashString(key));
        PlayerPrefs.DeleteKey(HashString(string.Format(format, key)));
    }

    public static bool HasKey(string key)
    {
        string hashedKey = HashString(key);
        if (!PlayerPrefs.HasKey(hashedKey))
        {
            return false;
        }

        int salted = PlayerPrefs.GetInt(hashedKey);
        int value = salted ^ salt;

        int loadedHash = PlayerPrefs.GetInt(HashString(string.Format(format, key)));

        return loadedHash == HashInt(value);
    }
}
