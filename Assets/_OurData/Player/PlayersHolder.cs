using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHolder : MonoBehaviour
{
    public static PlayersHolder instance { get; private set; }
    public List<HeroCtrl> heroCtrls;
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

    public virtual HeroCtrl GetHero(string name) {         
        foreach (HeroCtrl heroCtrl in this.heroCtrls)
        {
            if (heroCtrl.character.name == name)
            {
                return heroCtrl;
            }
        }
        Debug.LogError("There is no Hero with the name: " + name);
        return null;
    }
}
