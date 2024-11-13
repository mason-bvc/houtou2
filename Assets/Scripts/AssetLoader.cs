using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    public static AssetBundle AssetBundle;

    protected void Awake()
    {
        AssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/assetbundle");

        Resources.Audio.Explosion1 = AssetBundle.LoadAsset<AudioClip>("Explosion1");
        Resources.Audio.FairyDie = AssetBundle.LoadAsset<AudioClip>("FairyDie");
        Resources.Audio.GameOver = AssetBundle.LoadAsset<AudioClip>("CV3GameOver");
        Resources.Audio.PlayerHurt = AssetBundle.LoadAsset<AudioClip>("PlayerHurt");
        Resources.Audio.PlayerShoot = AssetBundle.LoadAsset<AudioClip>("PlayerShoot");
        Resources.Audio.Teleport = AssetBundle.LoadAsset<AudioClip>("Teleport");
        Resources.Prefabs.Bird = AssetBundle.LoadAsset<GameObject>("Bird");
        Resources.Prefabs.Fairy = AssetBundle.LoadAsset<GameObject>("Fairy");
        Resources.Prefabs.King = AssetBundle.LoadAsset<GameObject>("King");
        Resources.Prefabs.LaStake = AssetBundle.LoadAsset<GameObject>("LaStake");
        Resources.Prefabs.LaStakeBullet = AssetBundle.LoadAsset<GameObject>("LaStakeBullet");
        Resources.Prefabs.PlayerBullet = AssetBundle.LoadAsset<GameObject>("PlayerBullet");
        Resources.Materials.Background = AssetBundle.LoadAsset<Material>("Background");
        Resources.Materials.Background2 = AssetBundle.LoadAsset<Material>("Background2");
        Resources.Sprites.CorrinePortrait = AssetBundle.LoadAsset<Sprite>("CorinnePortrait");
        Resources.Sprites.LaStakePortrait = AssetBundle.LoadAsset<Sprite>("LaStakePortrait");
    }
}
