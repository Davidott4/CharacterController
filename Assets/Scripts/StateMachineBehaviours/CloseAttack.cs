using UnityEngine;
using System.Collections;

public class CloseAttack : StateMachineBehaviour {

	AttackControl attackControl;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<PlayerControl>().canBlock = false;
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		attackControl = animator.GetComponent<AttackControl>();
		attackControl.currentlyAttacking = false;
		animator.SetBool("Attack",false);

		animator.GetComponent<PlayerControl>().canMove = true;
		animator.GetComponent<PlayerControl>().canBlock = true;
	}
}

