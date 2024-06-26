using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    Button pathButton;
    [SerializeField]
    Button scaleUpButton;
    [SerializeField]
    Button scaleDownButton;
    [SerializeField]
    Button moveUpButton;
    [SerializeField]
    Button moveDownButton;
    [SerializeField]
    Button bindButton;
    [SerializeField]
    Button unbindButton;
    [SerializeField]
    Button hideButton;
    [SerializeField]
    Button showButton;
    [SerializeField]
    GameObject parentCamera;
    [SerializeField]
    GameObject map;
    [SerializeField]
    GameObject staticMap;

    private List<GameObject> waypoints;
    [SerializeField]
    Transform startTransform;
    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    GameObject waypointPrefab;
    [SerializeField]
    GameObject targetPrefab;
    [SerializeField]
    Transform robotTransform;


    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(null);
        waypoints = new();
    }

    private void Update()
    {
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        float scale = map.transform.localScale.x / staticMap.transform.localScale.x;
        robotTransform.position = RotatePoint((targetTransform.position * scale) + offset, this.transform.rotation) + this.transform.position - staticMap.transform.position;
        robotTransform.rotation = targetTransform.rotation;
    }

    public void BindMap()
    {
        this.transform.SetParent(parentCamera.transform);

        bindButton.gameObject.SetActive(false);
        unbindButton.gameObject.SetActive(true);
    }

    public void UnbindMap()
    {
        this.transform.SetParent(null);

        unbindButton.gameObject.SetActive(false);
        bindButton.gameObject.SetActive(true);
    }

    public void ChangeMapView(bool hide)
    {
        //Automatically unbind on hide to avoid unintentional shifting
        if (hide)
        {
            UnbindMap();
        }

        map.GetComponent<MeshRenderer>().enabled = !hide;

        bindButton.gameObject.SetActive(!hide);
        hideButton.gameObject.SetActive(!hide);
        scaleUpButton.gameObject.SetActive(!hide);
        scaleDownButton.gameObject.SetActive(!hide);
        moveUpButton.gameObject.SetActive(!hide);
        moveDownButton.gameObject.SetActive(!hide);

        showButton.gameObject.SetActive(hide);
        pathButton.gameObject.SetActive(hide);
    }

    public void MoveMap(float yDelta)
    {
        Vector3 offset = new Vector3(0f, yDelta, 0f);
        this.transform.position += offset;
    }

    public void ScaleMap(float scaleDelta)
    {
        if(this.transform.localScale.x + scaleDelta >= 1)
        {
            Vector3 scale = new Vector3(scaleDelta, scaleDelta, scaleDelta);
            map.transform.localScale += scale;
        }
    }

    private Vector3 RotatePoint(Vector3 point, Quaternion angles)
    {
      return angles * point; // rotate it
    }

    public void CreateWaypoints()
    {
        foreach (GameObject waypoint in waypoints)
        {
            Destroy(waypoint);
        }
        waypoints.Clear();

        if(startTransform == null || targetTransform == null)
        {
            return;
        }

        NavMesh.SamplePosition(startTransform.position, out NavMeshHit hitA, 10f, NavMesh.AllAreas);
        NavMesh.SamplePosition(targetTransform.position, out NavMeshHit hitB, 10f, NavMesh.AllAreas);
        Debug.Log(hitA);
        Debug.Log(hitB);

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(hitA.position, hitB.position, NavMesh.AllAreas, path))
        {
            Vector3 offset = new Vector3(0f, 0.5f, 0f);
            float scale = map.transform.localScale.x / staticMap.transform.localScale.x;
            //foreach (Vector3 corner in path.corners)
            //{
            //    waypoints.Add(
            //        Instantiate(waypointPrefab, RotatePoint((corner * scale) + offset, this.transform.rotation) + this.transform.position, Quaternion.identity, this.transform)
            //    );
            //}
            for (int i=0; i < path.corners.Length; i++)
            {
                Vector3 currentPos = path.corners[i];

                if (i != path.corners.Length - 1)
                {
                    Vector3 nextPos = path.corners[i + 1];
                    waypoints.Add(
                        Instantiate(
                            waypointPrefab,
                            RotatePoint((currentPos * scale) + offset, this.transform.rotation) + this.transform.position - staticMap.transform.position,
                            Quaternion.Euler(0f, Mathf.Atan2(nextPos.x - currentPos.x, nextPos.z - currentPos.z)*(180f/Mathf.PI), 0f),
                            this.transform
                        )
                    );
                }
                else
                {
                    waypoints.Add(
                        Instantiate(
                            targetPrefab,
                            RotatePoint((currentPos * scale) + offset, this.transform.rotation) + this.transform.position - staticMap.transform.position,
                            Quaternion.identity,
                            this.transform
                        )
                    );
                }
            }
        }
    }
}
