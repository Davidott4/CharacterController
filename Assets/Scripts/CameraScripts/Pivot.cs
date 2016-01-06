using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Pivot : FollowTarget {

	protected Transform camera;
	protected Transform pivot;
	protected Vector3 lastTargetPosition;

	// Use this for initialization
	protected virtual void Awake()
	{
		camera = GetComponentInChildren<Camera>().transform;
		pivot = camera.parent;
	}


	protected override void Start () 
	{
		base.Start();
	}
	
	// Update is called once per frame
	virtual protected void Update () 
	{
		if (!Application.isPlaying)
		{
			if(target !=null)
			{
				Follow(999);
				lastTargetPosition = target.position;
			}

			if (Mathf.Abs(camera.localPosition.x) > .5f || Mathf.Abs(camera.localPosition.y) > .5f)
			{
				camera.localPosition = Vector3.Scale(camera.localPosition, Vector3.forward);
			}

			camera.localPosition = Vector3.Scale(camera.localPosition, Vector3.forward);

		}
	}

	protected override void Follow(float deltaTime)
	{
		
	}
}
