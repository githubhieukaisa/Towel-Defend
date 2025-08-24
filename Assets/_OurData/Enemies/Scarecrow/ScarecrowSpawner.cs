using Unity.VisualScripting;
using UnityEngine;

public class ScarecrowSpawner : Spawner
{
    public static ScarecrowSpawner instance;

    [Header("Scarecrow")]
    [SerializeField] protected float height = 4.98f;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    protected override void ResetValue()
    {
        this.enemyName = "Scarecrow";
        this.spawnLimit = 1;
    }

    protected override Vector3 SpawnPos()
    {
        float x= Random.Range(-7.0f, 7.0f);
        Vector3 spawnPos= new Vector3(x, this.height, 0);
        return spawnPos;
    }

    public virtual void ScarecrowDead()
    {
        this.spawnDelay += 2;
    }
}
