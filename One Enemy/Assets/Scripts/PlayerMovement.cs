using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;
    public bool CanTurn = true;

    public float FullSpeed = 3f;

    private Vector2 walkInput = Vector2.zero;
    private Vector2 targetAim = Vector2.up;

    [SerializeField]
    private GameObject gunAnchor;
    
    private CharacterController controller;

    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CanMove) controller.Move(new Vector3(walkInput.x, 0, walkInput.y) * FullSpeed * Time.fixedDeltaTime);
        if(CanTurn && targetAim != Vector2.zero) gunAnchor.transform.localRotation = Quaternion.LookRotation(new Vector3(targetAim.x, 0, targetAim.y), Vector3.up);
    }


    public void OnAim(CallbackContext context)
    {
        if (CanTurn)
        {
            if(context.control.parent.name == "Mouse")
            {
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
            }
            //Debug.Log(context.control.parent.name);
            //if (context.control.path)
            
        }
    }

    public void OnMove(CallbackContext context)
    {
        walkInput = context.ReadValue<Vector2>();
        walkInput.Normalize();
    }
     
}
