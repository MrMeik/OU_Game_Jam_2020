using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool CanShield = false;
    public float DeployHeight;
    public float ReactionTime = 0.1f;

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

    [SerializeField]
    private Status status = Status.Disabled;

    private int shieldingId = -1;

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
            case Status.Hidden:
                if (retargetCurrentTime >= retargetTime)
                {
                    if (CanSeePlayer())
                    {
                        Deploy();
                    }
                    retargetCurrentTime -= retargetTime;
                }
                else retargetCurrentTime += Time.deltaTime;
                break;
            case Status.Deployed:
                if(retargetCurrentTime >= retargetTime)
                {
                    if (CanSeePlayer())
                    {
                        status = Status.Firing;
                    }
                    retargetCurrentTime -= retargetTime;
                }
                else retargetCurrentTime += Time.deltaTime;
                break;
            case Status.Firing:
                weapon.transform.LookAt(PlayerController.Instance.Position(), Vector3.up);
                weapon.Fire();
                if (retargetCurrentTime >= retargetTime)
                {
                    if (!CanSeePlayer())
                    {
                        status = Status.Deployed;
                    }
                    retargetCurrentTime -= retargetTime;
                }
                else retargetCurrentTime += Time.deltaTime;
                break;
            case Status.Shielding:
                if(shieldingId == -1) //shield.IsOn() && !shield.IsEngaging())
                {
                    shieldingId = LeanTween.delayedCall(1f, () => StopShielding()).id;
                }
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 source = weapon.transform.position + Vector3.up * .1f;
        Vector3 delta = (PlayerController.Instance.Position() - source).normalized;
        bool a = Physics.Raycast(source, delta, out RaycastHit hit);
        return a && hit.collider.CompareTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive() || !CanShield || shield.IsEngaging()) return;
        if (status == Status.Shielding)
        {
            if (!shield.IsOn()) return;
            if (other.CompareTag("Projectile"))
            {
                var proj = other.GetComponent<Projectile>();
                if (proj.IsIgnoringCollision(hitbox)) return;
                if (ShouldBeScared(proj) is false) return;
            }
            LeanTween.cancel(shieldingId);
        }
        if (other.CompareTag("Projectile"))
        {
            var proj = other.GetComponent<Projectile>();
            if (proj.IsIgnoringCollision(hitbox)) return;
            if (ShouldBeScared(proj) is false) return; 
            status = Status.Shielding;
            indicator.Flash(Indicator.Type.Purple);
            shieldingId = LeanTween.delayedCall(ReactionTime, () => { shield.EngageShield(); shieldingId = -1; }).id;
        }
        else if (other.CompareTag("Player"))
        {
            status = Status.Shielding;
            indicator.Flash(Indicator.Type.Purple);
            shieldingId = LeanTween.delayedCall(ReactionTime, () => { shield.EngageShield(); shieldingId = -1; }).id;
        }
    }

    private int hitlightningId = -1;
    private int hitTotal = 0;

    private void OnParticleCollision(GameObject other)
    {
        hitTotal++;
        if(hitlightningId == -1)
        {
            hitlightningId = LeanTween.delayedCall(0.1f, () =>
            {
                hitlightningId = -1;
                if(hitTotal > 0)
                {
                    health.ModifyHealth(-hitTotal * 20);
                }
                hitTotal = 0;

            }).id;
        }
    }

    private bool ShouldBeScared(Projectile proj)
    {
        var projPos = proj.transform.position;
        var projDelta = proj.MovementDirection;
        var toMe = (transform.position - projPos).normalized;
        return Vector3.Angle(projDelta, toMe) < 30f;
    }

    public void StopShielding()
    {
        LeanTween.cancel(shieldingId);
        if(status == Status.Shielding)
        {
            indicator.Flash(Indicator.Type.Red);
            status = Status.Firing;
        }
        shield.DisengageShield();
    }

    public void EnableLogic()
    {
        if (health.IsAlive() is false) return;
        status = Status.Deployed;
        Hide();
    }

    public void DisableLogic()
    {
        Hide(true);
        indicator.TurnOff();
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
        LeanTween.cancel(shieldingId);
        shield.DisengageShield();
        weapon.HaltFire();
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