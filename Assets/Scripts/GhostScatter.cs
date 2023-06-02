using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehavior
{
    private void OnDisable() {
        this.ghost.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Node node = other.GetComponent<Node>();

        // This as in scatter
        if (node != null && this.enabled && !this.ghost.frightened.enabled) {
            int index = Random.Range(0, node.availableDirections.Count);

            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -this.ghost.movement.direction) {
                // Change it to the next available direction so does not reverse back in previous position
                index++;

                if (index >= node.availableDirections.Count) {
                    index = 0;
                }
            }

            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
