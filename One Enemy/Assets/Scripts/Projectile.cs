using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int FlightSpeed = 10;
    public int MaxLifeTime = 5;
    public int Damage = 10;

    private Vector3 movementDirection;
    private int killId = -1;
    private Rigidbody projRB;

    private List<Collider> ignoredColliders = new List<Collider>();
    private Collider ownCollider;

    void Start()
    {
        movementDirection = transform.forward;
        projRB = GetComponent<Rigidbody>();
        ownCollider = GetComponent<Collider>();
        foreach (Collider col in ignoredColliders) Physics.IgnoreCollision(ownCollider, col);
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
            var shield = collider.GetComponent<Shield>();
            if (shield.IsOn())
            {
                if (shield.IsEngaging())
                {
                    ClearCollisions();
                    IgnoreCollision(collider);
                    ReflectAcross(collision.GetContact(0).normal);
                    FlightSpeed += (int)(FlightSpeed * 0.25f);
                }
                else BlowUp();
            }
        }
        else if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
        {
            collider.GetComponent<HurtableObject>().ModifyHealth(-Damage);
            BlowUp();
        }
        else BlowUp();
    }

    public void IgnoreCollision(Collider collider)
    {
        ignoredColliders.Add(collider);
        if(ownCollider != null) Physics.IgnoreCollision(ownCollider, collider);
    }

    public void ClearCollisions()
    {
        foreach (var collider in ignoredColliders)
            Physics.IgnoreCollision(ownCollider, collider, false);
        ignoredColliders.Clear();
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
