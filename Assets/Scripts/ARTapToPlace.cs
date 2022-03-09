using UnityEngine;
using UnityEngine.Events;


public class ARTapToPlace : MonoBehaviour
{
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
                    StorePosition(hitAnything);
                }
            }

        }
    }

    private void StorePosition(RaycastHit hitAnything)
    {
        if (hitAnything.transform.gameObject.CompareTag("Floor"))
        {
            if (avatarImporter.ImportedAvatar.activeSelf == false)
            {
                avatarImporter.ImportedAvatar.SetActive(true);
                avatarImporter.ImportedAvatar.transform.rotation = hitAnything.transform.rotation;
                avatarImporter.ImportedAvatar.transform.position = hitAnything.point;
            }
            else
            {
                newPosition = hitAnything.point;
                OnNewTouch.Invoke();
            }
        }
    }
}