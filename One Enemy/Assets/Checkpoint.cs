using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public List<HurtableObject> Enemies = new List<HurtableObject>();

    public UnityEvent OnResetAdditional;

    internal void ResetZone()
    {
        foreach(HurtableObject enemy in Enemies)
        {
            if (enemy.IsAlive()) enemy.ResetHealth();
        }
        OnResetAdditional?.Invoke();
    }
}
