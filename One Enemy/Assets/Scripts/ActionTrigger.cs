using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTrigger : MonoBehaviour
{
    public bool OnlyOnce = false;
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public bool WriteDebug = false;


    private bool entered = false;
    private bool exited = false;

    public void OnTriggerEnter(Collider other)
    {
        if (OnlyOnce && !entered)
        {
            if (other.CompareTag("Player"))
            {
                entered = true;
                OnPlayerEnter?.Invoke();
                if (WriteDebug) Debug.Log("Player Enter Once", this);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke();
                if (WriteDebug) Debug.Log("Player Enter", this);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (OnlyOnce && !exited)
        {
            if (other.CompareTag("Player"))
            {
                exited = true;
                OnPlayerExit?.Invoke();
                if (WriteDebug) Debug.Log("Player Exit Once", this);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke();
                if (WriteDebug) Debug.Log("Player Exit", this);
            }
        }
    }
}
