namespace Persistence
{
    public class SaveSystem
    {
        IPersistence persistence;

        public SaveSystem(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public void Save(GameData data)
        {
            persistence.Save(data);
        }

        public GameData Load()
        {
            return persistence.Load<GameData>();
        }

        public GameData LoadFromResources(string path)
        {
            return persistence.LoadFromResources<GameData>(path);
        }
    }
}
