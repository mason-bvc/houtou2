using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Note: not implementing a true singleton because I deemed it unnecessary.

public class Game : MonoBehaviour
{
    //
    // components
    //
    private static AudioSource _audioSource;

    //
    // dialog stuff
    //
    private static TMP_Text _dialog;
    private static string _dialogText = "";
    private static float _dialogTextVisibleRatio;
    private static float _dialogT;
    private static Image _dialogPortrait;

    //
    // resources
    //
    public static AssetBundle AssetBundle { get; protected set; }

    //
    // game state
    //
    private static FunCamera _funCamera;
    private static EnemySpawner _enemySpawner;
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

    public static void HandleTriggerEnter2D(Collider2D collider1, Collider2D collider2)
    {
        if (collider1.CompareTag(Global.TAG_ENEMY) && collider2.CompareTag(Global.TAG_PLAYER_BULLET))
        {
            // TODO: do this only when it's a fairy
            // for (var i = 0; i < 3; i++)
            // {
            //     var bird = Instantiate(Resources.Prefabs.Bird);
            //     var birdComponent = bird.GetComponent<Bird>();

            //     bird.transform.position = collider1.transform.position;
            //     birdComponent.Direction = Util.RandomDirection;
            // }

            Destroy(collider1.transform.root.gameObject);
            Destroy(collider2.transform.root.gameObject);
            _audioSource.PlayOneShot(Resources.Audio.Explosion1);

            return;
        }

        if (collider1.CompareTag(Global.TAG_PLAYER) && collider2.CompareTag(Global.TAG_HURT_PLAYER))
        {
            collider1.transform.root.GetComponent<Health>().Damage(25.0F);
            _audioSource.PlayOneShot(Resources.Audio.PlayerHurt);
            _funCamera.Shake();
        }
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

        _audioSource = GetComponent<AudioSource>();
        _dialog = GameObject.Find("Canvas/Dialog")?.GetComponent<TMP_Text>();
        _dialogPortrait = GameObject.Find("Canvas/Portrait")?.GetComponent<Image>();
        _enemySpawner = GameObject.Find("EnemySpawner")?.GetComponent<EnemySpawner>();
        _funCamera = GameObject.Find("Camera")?.GetComponent<FunCamera>();
        Player = FindObjectOfType<Player>();

        StartCoroutine(Sequence1());
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
        StartCoroutine(_enemySpawner.SpawnWaiter());
        yield return SetDialog("FAIRIES! Dispose of her at once!", 5.0F, Resources.Sprites.LaStakePortrait);
    }
}
