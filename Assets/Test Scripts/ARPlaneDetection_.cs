using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneDetection_ : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private GameObject floorPrefab;

    private GameObject floor;

    private bool isFloorPlaced;

    private List<ARPlane> foundPlanes = new List<ARPlane>();

    private void Start()
    {
        arPlaneManager.planesChanged += PlanesChanged;
    }

    private void PlanesChanged(ARPlanesChangedEventArgs obj)
    {
        if (obj != null && obj.added.Count > 0)
        {
            foundPlanes.AddRange(obj.added);
        }

        foreach (ARPlane plane in foundPlanes)
        {
            if (plane.extents.x * plane.extents.y >= 0.5f && !isFloorPlaced)
            {
                isFloorPlaced = true;
                floor = Instantiate(floorPrefab);
                floor.transform.position = plane.center;
                floor.transform.up = plane.normal;
                DisablePlanes();
            }
        }
    }

    private void DisablePlanes()
    {
        arPlaneManager.enabled = false;
        foreach (var plane in arPlaneManager.trackables)
        {
                plane.gameObject.SetActive(false);
        }
        this.enabled = false;
    }

    private void OnDisable()
    {
        arPlaneManager.planesChanged -= PlanesChanged;
    }

}
