using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool CanShield = false;
    public float DeployHeight;

    private Collider hitbox;
    [SerializeField]
    private Shield shield;
    [SerializeField]
    private GameObject weaponArm;
    [SerializeField]
    private ProjectileLauncher weapon;

    private Status status = Status.Disabled;
    private int animationId = -1;
    private float deployTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider>();
    }

    public void Hide()
    {
        if (IsHidden() || status == Status.Receding) return;
        LeanTween.cancel(animationId);
        status = Status.Receding;
        animationId = LeanTween.moveLocalY(weaponArm, 0f, CalcLowerTime()).setOnComplete(() => SetHidden()).id;
    }

    public void Deploy()
    {
        if (IsActive() || status == Status.Deploying) return;
        LeanTween.cancel(animationId);
        status = Status.Deploying;
        hitbox.enabled = true;
        animationId = LeanTween.moveLocalY(weaponArm, DeployHeight, CalcRiseTime()).setOnComplete(() => SetDeployed()).id;
    }

    private void SetDeployed()
    {
        status = Status.Deployed;
    }

    private void SetHidden()
    {
        hitbox.enabled = false;
    }

    private float CalcRiseTime() => ((DeployHeight - weaponArm.transform.localPosition.y) / DeployHeight) * deployTime;
    private float CalcLowerTime() => (weaponArm.transform.localPosition.y / DeployHeight) * deployTime;

    private bool IsHidden() =>
        status == Status.Disabled || status == Status.Hidden;

    private bool IsActive() => 
        status == Status.Deployed || status == Status.Shielding || status == Status.Firing;
    

    private enum Status
    {
        Disabled,
        Hidden,
        Receding,
        Deploying,
        Deployed,
        Shielding,
        Firing
    }
}