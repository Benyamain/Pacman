using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
// Make it required in order to use this class, it must be inherited
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    private void Awake() {
        this.ghost = GetComponent<Ghost>();
        // Just in case
        this.enabled = false;
    }

    public void Enable() {
        // Powerpellets
        Enable(this.duration);
    }

    public virtual void Enable(float duration) {
        this.enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable() {
        this.enabled = false;

        // Just in case
        CancelInvoke();
    }
}
