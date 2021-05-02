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
    [SerializeField]
    private Indicator indicator;
    [SerializeField]
    private HurtableObject health;

    private const float retargetTime = 0.5f;
    private float retargetCurrentTime = retargetTime;

    private Status status = Status.Disabled;
    private int animationId = -1;
    private float deployTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider>();
    }

    void Update()
    {
        if (status == Status.Disabled) return;
        if (status != Status.Firing) weapon.HaltFire();
        switch (status)
        {
            case Status.Deployed:
                if(retargetCurrentTime >= retargetTime)
                {
                    if (CanSeePlayer())
                    {
                        status = Status.Firing;
                    }
                    else retargetCurrentTime -= retargetTime;
                }
                else retargetCurrentTime += Time.deltaTime;
                break;
            case Status.Firing:
                weapon.transform.LookAt(PlayerController.Instance.Position(), Vector3.up);
                weapon.Fire();
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 source = weapon.transform.position;
        Vector3 delta = (PlayerController.Instance.Position() - source).normalized;
        return Physics.Raycast(source, delta, out RaycastHit hit) && hit.collider.CompareTag("Player");
    }

    public void Hide(bool disableAfter = false)
    {
        if (IsHidden() || status == Status.Receding) return;
        LeanTween.cancel(animationId);
        status = Status.Receding;
        animationId = LeanTween.moveLocalY(weaponArm, 0f, CalcLowerTime()).setOnComplete(() => SetHidden(disableAfter)).id;
    }

    public void Deploy()
    {
        if (health.IsAlive() is false || IsActive() || status == Status.Deploying) return;
        LeanTween.cancel(animationId);
        status = Status.Deploying;
        hitbox.enabled = true;
        animationId = LeanTween.moveLocalY(weaponArm, DeployHeight, CalcRiseTime()).setOnComplete(() => SetDeployed()).id;
    }

    private void SetDeployed()
    {
        status = Status.Deployed;
        indicator.Flash(Indicator.Type.Red, CalculateHealthPulseTime());
    }

    public void HealthModified(HurtableObject health)
    {
        if (IsActive())
        {
            indicator.ChangeFlashTime(CalculateHealthPulseTime());
        }
    }

    public void Kill()
    {
        Hide(true);
    }

    private float CalculateHealthPulseTime()
    {
        return Mathf.Pow(0.5f, 4 - Mathf.Ceil(health.CurrentHealth / (float)health.MaxHealth * 4f)) * 2f;
    }

    private void SetHidden(bool disable = false)
    {
        hitbox.enabled = false;
        if (disable)
        {
            status = Status.Disabled;
            indicator.TurnOff();
        }
        else
        {
            status = Status.Hidden;
            indicator.Flash(Indicator.Type.Green, 2f);
        }
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