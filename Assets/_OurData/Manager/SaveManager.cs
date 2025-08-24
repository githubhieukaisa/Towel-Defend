using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private const string SAVE_1 = "save_1";
    private const string SAVE_2 = "save_2";
    private const string SAVE_3 = "save_3";

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    private void Start()
    {
        this.LoadSaveGame();        
    }

    void OnApplicationQuit()
    {
        this.SaveGame();
    }

    protected virtual string GetSaveName()
    {
        return SaveManager.SAVE_1;
    }
    protected virtual string GetSaveName(string dataName)
    {
        return SaveManager.SAVE_1 + "_" + dataName;
    }
    public virtual void LoadSaveGame()
    {
        //string stringSave = SaveSystem.GetString(this.GetSaveName());
        //Debug.Log("LoadSaveGame: " + stringSave);

        string jsonString= SaveSystem.GetString(this.GetSaveName("ScoreManager"));
        Debug.Log("jsonString: " + jsonString);
        ScoreManager.instance.FromJson(jsonString);
    }

    public virtual void SaveGame()
    {
        //string stringSave = "bbbbbbbbbbbbb";
        //SaveSystem.SetString(this.GetSaveName(), stringSave);
        //Debug.Log("SaveGame: " + stringSave);

        string jsonString = JsonUtility.ToJson(ScoreManager.instance);
        SaveSystem.SetString(this.GetSaveName("ScoreManager"), jsonString);
        Debug.Log("SaveGame: " + jsonString);
    }
}
