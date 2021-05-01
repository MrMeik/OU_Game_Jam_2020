using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int FlightSpeed = 10;
    public int MaxLifeTime = 5;

    private Vector3 movementDirection;
    private int killId = -1;
    private Collider lastHit = null;
    private Rigidbody projRB;

    void Start()
    {
        movementDirection = transform.forward;
        projRB = GetComponent<Rigidbody>();
        ResetKillTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        projRB.MovePosition(transform.position + movementDirection * FlightSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collider = collision.collider;
        if (collider.CompareTag("Shield"))
        {
            if (collider == lastHit) return;
            var shield = collider.GetComponent<Shield>();
            if (shield.IsOn())
            {
                if (shield.IsEngaging())
                {
                    lastHit = collider;
                    ReflectAcross(collision.GetContact(0).normal);
                }
                else BlowUp();
            }
        }
        else if (collider.CompareTag("Player"))
        {
            //Hurt player
            Debug.Log("Ouch");
            BlowUp();
        }
        else BlowUp();
    }

    private void ResetKillTimer()
    {
        LeanTween.cancel(killId);
        killId = LeanTween.delayedCall(MaxLifeTime, () => BlowUp()).id;
    }

    private void BlowUp()
    {
        //Play Destroy animation
        LeanTween.cancel(killId);
        Destroy(gameObject);
    }

    private void ReflectAcross(Vector3 normal)
    {
        ResetKillTimer();
        movementDirection = Vector3.Reflect(movementDirection, -normal);
    }
}
