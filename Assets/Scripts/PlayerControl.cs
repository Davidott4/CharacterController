using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour 
{
	Rigidbody rigidBody;
	Animator animator;
	CapsuleCollider capsuleCollider;
	Transform camera;

	[HideInInspector]
	public Transform cameraHolder;

	[SerializeField] float lockSpeed = .5f;
	[SerializeField] float normSpeed = .8f;
	[SerializeField] float turnMultiplier = 2.0f;
	[SerializeField] float maxLockDistance = 20.0f;
	float speed;

	[SerializeField] float turnSpeed = 5;

	Vector3 directionPos;
	Vector3 storeDir;

	[HideInInspector]
	public float horizontal;

	[HideInInspector]
	public float vertical;

	[HideInInspector] 
	public bool rollInput;
	[HideInInspector] 
	public bool attackInput;
	[HideInInspector] 
	public bool blockInput;
	public bool currentlyBlocking;

	public bool lockTarget;
	int currentTarget;
	bool changeTarget;

	float targetTurnAmount;
	float currentTurnAmount;
	public bool canMove;
	public bool canBlock;
	public List<Transform> Enemies = new List<Transform>();

	public Transform cameraTarget;
	public float cameraTargetSpeed = 5;
	Vector3 targetPos;




	// Use this for initialization
	void Start () {
		//setup references
		rigidBody = GetComponent<Rigidbody>();
		camera = Camera.main.transform;
		//camera = Camera.main.transform;
		//cameraHolder = camera.parent.parent;
		capsuleCollider = GetComponent<CapsuleCollider>();
		SetupAnimator();

		GetComponent<PlayerAnimations>().enabled = true;


	}


	
	void FixedUpdate()
	{
		HandleInput();
		HandleCameraTarget();
		HandleBlockInput();

		if (canMove)
		{
			if (!lockTarget)
			{
				speed = normSpeed;
				HandleMovementNormal();
			}
			else
			{
				speed = lockSpeed;

				if (Enemies.Count >0)
				{
					HandleMovementLockOn();
					HandleRotationOnLock();
				}
				else
				{
					lockTarget = false; 
				}
			}
		}


	}

	void HandleBlockInput()
	{
		if (canBlock && blockInput)
		{
			animator.SetBool("Block", true);
			currentlyBlocking = true;
		}
		else
		{
			animator.SetBool("Block", false);
			currentlyBlocking = false;
		}

	}

	void HandleInput()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		rollInput = Input.GetButton("Jump");
		attackInput = Input.GetButton("Attack");
		blockInput = Input.GetButton("Block");

		storeDir = camera.right;
		//storeDir = cameraHolder.right; //get the direction from the camera holder instead of the camera
		//so we avoid differences in velocity based on where the camera is looking

		ChangeTargetsLogic();
	}

	void HandleMovementNormal()
	{
		canMove = animator.GetBool("CanMove");

		Vector3 dirForward = storeDir * horizontal;
		Vector3 dirSideways = camera.forward * vertical;
		//Vector3 dirSideways = cameraHolder.forward * vertical;

		//if (canMove) // Normalize the direction before applying so we dont move faster wile holding both axeses
			//rigidBody.AddForce((dirForward + dirSideways).normalized * speed / Time.deltaTime);

		directionPos = transform.position + (storeDir * horizontal) + (camera.forward * vertical);

		//Find direction from that position
		Vector3 dir = directionPos - transform.position;
		dir.y = 0;

		//find teh angle
		float angle = Vector3.Angle(transform.forward, dir);

		float animValue = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

		animValue = Mathf.Clamp01(animValue);

		//but split the valuse of the animator to work with new controller
		animator.SetFloat("Forward", animValue);
		animator.SetBool("LockOn", false);

		if (horizontal != 0 || vertical !=0)
		{
			if (angle != 0 && canMove)
			{
				rigidBody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
			}
		}

		// handle rotation only when we move, not attack or rolling
		cameraTarget.transform.forward = transform.forward;

	}

	void HandleCameraTarget()
	{
		if (!lockTarget)
		{
			targetPos = transform.position;

		}
		else
		{
			if(Enemies.Count >0)
			{
				Vector3 direction = Enemies[currentTarget].position - transform.position;
				direction.y = 0;

				float distance = Vector3.Distance(transform.position, Enemies[currentTarget].position);

				targetPos = direction.normalized * distance /4;

				targetPos += transform.position;

				cameraTarget.transform.forward = direction;

				if(distance >maxLockDistance)
				{
					lockTarget = false;
				}
			}

		}

		cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos, Time.deltaTime * cameraTargetSpeed);

		
	}

	void HandleMovementLockOn()
	{
		//Transform camHolder = camera.parent.parent;
		//Make sure it has corect scale

		Vector3 camForward = Vector3.Scale(camera.forward, new Vector3(1,0,1)).normalized;
		Vector3 camRight = Vector3.Scale(camera.right, new Vector3(1,0,1)).normalized;
		//Vector3 camForward = Vector3.Scale(cameraHolder.forward, new Vector3(1,0,1)).normalized;
		//Vector3 camRight = Vector3.Scale(cameraHolder.right, new Vector3(1,0,1)).normalized;

		Vector3 move = vertical * camForward + horizontal * camRight;

		Vector3 moveForward = camForward * vertical;
		Vector3 moveSideways = camRight * horizontal;

		// apply force
		//rigidBody.AddForce((moveForward + moveSideways).normalized * speed/ Time.deltaTime);

		ConvertMoveInputAndPassItToAnimator(move);
		
	}

	void ConvertMoveInputAndPassItToAnimator(Vector3 moveInput)
	{
		Vector3 localMove = transform.InverseTransformDirection(moveInput);
		float turnAmount = localMove.x;
		float forwardAmount = localMove.z;

		if (turnAmount !=0)
			turnAmount *=turnMultiplier;

		animator.SetBool("LockOn", true);
		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetFloat("Sideways", turnAmount, 0.1f, Time.deltaTime);

	}

	void HandleRotationOnLock()
	{
		Vector3 lookPos = Enemies[currentTarget].position;

		Vector3 lookDir = lookPos - transform.position;

		//kil the y value
		lookDir.y=0;

		//then rotate to look at the target
		Quaternion rot = Quaternion.LookRotation(lookDir);
		rigidBody.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
	}






	void ChangeTargetsLogic()
	{
		if (Input.GetButtonUp("Lock Target"))
		{
			lockTarget = !lockTarget;
		}

		if (Input.GetKeyUp(KeyCode.Q))
		{
			if (currentTarget < Enemies.Count -1)
			{
				currentTarget++;
			}
			else{
				currentTarget =0;
			}
		}
	}


	void SetupAnimator()
	{
		animator = GetComponent<Animator>();

		foreach (var childAnimator in GetComponentsInChildren<Animator>())
		{
			if (childAnimator != animator)
			{
				animator.avatar = childAnimator.avatar;
				Destroy(childAnimator);
				break; //break if/when we find it
			}
		}
	}

}
