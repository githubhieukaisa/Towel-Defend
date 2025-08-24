using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    [Header("Hero")]
    public List<HeroCtrl> herros=new List<HeroCtrl>();

    private void Reset()
    {
        this.LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        this.LoadHeroes();
    }

    protected virtual void LoadHeroes()
    {
        if (this.herros.Count > 0) return;
        foreach (HeroCtrl heroCtrl in transform.GetComponentsInChildren<HeroCtrl>()) 
        {
            this.herros.Add(heroCtrl);
        }
        Debug.Log(transform.name + ": LoadHeros");
    }

    public virtual GameObject GetHero()
    {
        GameObject heroObj=this.herros[0].gameObject;
        GameObject hero = Instantiate(heroObj, new Vector3(0, 0, 0), transform.rotation);
        return hero;
    }
}
