using UnityEngine;
using UnityEngine.Events;

public class ARTapToPlaceTest : MonoBehaviour
{
    [SerializeField] GameObject avatarGameObject;
    [SerializeField] Camera arCam;

    private Vector3 newPosition= Vector3.zero;
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

                if(Physics.Raycast(ray, out hitAnything, Mathf.Infinity))
                {
                    if(hitAnything.transform.gameObject.CompareTag("Floor"))
                    {
                        Debug.Log("<<<<>>>>" + hitAnything.transform.gameObject.tag);
                        // for the 1st time enable the avatar and place it at the touch position
                        if (avatarGameObject.activeSelf == false)
                        {
                            avatarGameObject.SetActive(true);
                            avatarGameObject.transform.rotation = hitAnything.transform.rotation;
                            avatarGameObject.transform.position = hitAnything.point;
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
        