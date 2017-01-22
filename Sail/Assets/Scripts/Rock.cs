using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject != null && collision.gameObject.tag == "Bullet")
		{
			Destroy(this.gameObject);

			GameScoreManager.instance.Score += 10;
		}
	}
}
