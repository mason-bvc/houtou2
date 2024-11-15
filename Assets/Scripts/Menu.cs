using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private TMP_Text[] _labels;
    private int _activeLabel;

    // [SerializeField]
    // private GameObject _gamePrefab;

    protected void OnEnable()
    {
        _labels = transform.Find("MenuRoot").GetComponentsInChildren<TMP_Text>();
    }

    protected void Update()
    {
        _labels[_activeLabel].fontStyle = FontStyles.Normal;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _activeLabel += 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _activeLabel -= 1;
        }

        _activeLabel = Math.Clamp(_activeLabel, 0, _labels.Length - 1);
        _labels[_activeLabel].fontStyle = FontStyles.Italic;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            var labelName = _labels[_activeLabel].gameObject.name;

            // TODO: disgusting
            if (labelName == "NewGame")
            {
                // TODO: hack
                // Destroy(GameObject.Find("King(Clone)"));
                // Instantiate(_gamePrefab);

                SceneManager.LoadScene("Main");
            }
            else if (labelName == "Quit")
            {
                Application.Quit();
            }
        }
    }
}
