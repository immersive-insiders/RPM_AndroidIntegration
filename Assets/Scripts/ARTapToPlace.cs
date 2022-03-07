using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARTapToPlace : MonoBehaviour
{
    //To use the ImportedAvatar property and get the Avatar GameObject
    [SerializeField] private AvatarImporter avatarImporter;
    [SerializeField] Camera arCam;

    private Vector3 newPosition = Vector3.zero;
    public Vector3 TouchPosition { get => newPosition; }

    public UnityEvent OnNewTouch;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCam.ScreenPointToRay(touch.position);
                RaycastHit hitAnything;
                if (Physics.Raycast(ray, out hitAnything, Mathf.Infinity))
                {
                    if (hitAnything.transform.gameObject.CompareTag("Floor"))
                    {
                        Debug.Log("<<<<>>>>" + hitAnything.transform.gameObject.tag);

                        if (avatarImporter.ImportedAvatar.activeSelf == false)
                        {
                            avatarImporter.ImportedAvatar.SetActive(true);
                            avatarImporter.ImportedAvatar.transform.rotation = hitAnything.transform.rotation;
                            avatarImporter.ImportedAvatar.transform.position = hitAnything.point;
                        }
                        else
                        {
                            Debug.Log("<<<< New position reg>>>>");

                            newPosition = hitAnything.point;
                            OnNewTouch.Invoke();
                        }
                    }
                }
            }

        }
    }
}