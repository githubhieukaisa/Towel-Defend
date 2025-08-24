using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score")]
    [SerializeField] protected int gold = 0;
    [SerializeField] protected int kill = 0;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    public virtual int GetGold()
    {
        return this.gold;
    }

    public virtual void Kill()
    {
        this.kill++;
    }

    public virtual void GoldAdd(int count)
    {
        this.gold += count;
    }

    public virtual bool GoldDeduct(int count)
    {
        if (this.gold < count) return false;
        this.gold -= count;
        return true;
    }

    public virtual void FromJson(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString)) return;
        Debug.Log("ScoreManager FromJson: " + jsonString);
        ScoreData data = JsonUtility.FromJson<ScoreData>(jsonString);
        this.gold = data.gold;
        this.kill = data.kill;
    }
}
