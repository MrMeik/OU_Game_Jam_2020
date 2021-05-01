using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Shield shield;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private ProjectileLauncher weapon;

    private bool hasShield = true;


    public void OnShieldButton(CallbackContext context)
    {
        if (hasShield)
        {
            if (context.started)
            {
                shield.EngageShield();
                movement.CanMove = false;
            }
            else if (context.performed is false)
            {
                shield.DisengageShield();
                movement.CanMove = true;
            }
        }
    }

    public void Shoot(bool state)
    {
        if (state) weapon.Fire();
        else weapon.HaltFire();
    }

    public void OnShoot(CallbackContext context)
    {
        if (movement.CanTurn)
        {
            if (context.started) movement.Firing = true;
            else if (context.performed != true) movement.Firing = false;
        }
        else movement.Firing = false;
    }
}
