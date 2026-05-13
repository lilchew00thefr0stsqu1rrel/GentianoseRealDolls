using System;
using System.IO;
using UnityEngine;

namespace TowerDefense
{
    [Serializable]
    public class Saver<T>
    {
        
        public static void TryLoad(string fileName, ref T data)
        {
            var path = FileHandler.Path(fileName);
            Debug.Log(path);
            if (File.Exists(path))
            {
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<Saver<T>>(dataString);
                data = saver.data;
            }
        }

        public static void TryLoad2(string path, ref T data)
        {
            Debug.Log(path);
            if (File.Exists(path))
            {
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<Saver<T>>(dataString);
                data = saver.data;
            }
        }

        public static void Save(string fileName, T data)
        {
            var wrapper = new Saver<T> { data = data };
            var dataString = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FileHandler.Path(fileName), dataString);
        }

        public static void Save2(string path, T data)
        {
            if (File.Exists(path))
            {
                var wrapper = new Saver<T> { data = data };
                var dataString = JsonUtility.ToJson(wrapper);
                File.WriteAllText(path, dataString);
            }
        }


        public T data;
    }

    public static class FileHandler
    {
        public static string Path(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}";
        }

        public static void Reset(string fileName)
        {
            var path = FileHandler.Path(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        internal static bool HasFile(string fileName)
        {
            return File.Exists(Path(fileName));
        }
    }
}