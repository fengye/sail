using UnityEngine;
using System.Collections;

// works but probably not as well as it could
public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = .15f;
	public Transform target;

	private Camera camera;
	private Vector3 veloctiy = Vector3.zero;

	void Start () {
		camera = GetComponent<Camera> ();
	}
	
	void LateUpdate () {
		if(target) {
			Vector3 point = camera.WorldToViewportPoint (target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp (transform.position, destination, ref veloctiy, dampTime);
		}
	}
}
