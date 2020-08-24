using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float moveSpeed = 2f;
    int waypointIndex = 0;

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position; // position of enemy is the current waypointIndex in the waypoints list - i.e. go to zero
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position; // where we want to move to
            var movementThisFrame = moveSpeed * Time.deltaTime; // how fast we want to move
            transform.position = Vector2.MoveTowards //Vector2.MoveTowards takes 3 parameters, current position, target position, speed of movement
                (transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

