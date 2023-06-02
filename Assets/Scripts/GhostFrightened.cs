using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;
    // Ghost eaten or not
    public bool eaten { get; private set; }

    // Powerpellet concern with preference of overriding (Ghost already enabled)
    public override void Enable(float duration)
    {
        base.Enable(duration);

        // When frightened
        this.body.enabled = false;
        this.eyes.enabled = false;
        this.blue.enabled = true;
        // Initially
        this.white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    // No longer frightened
    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void Flash() {
        if (!this.eaten) {
            this.blue.enabled = false;
            this.white.enabled = true;
            // Reset animation to beginning (timing might not lineup)
            this.white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void Eaten() {
        this.eaten = true;

        Vector3 position = this.ghost.home.inside.position;
        // Make sure we are not changing our z
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        // Home behavior that makes sure ghosts do not leave home until frightened state is done
        this.ghost.home.Enable(this.duration);

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void OnEnable() {
        // Ghost frightened
        this.ghost.movement.speedMultiplier = .5f;
        // Cannot eat ghost while it is frightened
        this.eaten = false;
    }

    private void OnDisable() {
        this.ghost.movement.speedMultiplier = 1f;
        this.eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            if (this.enabled) {
                Eaten();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Node node = other.GetComponent<Node>();

        // This as in frightened
        if (node != null && this.enabled) {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach(Vector2 availableDirection in node.availableDirections) {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                // Avoid magnitude, sqrt too costly runtime
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance) {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}
