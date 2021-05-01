using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilsController : MonoBehaviour
{
    [SerializeField]
    private GameObject Coil1;
    [SerializeField]
    private GameObject Coil2;

    [SerializeField]
    private ParticleSystem CoilPS1;
    [SerializeField]
    private ParticleSystem CoilPS2;

    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        CreateCenteredBoxCollider();
        if(Coil1.transform.localPosition.x == Coil2.transform.localPosition.x) SetupXAligned();
        else if(Coil1.transform.localPosition.z == Coil2.transform.localPosition.z) SetupZAligned();
        else throw new System.Exception("Coils not aligned");
    }

    private void SetupXAligned()
    {
        float distance = Vector3.Distance(Coil1.transform.localPosition, Coil2.transform.localPosition);
        boxCollider.size = new Vector3(0.25f, 2, distance);
    }

    private void SetupZAligned()
    {
        float distance = Vector3.Distance(Coil1.transform.localPosition, Coil2.transform.localPosition);
        boxCollider.size = new Vector3(distance, 2, 0.25f);
    }

    private void CreateCenteredBoxCollider()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = (Coil1.transform.localPosition + Coil2.transform.localPosition) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
