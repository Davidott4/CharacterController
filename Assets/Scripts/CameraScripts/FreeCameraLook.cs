using UnityEngine;

public class FreeCameraLook : Pivot {

	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float turnSpeed = 10f;
	[SerializeField] private float turnSmoothing = .1f;
	[SerializeField] private float tiltMax = 80f;
	[SerializeField] private float tiltMin = 45f;
	[SerializeField] private bool lockTarget = false;

	private PlayerControl playerControl;
	private GameObject player;
	private GameObject cameraTarget;
	private Camera cam;
	private float lookAngle;
	private float tiltAngle;

	private const float LookDistance = 100f;

	private float smoothX = 0;
	private float smoothY = 0;
	private float smoothXVelocity = 0;
	private float smoothYVelocity = 0;

 	protected override void Awake()
	{
		base.Awake();

		player = GameObject.FindGameObjectWithTag("Player");
		cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget");
		playerControl =  player.GetComponent<PlayerControl>();
		Cursor.lockState = CursorLockMode.Confined;
		cam = GetComponentInChildren<Camera>();
		camera = cam.transform;
		pivot = camera.parent;
	}


	// Update is called once per frame
	protected override void Update () {
		base.Update();

		lockTarget = playerControl.lockTarget;


		if (lockTarget && Input.GetButtonDown("Lock Target")) 
		{
			Cursor.lockState = CursorLockMode.Confined;
		}

		//HandleRotationMovement();
	}

	void onDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	protected override void Follow (float deltaTime)
	{
		transform.position = Vector3.Lerp(transform.position, target.position, deltaTime * moveSpeed);
	}

	void HandleRotationMovement()
	{
		Vector3 targetPosition = target.transform.position - (target.transform.forward);



		//NOT LOCKED ON
		if (!lockTarget)
		{

			float x = Input.GetAxis("Look Horizontal");
			float y = Input.GetAxis("Look Vertical");
			Debug.Log(x);

			float smoothX =0f;
			float smoothY =0f;

			if (turnSmoothing >0)
			{
				smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXVelocity, turnSmoothing);
				smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYVelocity, turnSmoothing);
			}
			else
			{
				smoothX = x;
				smoothY = y;
			}

			lookAngle += smoothX * turnSpeed;

			transform.rotation = Quaternion.Euler(0f, lookAngle, 0);

			tiltAngle -=  smoothY * turnSpeed;
			tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

			pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);

		}
		else  //LOCKED ON
		{
			


			float x = (target.transform.position - (target.transform.forward)).x;
			float y = Input.GetAxis("Look Vertical");
			Debug.Log(x);

			float smoothX =0f;
			float smoothY =0f;

			if (turnSmoothing >0)
			{
				smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXVelocity, turnSmoothing);
				smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYVelocity, turnSmoothing);
			}
			else
			{
				smoothX = x;
				smoothY = y;
			}

			lookAngle += smoothX * turnSpeed;

			transform.rotation = Quaternion.Euler(0f, lookAngle, 0);

			tiltAngle -=  smoothY * turnSpeed;
			tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

			pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);


		}
	}
}
