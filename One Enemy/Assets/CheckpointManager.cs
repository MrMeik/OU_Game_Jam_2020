using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint CurrentCheckpoint;

    public GameObject Player;

    [HideInInspector]
    public HurtableObject PlayerHealth;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = Player.GetComponent<HurtableObject>();
    }

    public void SetNewCheckpoint(Checkpoint checkpoint) => CurrentCheckpoint = checkpoint;

    public void ResetToCheckpoint()
    {
        ResetPlayer();
        BulletCollector.Instance.ClearChildren();
        CurrentCheckpoint.ResetZone();
    }

    public void ResetPlayer()
    {
        Player.transform.position = CurrentCheckpoint.transform.position;
        PlayerHealth.ResetHealth();
    }
}
