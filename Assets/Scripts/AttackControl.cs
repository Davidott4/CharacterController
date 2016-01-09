using UnityEngine;
using System.Collections;

public class AttackControl : MonoBehaviour {

	Animator animator;
	Rigidbody rigidBody;
	PlayerControl plControl;
	Stats plStats;

	bool attackInput;
	bool blockInput;
	public bool currentlyAttacking;
	bool blocking;
	bool decreaseStamina;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		plControl = GetComponent<PlayerControl>();
		plStats = GetComponent<Stats>();
	}
	
	void FixedUpdate () 
	{
		UpdateInput();
		HandleAttacks();
	}

	void UpdateInput()
	{
		this.attackInput = plControl.attackInput;
		this.blockInput = plControl.blockInput;
	}

	void HandleAttacks()
	{
		if (currentlyAttacking)
		{
			//animator.applyRootMotion = true;
		}
		else
		{
			//animator.applyRootMotion = false;
		}

		if (attackInput && !blocking && !currentlyAttacking)
		{
			plControl.canMove = false;
			currentlyAttacking = true;

			if(!decreaseStamina)
			{
				plStats.stamina -= plStats.attackStaminaCost;
			}

			StartCoroutine("InitiateAttack");
		}
		else 
		{
			decreaseStamina = false;
		}
	}
		

	IEnumerator InitiateAttack()
	{
		yield return new WaitForEndOfFrame();
		animator.SetBool("Attack", true);
		int randomValue = Random.Range(0,4);
		animator.SetInteger("AttackType", randomValue);
	}

}
