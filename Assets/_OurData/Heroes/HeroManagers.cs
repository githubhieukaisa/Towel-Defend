using System;
using UnityEngine;

public class HeroManagers : MonoBehaviour
{
    public static HeroManagers instance { get; private set; }
    public HeroesManager[] heroManagers;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of HeroManagers in the scene. Destroying this one.");
        }
    }
    private void Reset()
    {
        this.LoadHeroManagers();
    }

    private void LoadHeroManagers()
    {
        if (this.heroManagers.Length > 0) return;
        GameObject obj = GameObject.Find("HeroManagers");
        this.heroManagers = obj.GetComponentsInChildren<HeroesManager>();
        Debug.Log(transform.name + ": LoadHeroComponents");
    }
}
