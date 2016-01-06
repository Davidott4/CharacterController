﻿using UnityEngine;
using System.Collections;

public class EnableMovement : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<PlayerControl>().canMove = true;
	}
}
