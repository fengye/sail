using UnityEngine;
using System.Collections;

public class TrailGenerator : MonoBehaviour {

	public Transform[] particleObject;

	public float interval = 0.5f;

	float currTime;
	// Use this for initialization
	void Start () {
	
		currTime = 0;
	}
	
	// Update is called once per frame
	void Update () {

		currTime += Time.deltaTime;

		if (currTime > interval)
		{
			currTime -= interval;

			int length = particleObject.Length;

			int index = Random.Range(0, length - 1);
			Instantiate(particleObject[index], this.transform.position, this.transform.rotation);

		}
	
	}
}
