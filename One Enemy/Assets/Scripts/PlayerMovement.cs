using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public float FullSpeed = 3f;

    private Vector2 walkInput = Vector2.zero;
    private CharacterController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        controller.Move(new Vector3(walkInput.x, 0, walkInput.y) * FullSpeed * Time.fixedDeltaTime);
    }

    public void OnMove(CallbackContext context)
    {
        walkInput = context.ReadValue<Vector2>();
        walkInput.Normalize();
    }
}
