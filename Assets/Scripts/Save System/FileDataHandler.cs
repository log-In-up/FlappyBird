using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public sealed class FileDataHandler
    {
        #region Fields
        private readonly string _dataDirPath = "", _dataFileName = "";
        #endregion

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
        }

        #region Public methods
        public GameData Load()
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = string.Empty;
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error occured when trying to load data from file: {fullPath} {Environment.NewLine} {exception}");
                }
            }
            return loadedData;
        }

        public void Save(GameData data)
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = JsonUtility.ToJson(data, true);

                using FileStream stream = new FileStream(fullPath, FileMode.Create);
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error occured when trying to save data to file: {fullPath} {Environment.NewLine} {exception}");
            }
        }
        #endregion
    }
}