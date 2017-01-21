using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float SPEED_MAG = 50.0f;
	public float LIFETIME = 3.5f;
	// Use this for initialization
	Vector3 initDirection;
	void Start () {

		Vector3 dir = new Vector3(0, 1, 0);
		Quaternion rot = transform.rotation;
		Debug.Log("Bullet rotation: " + rot.eulerAngles);
		dir = rot * dir;
		initDirection = dir;

		StartCoroutine(SelfDestruct());
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += initDirection * Time.deltaTime * SPEED_MAG;
	}

	// void OnCollisionEnter2D(Collision2D collision)
	// {
	// 	if (collision.gameObject != null && collision.gameObject.tag == "Destructible")
	// 	{
	// 		Destroy(this.gameObject);
	// 	}
	// } 

	IEnumerator SelfDestruct()
	{
		yield return new WaitForSeconds(LIFETIME);
		GameObject.Destroy(this.gameObject);
	}
}
