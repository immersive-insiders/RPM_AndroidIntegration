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

    private void OnEnable()
    {
        arTapToPlace.OnNewTouch.AddListener(OnTouch);
        avatarImporter.OnAvatarStored.AddListener(GetAvatarTransform);
        
    }

    private void OnTouch()
    {
        avatarAnimationController.StopWalkAnimation();
        avatarAnimationController.StopTurnAnimation();
        touchPos = arTapToPlace.TouchPosition;

        //using the helper method to calculate the angle
        float angle = CalculateAngle(touchPos, avatarTransform.position, avatarTransform);

        // if there is no change in angle then avatar has to just move forward
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

    private void GetAvatarTransform()
    {
        avatarTransform = avatarImporter.ImportedAvatar.transform;
    }

    void Update()
    {

        //if (avatarImporter.ImportedAvatar != null && avatarTransform == null)
        //{
        //    avatarTransform = avatarImporter.ImportedAvatar.transform;
        //}

        if (isTurnning)
        {
            // If the dot product of two normalized vectors is:
            //      1 then, the vectors are in the same direction.
            //      -1 then, the vectors are in opposite direction
            //      0 then, the vectors are 90deg to each other

            if (Vector3.Dot(avatarTransform.forward, Vector3.Normalize(touchPos - avatarTransform.position)) > 0.9f)
            {
                avatarAnimationController.StopTurnAnimation();

                //once the avatar has turned, the moving animation can start
                isMoving = true;
                isTurnning = false;
            }
        }

        if (isMoving && !avatarAnimationController.IsTurnAnimatorPlaying())
        {
            if (Vector3.Distance(avatarTransform.position, touchPos) > 0.1f)
            {
                avatarTransform.LookAt(touchPos); // to account of that 0.1 rotation that's missed

                // to make sure the animation is played just once in the entire update cycle
                if (!avatarAnimationController.IsMoveAnimatorPlaying())
                    avatarAnimationController.StartWalkAnimation();


                // it's normalized so that the velocity remains constant. 
                // 0.3 is a magic number to match the speed with the animation speed
                avatarTransform.position += (touchPos - avatarTransform.position).normalized * 0.3f * Time.deltaTime;

                //  the below code works as well
                //  avatarTransform.position = Vector3.MoveTowards(avatarTransform.position, touchPos,  0.3f * Time.deltaTime);
            }
            else
            {
                avatarAnimationController.StopWalkAnimation();
                isMoving = false;
            }
        }

    }

    private float CalculateAngle(Vector3 targetPos, Vector3 currentPos, Transform avatarTransform)
    {
        float angle = Vector3.SignedAngle(currentPos - targetPos, avatarTransform.forward, avatarTransform.up);
        return angle;
    }

    private void OnDisable()
    {
        arTapToPlace.OnNewTouch.RemoveListener(OnTouch);
        avatarImporter.OnAvatarStored.AddListener(GetAvatarTransform);

    }
}