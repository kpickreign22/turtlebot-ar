using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private List<GameObject> waypoints;
    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    GameObject waypointPrefab;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new();
        waypoints = new();
    }

    public void CreateWaypoints()
    {
        foreach(GameObject waypoint in waypoints)
        {
            Destroy(waypoint);
        }
        waypoints.Clear();

        if (agent.CalculatePath(targetTransform.position, path))
        {
            Vector3 offset = new Vector3(0f, 0.5f, 0f);
            foreach (Vector3 corner in path.corners)
            {
                waypoints.Add(
                    Instantiate(waypointPrefab, corner + offset, Quaternion.identity, this.transform.parent)
                );
            }
        }
    }
}
