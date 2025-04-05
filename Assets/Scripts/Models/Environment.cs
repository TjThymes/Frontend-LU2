using System.Collections.Generic;

[System.Serializable]
public class EnvironmentResponse
{
    public string id;
    public string name;
    public string description;
    public List<GameObjectData> objects;
}
