using System;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    private TMP_Text _text;

    protected void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    protected void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);

        _text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}