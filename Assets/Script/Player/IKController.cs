using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour {

    Animator animator;						// 玩家动画控制器
    public bool isActive = true;			// 是否启用IK
    public Transform leftHandObject = null;	// 玩家左手IK标记物
    public Transform rightHandObject = null;	// 玩家右手IK标记物

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex) {
        if (animator) {
            if (isActive) {
                if (leftHandObject != null) {
                    //设置玩家左手的IK，使玩家左手的位置尽量靠近leftHandObj对象，左手的朝向与leftHandObj相同
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObject.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObject.rotation);
                }
                if (rightHandObject != null) {
                    //设置玩家右手的IK，使玩家右手的位置尽量靠近RightHandObj对象，右手的朝向与RightHandObj相同
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObject.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObject.rotation);
                }
            }
            else {
                //取消玩家角色的IK，使玩家角色的头部，驱赶，左右手的位置和朝向受正向动力学控制
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }
}
