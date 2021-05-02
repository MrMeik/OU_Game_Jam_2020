using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountTrigger : MonoBehaviour
{
    public UnityEvent OnCountReached;
    public int Target = 1;
    public int Current = 0;
    private bool triggered = false;

    public void Start()
    {
        CheckTrigger();
    }

    public void AddCount()
    {
        Current++;
        CheckTrigger();
    }

    public void CheckTrigger()
    {
        if(triggered is false && Current >= Target)
        {
            OnCountReached?.Invoke();
            triggered = true;
        }
    }
}
