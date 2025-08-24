using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager instance;

    [Header("Game Level")]
    [SerializeField] protected int level = 0;
    [SerializeField] protected float secondPerLevel = 30;
    [SerializeField] protected float timer = 0;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    private void FixedUpdate()
    {
        this.timer += Time.fixedDeltaTime;
        this.LevelCalculate();
    }

    protected virtual void LevelCalculate()
    {
        //secondPerLevel = 30;
        //timer = 2;
        
        this.level= (int) Mathf.Floor(this.timer / this.secondPerLevel);
        this.level += 1;
    }

    public virtual int Level()
    {
        return this.level;
    }
}
