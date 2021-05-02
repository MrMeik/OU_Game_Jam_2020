using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MovingObject
{
    public bool CanMove = true;
    public bool CanTurn = true;

    public float FullSpeed = 3f;

    private Vector2 walkInput = Vector2.zero;
    private Vector2 targetAim = Vector2.up;

    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private GameObject gunAnchor;

    public bool Firing = false;

    private CharacterController controller;

    private PlayerController playerController;

    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove)
        {
            velocity = new Vector3(walkInput.x, 0, walkInput.y) * FullSpeed;
            controller.Move(velocity * Time.fixedDeltaTime);
        }
        else velocity = Vector3.zero;
        if (CanTurn && targetAim != Vector2.zero) 
            gunAnchor.transform.localRotation = Quaternion.LookRotation(new Vector3(targetAim.x, 0, targetAim.y), Vector3.up);

        playerController.Shoot(Firing);
    }


    public void OnAim(CallbackContext context)
    {
        if (CanTurn)
        {
            if (context.control.parent.name == "Mouse")
            {
                if (cam == null) return;
                var playerPos = cam.WorldToScreenPoint(transform.position);
                var mousePos = context.ReadValue<Vector2>();
                targetAim = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
                targetAim.Normalize();
                //Debug.Log(cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane)));
            }
            else
            {
                targetAim = context.ReadValue<Vector2>();
                targetAim.Normalize();
                if (targetAim.magnitude > 0.1f) Firing = true;
                else Firing = false;
            }
            //Debug.Log(context.control.parent.name);
            //if (context.control.path)
        }
        else
        {
            Firing = false;
        }
    }

    public void OnMove(CallbackContext context)
    {
        walkInput = context.ReadValue<Vector2>();
        walkInput.Normalize();
    }

    public override Vector3 GetVelocity() => velocity;
}

public abstract class MovingObject : MonoBehaviour
{
    public abstract Vector3 GetVelocity();
}
