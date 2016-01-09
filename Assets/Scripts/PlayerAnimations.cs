using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {

	PlayerControl playerControl;

	[SerializeField] public float rollSpeed = 0f;

	float horizontal;
	float vertical;
	bool rollInput;

	public bool rolling;
	bool hasRolled;

	Vector3 directionPos;
	Vector3 storeDir;
	//Transform cameraHolder;
	Transform camera;
	Rigidbody rigidBody;
	Animator animator;

	public bool hasDirection;

	Vector3 dirForward;
	Vector3 dirSideways;
	Vector3 dir;

	// Use this for initialization
	void Start () 
	{
		playerControl = GetComponent<PlayerControl>();
		//cameraHolder = playerControl.cameraHolder;
		camera = Camera.main.transform;
		rigidBody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		this.rollInput = playerControl.rollInput;
		this.horizontal = playerControl.horizontal;
		this.vertical = playerControl.vertical;
		storeDir = camera.transform.right;
		//storeDir = cameraHolder.right;



		if (rollInput)
		{
			Debug.Log("roll");
			if(!rolling)
			{
				playerControl.canMove = false;
				rolling = true;
			}
		}

		if (rolling)
		{
			if(!hasDirection)
			{
				dirForward = storeDir * horizontal;
				dirSideways = camera.transform.forward * vertical;
				//dirSideways = cameraHolder.forward * vertical;

				//directionPos = transform.position + (storeDir * horizontal) + (cameraHolder.forward * vertical);
				directionPos = transform.position + (storeDir * horizontal) + (camera.transform.forward *vertical);
				dir = directionPos - transform.position;
				dir.y = 0;

				animator.SetTrigger("Roll");
				hasDirection = true;

			}

			rigidBody.AddForce((dirForward + dirSideways).normalized * rollSpeed/Time.deltaTime);
			//find the angle
			float angle = Vector3.Angle(transform.forward, dir);

			//if we anrent prepared to jump, then basicall do eveything
			float animCalue = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

			if (angle != 0)
			{
				rigidBody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rollSpeed * Time.deltaTime);
			}

		}

	}
}
