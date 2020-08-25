using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // Configuration parameters
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;

    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position; // position of enemy is the current waypointIndex in the waypoints list - i.e. go to zero
    }
    private void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig) // method REQUIRES WaveConfig waveConfig when it is called, otherwise it won't run
    {
        this.waveConfig = waveConfig;  // this.waveConfig in this class instance equals the waveConfig value we receive when the method is called
    }
    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position; // where we want to move to
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime; // how fast we want to move
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

