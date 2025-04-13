using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoPrefabMaker
{
    [MenuItem("Tools/Generate Prefabs From Sprites")]
    public static void GeneratePrefabs()
    {
        string spritesFolder = "Assets/Assets/kenney_tiny-town/Tiles/";
        string prefabFolder = "Assets/Prefabs/";

        if (!Directory.Exists(prefabFolder))
        {
            Directory.CreateDirectory(prefabFolder);
        }

        // 🔥 Zoek alle sprites via AssetDatabase
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { spritesFolder });

        if (guids.Length == 0)
        {
            Debug.LogError("❌ Geen sprites gevonden in: " + spritesFolder);
            return;
        }

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

            if (sprite == null) continue;

            GameObject go = new GameObject(sprite.name);
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;

            string localPath = prefabFolder + sprite.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(go, localPath);

            GameObject.DestroyImmediate(go);
        }

        Debug.Log($"✅ {guids.Length} prefabs succesvol aangemaakt in {prefabFolder}");
    }
}
