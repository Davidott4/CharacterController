using UnityEngine;
using System.Collections;

public class CloseRolling : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlayerAnimations pl = animator.GetComponent<PlayerAnimations>();

		pl.rolling = false;
		pl.hasDirection = false;

		animator.GetComponent<PlayerControl>().canMove = true;

	}
}

