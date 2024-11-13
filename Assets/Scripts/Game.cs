using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Note: not implementing a true singleton because I deemed it unnecessary.

public class Game : MonoBehaviour
{
    //
    // components
    //
    public static AudioSource AudioSource;

    //
    // dialog stuff
    //
    private static TMP_Text _dialog;
    private static string _dialogText = "";
    private static float _dialogTextVisibleRatio;
    private static float _dialogT;
    private static Image _dialogPortrait;

    //
    // hud stuff
    //
    private static TMP_Text _wave;
    private static Image[] _hearts;

    //
    // resources
    //
    public static AssetBundle AssetBundle { get; protected set; }

    //
    // game state
    //
    private static Background _background;
    private static EnemySpawner _birdSpawner;
    private static EnemySpawner _fairySpawner;
    private static FunCamera _funCamera;
    public static Player Player { get; protected set; }

    // Rationale: When you put the responsibility on either object to delete
    // itself based on the others' tag in their own scripts, you do two things.
    // Number one, you rely on the execution order of the scripts of the
    // respective game objects to handle deleting themselves in the proper
    // order.* Number two, you give the game objects a weak awareness of each
    // other, slightly increasing the coupling. Not only those, but you also
    // increase the number of contribution points and "jumping around" you have
    // to do in the codebase in order to change the deletion behavior. That all
    // majorly sucks.

    // Because this project is due very soon, rapid development concerns start
    // to pile up quick. If this was an honest-to-God piece of software that
    // was managed by a medium-large company that does sprints, kanbas, scrums,
    // and all that crap, I would probably add a
    // "TriggerEnter2DBehaviorProvider" that acts as somewhat of a Command
    // Pattern Factory that itself decides what should be done when two
    // colliders of two different tags hit each other in order to divide
    // responsibilities, but I really do not have time or desire to prematurely
    // abstract and architect like I'm a Gang of Four enthusiast from the
    // mid-'90s.

    // So in the end, we get a monolithic Game class, but we also get
    // some dependency inversion and _some_ decoupling, at least between the
    // two colliding objects. The objects themselves must still be coupled with
    // this Game manager object since they have to directly call into its
    // static fields because, again, time concerns. This can be alleviated with
    // some simple dependency injection, but as far as I know, there is no
    // event you can hook into that fires when a Transform is added to a scene
    // wherein to "Inject" your "Dependencies" through a hypothetical
    // higher-order object.

    // * you could defer the deletion of the objects to the end of the frame,
    // but you still run into problem number two.

    // public static void HandleTriggerEnter2D(Collider2D collider1, Collider2D collider2)
    // {
    //     if (collider1.CompareTag(Global.TAG_ENEMY) && collider2.CompareTag(Global.TAG_PLAYER_BULLET))
        // {
            // TODO: do this only when it's a fairy
            // for (var i = 0; i < 3; i++)
            // {
            //     var bird = Instantiate(Resources.Prefabs.Bird);
            //     var birdComponent = bird.GetComponent<Bird>();

            //     bird.transform.position = collider1.transform.position;
            //     birdComponent.Direction = Util.RandomDirection;
            // }

    //         Destroy(collider1.transform.root.gameObject);
    //         Destroy(collider2.transform.root.gameObject);
    //         _audioSource.PlayOneShot(Resources.Audio.Explosion1);

    //         return;
    //     }

    //     if (collider1.CompareTag(Global.TAG_PLAYER) && collider2.CompareTag(Global.TAG_HURT_PLAYER))
    //     {
    //         collider1.transform.root.GetComponent<Health>().Damage(25.0F);
    //         _audioSource.PlayOneShot(Resources.Audio.PlayerHurt);
    //         _funCamera.Shake();
    //     }
    // }

    public static float CalculateDamage(GameObject damager, GameObject victim)
    {
        if (damager.CompareTag(Global.TAG_PLAYER_BULLET) && victim.CompareTag(Global.TAG_ENEMY))
        {
            return 25.0F;
        }

        if (victim.CompareTag(Global.TAG_PLAYER))
        {
            if (damager.CompareTag(Global.TAG_ENEMY) || damager.CompareTag(Global.TAG_HURT_PLAYER))
            {
                return 25.0F;
            }
        }

        Debug.Log("No appropriate damage found");

        return 0.0F;
    }

    private static void OnPlayerDamaged()
    {
        for (var i = _hearts.Length - 1; i >= 0; i--)
        {
            _hearts[i].gameObject.SetActive(true);

            if (Player.Health.GetHealth() / Player.Health.InitialHealth * _hearts.Length <= i)
            {
                _hearts[i].gameObject.SetActive(false);
            }
        }

        if (Player.Health.GetHealth() <= 0.0F)
        {
            Destroy(GameObject.Find("Playism"));
            return;
        }

        _funCamera.Shake();
    }

    protected void Start()
    {
        AssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/assetbundle");

        Resources.Audio.Explosion1 = AssetBundle.LoadAsset<AudioClip>("Explosion1");
        Resources.Audio.PlayerHurt = AssetBundle.LoadAsset<AudioClip>("PlayerHurt");
        Resources.Audio.PlayerShoot = AssetBundle.LoadAsset<AudioClip>("PlayerShoot");
        Resources.Audio.FairyDie = AssetBundle.LoadAsset<AudioClip>("FairyDie");
        Resources.Prefabs.Bird = AssetBundle.LoadAsset<GameObject>("Bird");
        Resources.Prefabs.Fairy = AssetBundle.LoadAsset<GameObject>("Fairy");
        Resources.Sprites.CorrinePortrait = AssetBundle.LoadAsset<Sprite>("CorinnePortrait");
        Resources.Sprites.LaStakePortrait = AssetBundle.LoadAsset<Sprite>("LaStakePortrait");

        AudioSource = GetComponent<AudioSource>();
        _background = GameObject.Find("Playism/Background")?.GetComponent<Background>();
        _dialog = GameObject.Find("Playism/Canvas/Dialog")?.GetComponent<TMP_Text>();
        _dialogPortrait = GameObject.Find("Playism/Canvas/Portrait")?.GetComponent<Image>();
        _birdSpawner = GameObject.Find("Playism/BirdSpawner")?.GetComponent<EnemySpawner>();
        _fairySpawner = GameObject.Find("Playism/FairySpawner")?.GetComponent<EnemySpawner>();
        _funCamera = GameObject.Find("Camera")?.GetComponent<FunCamera>();
        _hearts = GameObject.Find("Playism/Canvas/Hearts").GetComponentsInChildren<Image>();
        _wave = GameObject.Find("Playism/Canvas/WaveText")?.GetComponent<TMP_Text>();
        Player = FindObjectOfType<Player>();

        static IEnumerator PostPlayerInit()
        {
            yield return new WaitForEndOfFrame();
            Player.Health.Damaged.AddListener(OnPlayerDamaged);
        }

        StartCoroutine(PostPlayerInit());
        StartCoroutine(Sequence1());
        StartCoroutine(Sequence2());
        StartCoroutine(Sequence3());
        StartCoroutine(Sequence4());
        StartCoroutine(Sequence5());
        StartCoroutine(Sequence6());
    }

    protected void Update()
    {
        _dialogT += Time.deltaTime;
        _dialogT = Mathf.Clamp(_dialogT, 0, 1);
        _dialogTextVisibleRatio = Mathf.Lerp(0, 1, _dialogT);
        _dialog.text = _dialogText[..(int)(_dialogText.Length * _dialogTextVisibleRatio)];
    }

    private IEnumerator SetDialog(string text, float waitSeconds, Sprite portrait)
    {
        _dialogPortrait.gameObject.SetActive(true);
        _dialogPortrait.sprite = portrait;
        _dialogText = text;
        _dialogT = 0;

        yield return new WaitForSeconds(waitSeconds);

        _dialogText = "";
        _dialogPortrait.gameObject.SetActive(false);
    }

    private IEnumerator Sequence1()
    {
        yield return SetDialog("Ohohoh! Corinne, welcome to your personal Hell!", 5.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("You're toast when I get a hold of you, bloodsucker.", 5.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("I think not, darling...", 2.0F, Resources.Sprites.LaStakePortrait);
        StartCoroutine(_birdSpawner.BeginSpawn());
        yield return SetDialog("FAIRIES! Dispose of her at once!", 5.0F, Resources.Sprites.LaStakePortrait);
        _background.Color = Color.blue / 2.0F;
    }

    private IEnumerator Sequence2()
    {
        // TODO: ramp up
        yield return new WaitForSeconds(58.0F);
        yield return SetDialog("Here they come.", 3.0F, Resources.Sprites.CorrinePortrait);
        _fairySpawner.IsStopped = false;
        StartCoroutine(_fairySpawner.BeginSpawn());
        _birdSpawner.CurrentMaxEnemies = 10;
    }

    private IEnumerator Sequence3()
    {
        // TODO: ramp up
        yield return new WaitForSeconds(60.0F * 2.0F + 20.0F);
        yield return SetDialog("Having trouble keeping up, darling?", 4.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("This is child's play.", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("Such false bravado...", 4.0F, Resources.Sprites.LaStakePortrait);
        _wave.text = "Wave 2";
        _fairySpawner.CurrentMaxEnemies = 10;
        _birdSpawner.CurrentMaxEnemies = 15;
        _background.Color = Color.green / 2.0F;
    }

    private IEnumerator Sequence4()
    {
        yield return new WaitForSeconds(60.0F * 3.0F + 16.0F);

        _fairySpawner.CurrentMaxEnemies = 25;
        _birdSpawner.CurrentMaxEnemies = 10;
        _background.Color /= 2.0F;
    }

    private IEnumerator Sequence5()
    {
        yield return new WaitForSeconds(60.0F * 4.0F);

        _birdSpawner.CurrentMaxEnemies = 25;
        _background.Color *= 2.0F;
    }

    private IEnumerator Sequence6()
    {
        yield return new WaitForSeconds(60.0F * 4.0F + 6.0F);
        _background.Color = new Color(1.0F, 1.0F, 0.0F, 1.0F) / 2.0F;
    }
}
