using UnityEngine;

public class HieuMonoBehavior : MonoBehaviour
{
    public bool debug=false;
    private void Awake()
    {
        this.LoadComponents();
    }
    private void Reset()
    {
        this.ResetValue();
        this.LoadComponents();
    }
    protected virtual void ResetValue()
    {
        // This method is intended to be overridden in derived classes
    }
    protected virtual void LoadComponents()
    {
        // This method is intended to be overridden in derived classes
    }

    protected virtual void DebugRaycast(Vector3 start, RaycastHit hit, Vector3 direction)
    {
        if (!this.debug) return;

        if (hit.transform == null)
        {
            Debug.DrawRay(start, direction, Color.red);
            Debug.Log(transform.name + ": No hit");
        }
        else
        {
            Debug.DrawRay(start, direction * hit.distance, Color.green);
            Debug.Log(transform.name + ": Hit " + hit.transform.name);
        }
    }
}
