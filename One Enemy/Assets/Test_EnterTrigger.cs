using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnterTrigger : MonoBehaviour
{
    public Turret target;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) target.Deploy();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) target.Hide();
    }
}
