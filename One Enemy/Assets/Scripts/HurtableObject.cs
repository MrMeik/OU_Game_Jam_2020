using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtableObject : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;
    public UnityEvent OnDead;
    public UnityEvent<HurtableObject> HealthModified;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public void ModifyHealth(int delta)
    {
        if (delta == 0) return;
        int newHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
        if (newHealth == currentHealth) return;
        currentHealth = newHealth;
        if (currentHealth == 0) OnDead?.Invoke();
        else HealthModified?.Invoke(this);
    }

    public void ResetHealth()
    {
        int delta = maxHealth - currentHealth;
        ModifyHealth(delta);
    }

    public bool IsAlive() => currentHealth > 0;
}
