using System;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    private DateTime _startTime;
    private TMP_Text _text;

    protected void Start()
    {
        _text = GetComponent<TMP_Text>();
        _startTime = DateTime.Now;
    }

    protected void Update()
    {
        TimeSpan timeSpan = DateTime.Now - _startTime;

        _text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
