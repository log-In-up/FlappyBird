namespace SaveSystem
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);
        void NewGame(GameData data);
        void SaveData(GameData data);
    }
}