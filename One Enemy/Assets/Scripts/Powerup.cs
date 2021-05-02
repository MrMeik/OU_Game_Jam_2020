using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y - .5f, 2f).setEaseInOutQuad().setLoopPingPong();
        LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 1.8f).setEaseInOutQuad().setLoopPingPong();
    }
}
