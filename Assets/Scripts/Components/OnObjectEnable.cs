using UnityEngine;
using UnityEngine.Events;

public class OnObjectEnable : MonoBehaviour
{
    // Public config
    public UnityEvent onEnable;

    public void OnEnable()
    {
        if (this.onEnable != null)
        {
            this.onEnable.Invoke();
        }
    }
}
