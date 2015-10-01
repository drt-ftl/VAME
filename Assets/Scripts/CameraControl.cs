using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	Vector2 currentMousePosition;
	Vector2 lastMousePosition;
	public float rotationDamping = 3.0f;
	public Transform target;
	private float distance = 10.0f;

	void Start () 
	{
		currentMousePosition = Input.mousePosition;
		lastMousePosition = currentMousePosition;
	}

	void Update () 
	{
		currentMousePosition = Input.mousePosition;
		var delta = currentMousePosition - lastMousePosition;
		var distance = Vector3.Distance (target.transform.position, transform.position);
		if (Input.GetMouseButton(1))
		{

		}
		if (Input.GetMouseButton(2))
		{
			transform.position -= delta.x * transform.right * 0.05f;
			transform.position -= delta.y * transform.up * 0.05f;
			var _distance = Vector3.Distance(target.transform.position, transform.position);
			var ratio = distance/_distance;
			transform.position *= ratio;
			transform.LookAt (target);
		}

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		var scroll = Input.mouseScrollDelta.y;
		transform.position += (ray.direction * scroll);
		lastMousePosition = currentMousePosition;
	}
}
