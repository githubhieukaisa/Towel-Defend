using UnityEngine;

public class Ground : HieuMonoBehavior
{
    public virtual void ChangeLayer(int layer)
    {
        gameObject.layer = layer;
    }
}
