using UnityEngine;
using System.Collections;

public class EnableMovementStateExit : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<PlayerControl>().canMove = true;
	}
}
