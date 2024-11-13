using UnityEngine;

// Rationale: in such a time-sensitive environment, screwing and drilling down
// in the inspector to check if all the resource references are tight sounds
// like a horrible nightmare from which I could never return.

public static class Resources
{
    public static class Audio
    {
        public static AudioClip PlayerShoot;
        public static AudioClip PlayerHurt;
        public static AudioClip FairyDie;
        public static AudioClip Explosion1;
        public static AudioClip GameOver;
    }

    public static class Prefabs
    {
        public static GameObject PlayerBullet;
        public static GameObject Bird;
        public static GameObject Fairy;
    }

    public static class Sprites
    {
        public static Sprite Corrine;
        public static Sprite CorrinePortrait;
        public static Sprite LaStakePortrait;
    }
}
