using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private GameObject shieldVisual;
    [SerializeField]
    private Renderer shieldRenderer;
    [SerializeField]
    private Material initColor;
    [SerializeField] 
    private Material fullColor;

    [SerializeField]
    private float ShieldEngageTime = 0.2f;
    [SerializeField]
    private float FullyChargedTime = .75f;
    [SerializeField]
    private float DecayTime = 1.5f;

    private Vector3 inScale = new Vector3(.7f, 1, .7f);
    private Vector3 outScale = Vector3.one;
    private Vector3 fullScale = Vector3.zero; 
    private int shieldAnimationId = -1;
    private bool fullCycleUsed = false;

    private Stage shieldStage;

    public bool IsEngaging() => shieldStage == Stage.Engaging;
    public bool IsOn() => shieldStage != Stage.Off;

    // Start is called before the first frame update
    void Start()
    {
        TrySetUp();
    }

    public void DisengageShield()
    {
        shieldStage = Stage.Off;
        gameObject.SetActive(false);
        LeanTween.cancel(shieldAnimationId);
    }

    public void EngageShield()
    {
        if (shieldStage != Stage.Off) return;
        gameObject.SetActive(true);
        TrySetUp();
        shieldStage = Stage.Engaging;
        fullCycleUsed = false;
        LeanTween.cancel(shieldAnimationId);
        shieldVisual.transform.localScale = inScale;
        shieldRenderer.sharedMaterial = initColor;

        shieldAnimationId = LeanTween.scale(shieldVisual, fullScale, ShieldEngageTime).setEaseOutElastic().setOnComplete(() => WaitOnFull()).id;
    }

    private void TrySetUp()
    {
        if (fullScale == Vector3.zero)
        {
            fullScale = shieldVisual.transform.localScale;
            shieldRenderer = shieldVisual.GetComponent<Renderer>();
        }
    }

    private void WaitOnFull()
    {
        shieldStage = Stage.Full;
        LeanTween.cancel(shieldAnimationId);
        shieldRenderer.sharedMaterial = fullColor;
        shieldAnimationId = LeanTween.delayedCall(FullyChargedTime, () => BeginShieldDecay()).id;
    }

    private void BeginShieldDecay()
    {
        shieldStage = Stage.Decaying;
        fullCycleUsed = true;
        LeanTween.cancel(shieldAnimationId);
        shieldAnimationId = LeanTween.scale(shieldVisual, outScale, DecayTime).setOnComplete(() => DisengageShield()).id;
    }

    private enum Stage
    {
        Off,
        Engaging,
        Full,
        Decaying
    }
}
