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
            var factor = 0.05f;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                factor = 0.0005f;
            }
            transform.position -= delta.x * transform.right * factor;
			transform.position -= delta.y * transform.up * factor;
			var _distance = Vector3.Distance(target.transform.position, transform.position);
			var ratio = distance/_distance;
			transform.position *= ratio;
			transform.LookAt (target);
            if (InspectorT.slicerForm != null)
                InspectorT.slicerForm.panel1.Invalidate();
		}
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            var factor = 0.005f;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                var scroll = Input.mouseScrollDelta.y * factor;
                Camera.main.nearClipPlane = Mathf.Clamp(Camera.main.nearClipPlane + scroll, 0f, 1f);
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var factor = 0.3f;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                factor = 0.01f;
            }
            var scroll = Input.mouseScrollDelta.y * factor;
            transform.position += (ray.direction * scroll);
        }
		lastMousePosition = currentMousePosition;
	}
}
