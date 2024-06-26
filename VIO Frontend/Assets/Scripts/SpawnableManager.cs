using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;
    [SerializeField]
    Button newButton;
    [SerializeField]
    Button moveButton;
    [SerializeField]
    Button scaleUpButton;
    [SerializeField]
    Button scaleDownButton;
    [SerializeField]
    Button deleteButton;
    [SerializeField]
    Material inactiveMaterial;
    [SerializeField]
    Material activeMaterial;

    Camera arCam;
    GameObject spawnedObject;
    int state;

    ColorBlock normalCb, selectedCb;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();

        state = 0;
        normalCb = newButton.colors;
        selectedCb = normalCb;
        selectedCb.normalColor = Color.yellow;
        selectedCb.selectedColor = Color.yellow;
        newButton.colors = selectedCb;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0 || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable" && state == 1)
                    {
                        if (spawnedObject != null)
                        {
                            //spawnedObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
                        }
                        spawnedObject = hit.collider.gameObject;
                        //spawnedObject.GetComponent<MeshRenderer>().material = activeMaterial;
                        deleteButton.gameObject.SetActive(true);
                        scaleUpButton.gameObject.SetActive(true);
                        scaleDownButton.gameObject.SetActive(true);
                    }
                    else if (state == 0)
                    {
                        if (spawnedObject != null)
                        {
                            //spawnedObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
                        }
                        SpawnPrefab(m_Hits[0].pose.position);
                    }
                    else
                    {
                        if (spawnedObject != null)
                        {
                            //spawnedObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
                        }
                        spawnedObject = null;
                        deleteButton.gameObject.SetActive(false);
                        scaleUpButton.gameObject.SetActive(false);
                        scaleDownButton.gameObject.SetActive(false);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = m_Hits[0].pose.position;
            }
        }
    }

    void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
        //spawnedObject.GetComponent<MeshRenderer>().material = activeMaterial;
        deleteButton.gameObject.SetActive(true);
        scaleUpButton.gameObject.SetActive(true);
        scaleDownButton.gameObject.SetActive(true);
    }

    public void UpdateState(int newState)
    {
        state = newState;
        // Create new object mode
        if (state == 0)
        {
            newButton.colors = selectedCb;
            moveButton.colors = normalCb;
        }
        // Move objects mode
        else if (state == 1)
        {
            newButton.colors = normalCb;
            moveButton.colors = selectedCb;
        }
    }

    public void ScaleSelection(float deltaScale)
    {
        if (spawnedObject != null)
        {
            if(spawnedObject.transform.localScale.x + deltaScale >= 0.1)
            {
                spawnedObject.transform.localScale += new Vector3(deltaScale, deltaScale, deltaScale);
            }
        }
    }

    public void DeleteSelection()
    {
        if(spawnedObject != null)
        {
            Destroy(spawnedObject);
            deleteButton.gameObject.SetActive(false);
            scaleUpButton.gameObject.SetActive(false);
            scaleDownButton.gameObject.SetActive(false);
        }
    }
}
