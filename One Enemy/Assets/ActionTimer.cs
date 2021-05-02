using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTimer : MonoBehaviour
{
    public UnityEvent TimerComplete;

    public int StartTime = 30;

    [SerializeField]
    private float currentTime;
    private bool count = false;
    public bool completed = false;

    public void Start()
    {
        currentTime = StartTime;
    }

    public void Update()
    {
        if (count && !completed)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                TimerComplete?.Invoke();
                completed = true;
            }
        }
    }

    public void StartTimer()
    {
        if (count) return;
        count = true;
    }
    
    public void HaltAndReset()
    {
        count = false;
        currentTime = StartTime;
    }
}
