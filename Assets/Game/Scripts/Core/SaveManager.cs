using System.IO;
using UnityEngine;

namespace Game.Core
{
    public static class SaveManager
    {
        public static void SaveData<T>(T data, string fileName)
        {
            try
            {
                string path = Path.Combine(Application.persistentDataPath, fileName);
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
                Debug.Log(fileName + " saved successfully");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Save failed " + fileName);
                Debug.LogException(e);
            }
        }

        public static T LoadData<T>(string fileName, T defaulValue)
        {
            try
            {
                string path = Path.Combine(Application.persistentDataPath, fileName);
                if(File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    T data = JsonUtility.FromJson<T>(json);
                    Debug.Log(fileName + " load successfully");
                    return data;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Load failed " + fileName);
                Debug.LogException(e);
            }
            return defaulValue;
        } 

        public static bool FileExists(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            return File.Exists(path);
        }
    }
}