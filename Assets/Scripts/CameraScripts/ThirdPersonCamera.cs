using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

	[SerializeField] private float distanceAway = 10f;
	[SerializeField] private float distanceUp = 10f;
	[SerializeField] private float smooth =3f ;
	[SerializeField] private Transform follow;
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		follow = GameObject.FindWithTag("CameraTarget").transform;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void LateUpdate()
	{
		targetPosition = follow.position + follow.up*distanceUp - follow.forward * distanceAway;
		Debug.DrawRay(follow.position, Vector3.up * distanceUp , Color.red);
		Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
		Debug.DrawRay(follow.position, targetPosition, Color.magenta);

		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

		transform.LookAt(follow);
	}
}
