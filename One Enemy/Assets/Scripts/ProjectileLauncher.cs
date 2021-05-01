using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public bool LoopFire = false;
    public float RefireRate = 0.05f;

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject ejectionPoint;
    [SerializeField]
    private Collider[] sourceColliders;
    [SerializeField]
    private MovingObject movementSource;

    private int fireId = -1;

    public void Start()
    {
        Fire();
    }

    public void Fire()
    {
        LeanTween.cancel(fireId);
        
        var newObj = Instantiate(projectilePrefab, ejectionPoint.transform.position, this.transform.rotation, BulletCollector.Instance.transform);
        if(sourceColliders.Length != 0) foreach(var collider in sourceColliders) Physics.IgnoreCollision(newObj.GetComponent<Collider>(), collider);
        
        if(movementSource != null)
        {
            var sourceVelocity = movementSource.GetVelocity();
            float deltaSpeed = Vector3.Dot(transform.forward, sourceVelocity);
            if(deltaSpeed > 0) newObj.GetComponent<Projectile>().FlightSpeed += Mathf.RoundToInt(deltaSpeed);
            //Debug.Log(deltaSpeed);
                //Vector3.Project(sourceVelocity, transform.forward);
        }

        if(LoopFire) fireId = LeanTween.delayedCall(RefireRate, () => Fire()).id;
    }
}
