using UnityEngine;
using System.Collections;

public class BoxDropper : MonoBehaviour {

	public float dropInterval = 1.0f;
	public Transform boxObject;

	float accumTime;
	// Use this for initialization
	void Start () {
	
		accumTime = 0;
	}
	
	// Update is called once per frame
	void Update () {

		accumTime += Time.deltaTime;

		if (accumTime > dropInterval)
		{
			accumTime -= dropInterval;

			Instantiate(boxObject, this.transform.position, this.transform.rotation);

		}
	
	}
}
