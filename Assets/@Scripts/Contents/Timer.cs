using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    int _elapsedtime = 0;

    public int Time
    {
        get { return _elapsedtime; }
    }

    Coroutine _timer;

    public void StartTimer()
    {
        if (_timer != null)
            StopCoroutine(UpdateTimer());
        _timer = StartCoroutine(UpdateTimer());
    }
    IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _elapsedtime++;
        }
    }

}
