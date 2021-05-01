using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int FlightSpeed = 10;
    public Vector3 MovementDirection;

    private int MaxLifeTime = 5;
    private int killId = -1;
    private Collider lastHit = null;
    private Rigidbody projRB;

    void Start()
    {
        projRB = GetComponent<Rigidbody>();
        ResetKillTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        projRB.MovePosition(transform.position + MovementDirection * FlightSpeed * Time.deltaTime);
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
        MovementDirection = Vector3.Reflect(MovementDirection, -normal);
    }
}
