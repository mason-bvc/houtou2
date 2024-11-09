using UnityEngine;

public class Game : MonoBehaviour
{
    public static AssetBundle AssetBundle { get; protected set; }
    public static Player Player { get; protected set; }

    protected void Start()
    {
        AssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/assetbundle");
        Player = FindObjectOfType<Player>();
    }
}
