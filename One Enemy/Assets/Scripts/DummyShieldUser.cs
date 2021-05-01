using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyShieldUser : MonoBehaviour
{
    public Shield shield;

    // Start is called before the first frame update
    void Start()
    {
        shield.EngageShield();
    }
}
