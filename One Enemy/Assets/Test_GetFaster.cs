using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GetFaster : MonoBehaviour
{
    public Indicator ind;

    public float flashTime = 5f;

    private bool first = true;

    void Update()
    {
        if (first)
        {
            ind.Flash(Indicator.Type.Purple, flashTime);
            first = false;
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        flashTime *= 2f;
        ind.ChangeFlashTime(flashTime);
    }
}
