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

    private Type currentType = Type.Green;
    private float currentTime = 0f;
    private bool currentIsOn = false;

    public float CurrentFlashTime => currentTime * 2f;

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

    private void StopAnimation()
    {
        if (IsFlashing())
        {
            LeanTween.cancel(id);
            id = -1;
        }
    }

    private bool IsFlashing() => id != -1;

    public void Flash(Type type) => Flash(type, (currentTime == 0) ? 1 : currentTime * 2f);

    public void Flash(Type type, float pulseTime)
    {
        StopAnimation();
        currentType = type;
        currentTime = pulseTime / 2f;
        currentIsOn = true;
        id = LoopTween(currentType, currentTime, true);
    }

    public void ChangeFlashTime(float newFlashTime)
    {
        if (IsFlashing())
        {
            if (newFlashTime == CurrentFlashTime) return;
            currentTime = newFlashTime / 2f;

            var descr = LeanTween.descr(id);
            float timeLeft = descr.time - descr.passed;
            timeLeft = Mathf.Clamp(timeLeft, 0f, currentTime);
            StopAnimation();
            id = LeanTween.delayedCall(timeLeft, () => id = LoopTween(currentType, currentTime, !currentIsOn)).id;
        }
        else
        {
            currentTime = newFlashTime / 2f;
        }
    }

    private int LoopTween(Type type, float callTime, bool isOn)
    {
        SetLightMaterial(type, isOn);
        return LeanTween.delayedCall(callTime, () => id = LoopTween(type, callTime, !isOn)).id;
    }

    private void SetLightMaterial(Type type, bool isOn = true)
    {
        this.currentIsOn = isOn;
        if (isOn is false) rend.sharedMaterial = off;
        else
        {
            currentType = type;
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


