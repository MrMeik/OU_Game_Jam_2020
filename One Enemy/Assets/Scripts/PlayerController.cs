using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField]
    private Shield shield;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private ProjectileLauncher weapon;


    [SerializeField]
    private bool hasShield = false;

    private float shieldInputCounter = 0f;

    private bool turnShieldOn = false;
    private bool turnShieldOff = false;

    private void Start()
    {
        Instance = this;
    }

    public void OnQuit(CallbackContext context)
    {
        Application.Quit();
    }

    public Vector3 Position()
    {
        Vector3 pos = transform.position;
        pos.y = 1f;
        return pos;
    }

    public void Update()
    {
        if(shieldInputCounter <= 0f)
        {
            if (turnShieldOn)
            {
                shield.EngageShield();
                movement.CanMove = false;
                movement.CanTurn = false;
                turnShieldOn = false;
                shieldInputCounter = 1f;
            }
        }
        else
        {
            shieldInputCounter -= Time.deltaTime;
        }

        if (turnShieldOff)
        {
            shield.DisengageShield();
            movement.CanMove = true;
            movement.CanTurn = true;
            turnShieldOff = false;
        }
    }

    public void OnShieldButton(CallbackContext context)
    {
        if (hasShield)
        {
            if (context.started)
            {
                turnShieldOn = true;
                //shield.EngageShield();
                //movement.CanMove = false;
            }
            else if (context.performed is false)
            {
                turnShieldOff = true;
                //shield.DisengageShield();
                //movement.CanMove = true;
            }
        }
    }

    public void EnableSuperMega()
    {
        weapon.SuperMegaFire = true;
    }

    public void EnableShield()
    {
        hasShield = true;
    }

    public void EnableMega()
    {
        weapon.MegaFire = true;
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
