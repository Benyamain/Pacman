using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;
    
    private void OnTriggerEnter2D(Collider2D other) {
        Vector3 position = other.transform.position;
        // Does not mess up draw order by manually inputting for the x and y
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;
        other.transform.position = position;
    }
}
