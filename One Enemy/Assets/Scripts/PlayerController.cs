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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
