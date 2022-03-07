using UnityEngine;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private AvatarImporter avatarImporter;
    
    [SerializeField] private RuntimeAnimatorController avatarController;

    private Animator animator;

    private bool isAnimatorAssigned = false;

    private void Update()
    {
        // make sure that the avatar is loaded before getting the Animator component
        // and also make sure it gets assigned to the variable just once.
        if (avatarImporter.ImportedAvatar != null && !isAnimatorAssigned)
        {
            animator = avatarImporter.ImportedAvatar.transform.GetComponent<Animator>();
            animator.runtimeAnimatorController = avatarController;
            isAnimatorAssigned = true;
        }
    }

    public void StartWalkAnimation()
    {
        this.animator.SetBool("isMoving", true);
    }

    public void StopWalkAnimation()
    {
        this.animator.SetBool("isMoving", false);
    }

    public void StartTurnAnimation(float angle)
    {
        if (angle > 0)
        {
            this.animator.SetBool("isTurningRight", true);
        }
        else
        {
            this.animator.SetBool("isTurningLeft", true);
        }
    }

    public void StopTurnAnimation()
    {
        this.animator.SetBool("isTurningLeft", false);
        this.animator.SetBool("isTurningRight", false);
    }

    public bool IsTurnAnimatorPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("RightTurn") || animator.GetCurrentAnimatorStateInfo(0).IsName("LeftTurn");
    }

    public bool IsMoveAnimatorPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Walking");
    }
}
