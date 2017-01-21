using UnityEngine;
using System.Collections;

public class FoamParticle : MonoBehaviour {

	float lifeTime;
	float currTime;
	SpriteRenderer spriteRenderer;
	// Use this for initialization


	void Start () {

		lifeTime = 1.0f;
		currTime = lifeTime;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		currTime -= Time.deltaTime;

		float alpha = currTime / lifeTime;
		spriteRenderer.color = new Color(1, 1, 1, alpha);

		float invAlpha = 1 - alpha;
		transform.localScale = new Vector3(1 + invAlpha * 1.5f, 1 + invAlpha * 1.5f, 1 + invAlpha * 1.5f);

		if (currTime < 0)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
