using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	Vector2 currentMousePosition;
	Vector2 lastMousePosition;
	public float rotationDamping = 3.0f;
	public Transform target;
	private float distance = 10.0f;
    private Vector3 perspPos = Vector3.zero;
    private Vector3 orthoPos = Vector3.zero;

	void Start () 
	{
		currentMousePosition = Input.mousePosition;
		lastMousePosition = currentMousePosition;
	}

	void Update () 
	{
        CheckViewType();
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
            
            if (Camera.main.orthographic)
            {
                transform.position -= delta.x * Camera.main.transform.right * factor;
                transform.position -= delta.y * Camera.main.transform.up * factor;
            }
            else
            {
                transform.position -= delta.x * transform.right * factor;
                transform.position -= delta.y * transform.up * factor;
                var _distance = Vector3.Distance(target.transform.position, transform.position);
                var ratio = distance / _distance;
                transform.position *= ratio;
                transform.LookAt(target);
            }
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
            if (Camera.main.orthographic)
            {
                Camera.main.orthographicSize += scroll;
            }
            else
            {
                transform.position += (ray.direction * scroll);
            }
        }
		lastMousePosition = currentMousePosition;
	}

    private void CheckViewType()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Camera.main.orthographic = false;
            Camera.main.transform.LookAt(target);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = Vector3.up * 10;
            Camera.main.transform.LookAt(target);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = Vector3.back * 10;
            Camera.main.transform.LookAt(target);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = Vector3.left * 10;
            Camera.main.transform.LookAt(target);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = Vector3.down * 10;
            Camera.main.transform.LookAt(target);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = Vector3.forward * 10;
            Camera.main.transform.LookAt(target);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = Vector3.right * 10;
            Camera.main.transform.LookAt(target);
        }
    }
}
