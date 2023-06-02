using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    // Just in case
    private void OnEnable() {
        StopAllCoroutines();
    }

    private void OnDisable() {
        // Check for active self to prevent error when object is destroyed
        if (this.gameObject.activeInHierarchy) {
            StartCoroutine(ExitTransition());
        }
    }

    // Bounce up and down while in home
    private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log(true);
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
  
    }

    private IEnumerator ExitTransition() {
        this.ghost.movement.SetDirection(Vector2.up, true);
        // Turn off collisions and physics while in transition
        this.ghost.movement.rigidbody.isKinematic = true;
        this.ghost.movement.enabled = false;

        Vector3 position = this.transform.position;
        float duration = .5f;
        float elapsed = 0f;

        // Animation
        while (elapsed < duration) {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            // Waits one frame
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration) {
            ghost.SetPosition(Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            // Waits one frame
            yield return null;
        }

        this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        // Turn off collisions and physics while in transition
        this.ghost.movement.rigidbody.isKinematic = false;
        this.ghost.movement.enabled = true;
    }
}
