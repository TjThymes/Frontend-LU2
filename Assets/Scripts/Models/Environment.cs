using System.Collections.Generic;
namespace Assets.Scripts.Models
{
    [System.Serializable]
    public class EnvironmentResponse
    {
        public string id;
        public string name;
        public string description;
        public int width;
        public int height;
        public List<GameObjectData> objects;
    }
}