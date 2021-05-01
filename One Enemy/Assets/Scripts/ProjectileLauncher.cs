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

    private int fireId = -1;

    public void Start()
    {
        Fire();
    }

    public void Fire()
    {
        LeanTween.cancel(fireId);
        Instantiate(projectilePrefab, ejectionPoint.transform.position, this.transform.rotation, transform);
        if(LoopFire) fireId = LeanTween.delayedCall(RefireRate, () => Fire()).id;
    }
}
