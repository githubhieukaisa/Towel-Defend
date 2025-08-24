using UnityEngine;

public class PlayerInteractable : HieuMonoBehavior
{
    public virtual void Interact()
    {
        Debug.Log($"{transform.name} interacted with.");
    }
}
