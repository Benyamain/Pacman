using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    // Boxcasting
    public LayerMask obstacleLayer;
    public Vector2 direction { get; private set; }
    // Move up as soon as there is an opening and not being blocked by an obstacle
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    private void Start() {
        ResetState();
    }

    public void ResetState() {
        this.speedMultiplier = 1f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        // Ghosts can pass through the walls without collision
        this.rigidbody.isKinematic = false;
        // Script is enabled
        this.enabled = true;
    }

    // Physics in fixed update so game is consistent since fps varies across different devices
    private void FixedUpdate() {
        Vector2 position = this.rigidbody.position;
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;

        this.rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction) {

    }

    public bool Occupied(Vector2 direction) {
        RaycastHit2D hit = Physics2D.BoxCast
    }
}
