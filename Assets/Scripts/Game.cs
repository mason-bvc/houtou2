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
    public AudioSource AudioSource;

    //
    // dialog stuff
    //
    private TMP_Text _dialog;
    private string _dialogText = "";
    private float _dialogTextVisibleRatio;
    private float _dialogT;
    private Image _dialogPortrait;

    //
    // hud stuff
    //
    private TMP_Text _wave;
    private Image[] _hearts;

    //
    // resources
    //
    public static AssetBundle AssetBundle { get; protected set; }

    //
    // game state
    //
    private AudioSource _music;
    private Background _background;
    private EnemySpawner _birdSpawner;
    private EnemySpawner _fairySpawner;
    private FunCamera _funCamera;
    private GameObject _dieCanvas;
    private TMP_Text _dieCanvasText;
    private GameObject _menu;
    private ScreenFlash _screenFlash;
    private GameObject _laStake;
    private TMP_Text _scoreText;
    public static Game Instance;
    public GameObject Playism { get; protected set; }
    public Player Player { get; protected set; }
    public int Score;

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

    private void OnPlayerHealthUpdated()
    {
        for (var i = _hearts.Length - 1; i >= 0; i--)
        {
            _hearts[i].gameObject.SetActive(true);

            if (Player.Health.GetHealth() / Player.Health.InitialHealth * _hearts.Length <= i)
            {
                _hearts[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnPlayerHealed()
    {
        OnPlayerHealthUpdated();
    }

    private void OnPlayerDamaged()
    {
        OnPlayerHealthUpdated();

        IEnumerator DoDeathSequence()
        {
            yield return new WaitForSeconds(1.0F);
            _dieCanvas.SetActive(true);
            AudioSource.PlayOneShot(Resources.Audio.GameOver);
        }

        if (Player.Health.GetHealth() <= 0.0F)
        {
            Destroy(Playism);
            _music.Stop();
            Instance.StartCoroutine(DoDeathSequence());
            return;
        }

        _funCamera.Shake();
    }

    protected void Awake()
    {
        AssetBundle = AssetLoader.AssetBundle;
    }

    protected void Start()
    {
        Instance = this;
        AudioSource = GetComponent<AudioSource>();
        Playism = GameObject.Find("Playism");
        _background = GameObject.Find("Playism/Background")?.GetComponent<Background>();
        _dialog = GameObject.Find("Playism/Canvas/Dialog")?.GetComponent<TMP_Text>();
        _dialogPortrait = GameObject.Find("Playism/Canvas/Portrait")?.GetComponent<Image>();
        _birdSpawner = GameObject.Find("Playism/BirdSpawner")?.GetComponent<EnemySpawner>();
        _fairySpawner = GameObject.Find("Playism/FairySpawner")?.GetComponent<EnemySpawner>();
        _funCamera = GameObject.Find("Camera")?.GetComponent<FunCamera>();
        _hearts = GameObject.Find("Playism/Canvas/Hearts").GetComponentsInChildren<Image>();
        _wave = GameObject.Find("Playism/Canvas/WaveText")?.GetComponent<TMP_Text>();
        _dieCanvas = GameObject.Find("DieCanvas");
        _dieCanvasText = _dieCanvas.transform.Find("Text").GetComponent<TMP_Text>();
        _dieCanvas.SetActive(false);
        _menu = GameObject.Find("Menu");
        _menu.SetActive(false);
        _music = GameObject.Find("Music")?.GetComponent<AudioSource>();
        _screenFlash = GameObject.Find("ScreenFlashCanvas/ScreenFlash")?.GetComponent<ScreenFlash>();
        _scoreText = GameObject.Find("Playism/Canvas/ScoreText")?.GetComponent<TMP_Text>();
        Player = FindObjectOfType<Player>();

        IEnumerator PostPlayerInit()
        {
            yield return new WaitForEndOfFrame();
            Player.Health.Damaged.AddListener(OnPlayerDamaged);
            Player.Health.Healed.AddListener(OnPlayerHealed);
        }

        StartCoroutine(PostPlayerInit());
        // TODO: uncomment
        StartCoroutine(Sequence1());
        StartCoroutine(Sequence2());
        StartCoroutine(Sequence3());
        StartCoroutine(Sequence4());
        StartCoroutine(Sequence5());
        StartCoroutine(Sequence6());
        StartCoroutine(Sequence7());
        StartCoroutine(Sequence8());
        StartCoroutine(Sequence9());
    }

    protected void Update()
    {
        _dialogT += Time.deltaTime;
        _dialogT = Mathf.Clamp(_dialogT, 0, 1);
        _dialogTextVisibleRatio = Mathf.Lerp(0, 1, _dialogT);
        _dialog.text = _dialogText[..(int)(_dialogText.Length * _dialogTextVisibleRatio)];
        _scoreText.text = string.Format("{0:D7}", Score);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var val = !_menu.activeSelf;

            _menu.SetActive(val);
            Player.IsStopped = _menu.activeSelf;
            // Playism.SetActive(!val);

            // _music.UnPause();

            // if (_menu.activeSelf)
            // {
            //     _music.Pause();
            // }
        }
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

    private IEnumerator Sequence7()
    {
        // TODO: uncomment
        yield return new WaitForSeconds(60.0F * 4.0F + 15.0F);
        // yield return new WaitForSeconds(1);

        foreach (var enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.gameObject.SetActive(false);
        }

        foreach (var enemy in FindObjectsOfType<Mob>())
        {
            Destroy(enemy.gameObject);
        }

        _screenFlash.Color = Color.white;

        yield return new WaitForSeconds(1);

        Player.IsStopped = true;
        _screenFlash.Color = Color.black;

        yield return new WaitForSeconds(8);

        _laStake = Instantiate(Resources.Prefabs.LaStake, Playism.transform);

        var laStakeComponent = _laStake.GetComponent<LaStake>();

        _laStake.transform.position = new Vector3(0, 6, 0);
        Player.transform.position = new Vector3(0, -2, 0);
        Player.AimTransform.up = Vector3.up;

        _screenFlash.Color = new Color(0, 0, 0, 0);
        _background.GetComponent<Renderer>().material = Resources.Materials.Background2;
        yield return new WaitForSeconds(1);
        // TODO: uncomment
        yield return SetDialog("Took out your goons LaStake, now what?", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("Undo this now or I'll doom the both of us.", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("Ever the hasty one...", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("The world you're from-", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("-is illusion as much as this one.", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("Spending eternity in this vacuum is no different-", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("-than your meaningless life at \"home\".", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("You're wrong. Unlike you, I have friends-", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("-and people who care about me.", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("My ambitions lie in something greater-", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("-than just power.", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("More meaningless human drivel.", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("Now if you'll allow me...", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("to fully embody the lesson of...", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("\"If you want something done right-\"", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("\"-DO IT YOURSELF!\"", 3.0F, Resources.Sprites.LaStakePortrait);
        Player.IsStopped = false;
        laStakeComponent.StartBeingMean();
    }

    private IEnumerator Sequence8()
    {
        // TODO: uncomment
        yield return new WaitForSeconds(60.0F * 5.0F + 15.0F);
        // yield return new WaitForSeconds(15);

        _fairySpawner.gameObject.SetActive(true);
        _birdSpawner.gameObject.SetActive(true);
        StartCoroutine(_fairySpawner.BeginSpawn());
        StartCoroutine(_birdSpawner.BeginSpawn());
        _fairySpawner.CurrentMaxEnemies = 3;
        _birdSpawner.CurrentMaxEnemies = 10;
    }

    private IEnumerator Sequence9()
    {
        // TODO: uncomment
        yield return new WaitForSeconds(60.0F * 6.0F + 16.0F);
        // yield return new WaitForSeconds(30.0F);

        _laStake.GetComponent<LaStake>().StopBeingMean();
        Player.IsStopped = true;

        foreach (var enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.gameObject.SetActive(false);
        }

        foreach (var bullet in FindObjectsOfType<LaStakeBullet>())
        {
            Destroy(bullet.gameObject);
        }

        foreach (var enemy in FindObjectsOfType<Mob>())
        {
            Destroy(enemy.gameObject);
        }

        yield return SetDialog("You look unusually pale, LaStake...", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("Do you need to take ten?", 3.0F, Resources.Sprites.CorrinePortrait);
        yield return SetDialog("I'm *huff* fine *gasp* you...", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("*gasping* insignificant...", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("whelp...", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("*faints*", 3.0F, Resources.Sprites.LaStakePortrait);
        yield return SetDialog("What a piece of work.", 3.0F, Resources.Sprites.CorrinePortrait);

        _screenFlash.Color = new Color(0.0F, 0.0F, 0.0F, 1.0F);

        yield return new WaitForSeconds(5.0F);
        _dieCanvas.SetActive(true);
        _dieCanvasText.text = "Corinne escaped!";
    }
}
