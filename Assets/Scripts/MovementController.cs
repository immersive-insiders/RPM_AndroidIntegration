using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private AvatarImporter avatarImporter;
    [SerializeField] private ARTapToPlace arTapToPlace;
    [SerializeField] private AvatarAnimationController avatarAnimationController;

    private Vector3 touchPos;
    private Transform avatarTransform = null;

    private bool isTurnning;
    private bool isMoving;

    private void Start()
    {
        arTapToPlace.OnNewTouch.AddListener(OnTouch);
    }

    private void OnTouch()
    {
        Debug.Log("<< ON touch has been called>>>>");
        avatarAnimationController.StopWalkAnimation();
        avatarAnimationController.StopTurnAnimation();
        touchPos = arTapToPlace.TouchPosition;

        //using the helper method to calculate the angle
        float angle = CalculateAngle(touchPos, avatarTransform.position, avatarTransform);
        
        // if there is no chnage in angle then avatar has to just move foward
        if (angle == 0)
        {
            isTurnning = false;
            isMoving = true;
        }

        // else, the avatar has to turn in the direction of touch before moving.
        else
        {
            isTurnning = true;
            // Setting moving to false because if the user touches the screen to a new point while the avatar is moving
            // then the moving animation has to be stopper
            isMoving = false;
            avatarAnimationController.StartTurnAnimation(angle);
        }
    }

    void Update()
    {
        
        if (avatarImporter.ImportedAvatar != null && avatarTransform==null)
        {
            avatarTransform = avatarImporter.ImportedAvatar.transform;
        }

        if (isTurnning)
        {
            Debug.Log("<< Update turn>>>>");

            // If the dot porduct of two normalized vector is 1 then, the vectors are in same direction
            // If the dot porduct of two normalized vector is -1 then, the vectors are in opposite direction
            // If the dot porduct of two normalized vector is 0 then, the vectors are 90deg to each other
            // So if the dot product is >0.9 it's really close to the same direction.
            if (Vector3.Dot(avatarTransform.forward, Vector3.ProjectOnPlane(touchPos - avatarTransform.position, avatarTransform.up).normalized) > 0.9f)
            {
                avatarAnimationController.StopTurnAnimation();
                //once the avatar has turned, the moving animation can start
                isMoving = true;
                isTurnning = false;
            }
        }

        if (isMoving && !avatarAnimationController.IsTurnAnimatorPlaying())
        {
            Debug.Log("<< Update move>>>>");

            if (Vector3.Distance(avatarTransform.position, touchPos) > 0.1f)
            {
                avatarTransform.LookAt(touchPos); // to account of that 0.1 rotation that's missed
                if (!avatarAnimationController.IsMoveAnimatorPlaying())
                {
                    Debug.Log("<<<<<<< Rot has completed, Starting Move animation >>>>>>> ");
                    avatarAnimationController.StartWalkAnimation();
                }

                // it's normalized so that the velocity remains constant
                avatarTransform.position += (touchPos - avatarTransform.position).normalized * 0.3f * Time.deltaTime;
                // 0.3 is a magic number to match the speed with the animation speed
            }
            else
            {
                avatarAnimationController.StopWalkAnimation();
                isMoving = false;
            }
        }

    }

    private void OnDisable()
    {
        arTapToPlace.OnNewTouch.RemoveListener(OnTouch);

    }
    private float CalculateAngle(Vector3 targetPos, Vector3 currentPos, Transform avatarTransform)
    {
        float angle = Vector3.SignedAngle(currentPos - targetPos, avatarTransform.forward, avatarTransform.up);
        return angle;
    }
}
