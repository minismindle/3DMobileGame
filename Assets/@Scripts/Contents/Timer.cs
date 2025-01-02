using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    int _elapsedtime = 0;

    public int Time
    {
        get { return _elapsedtime; }
        set { _elapsedtime = value; }   
    }

    Coroutine _timer;

    public void StartTimer()
    {
        if (_timer != null)
            StopCoroutine(UpdateTimer());
        _timer = StartCoroutine(UpdateTimer());
    }
    public void StopTimer()
    {
        if( _timer != null )    
            StopCoroutine(UpdateTimer());   
        _timer = null;
    }
    public void ClearTimer()
    {
        Time = 0;
    }
    IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _elapsedtime++;
        }
    }
    public string SetTimeText(int time)
    {
        string timeText = "";
        if (time < 10)
        {
            timeText = $"00:0{time}";
        }
        else if (time < 60)
        {
            timeText = $"00:{time}";
        }
        else if (time < 600)
        {
            if (time % 60 < 10)
                timeText = $"0{time / 60}:0{time % 60}";
            else
                timeText = $"0{time / 60}:{time % 60}";
        }
        return timeText;
    }

}
