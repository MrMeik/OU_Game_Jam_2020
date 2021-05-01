using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField]
    private Material off;
    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;
    [SerializeField]
    private Material purple;

    private int id = -1;

    private Renderer rend;

    public void TurnOff()
    {
        StopAnimation();
        SetLightMaterial(Type.Green, false);
    }

    public void SetLight(Type type)
    {
        StopAnimation();
        SetLightMaterial(type);
    }

    private void StopAnimation() => LeanTween.cancel(id);

    public void Flash(Type type, float pulseTime)
    {
        id = LoopTween(type, pulseTime / 2f, true);
    }

    private int LoopTween(Type type, float callTime, bool isOn)
    {
        SetLightMaterial(type, isOn);
        return LeanTween.delayedCall(callTime, () => id = LoopTween(type, callTime, !isOn)).id;
    }

    private void SetLightMaterial(Type type, bool isOn = true)
    {
        if (isOn is false) rend.sharedMaterial = off;
        else
        {
            rend.sharedMaterial = Lookup(type);
        }
    }

    private Material Lookup(Type type) => type switch
    {
        Type.Red => red,
        Type.Purple => purple,
        _ => green,
    };

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public enum Type
    {
        Green,
        Red,
        Purple
    }
}


