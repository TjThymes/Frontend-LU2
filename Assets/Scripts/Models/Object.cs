using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class GameObjectData
    {
        public string id;
        public string environmentID;
        public int prefabID;
        public float positionX;
        public float positionY;
        public float scaleX;
        public float scaleY;
        public float rotationZ;
        public int sortingLayer;
    }
}
