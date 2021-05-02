using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollector : MonoBehaviour
{
    public static BulletCollector Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void ClearChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
