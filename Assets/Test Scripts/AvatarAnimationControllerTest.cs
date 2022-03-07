using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationControllerTest : MonoBehaviour
{
    [SerializeField] private GameObject avatarGameObject;
    private Animator animator;

    private void Start()
    {
        animator = avatarGameObject.GetComponent<Animator>();
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
        if( angle >0)
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
        return animator.GetCurrentAnimatorStateInfo(0).IsName("RightTurn") || animator.GetCurrentAnimatorStateInfo(0).IsName("LeftTurn") ;     
    }

    public bool IsMoveAnimatorPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Walking");
    }
}

