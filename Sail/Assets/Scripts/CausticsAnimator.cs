using UnityEngine;
using System.Collections;

public class CausticsAnimator : MonoBehaviour {

	MeshRenderer meshRenderer;
	Material material;

	public Vector2 offsetSpeed;

	float currTime;
	// Use this for initialization
	void Start () {
	
		meshRenderer = GetComponent<MeshRenderer>();
		material = meshRenderer.material;
		currTime  = 0;

	}
	
	// Update is called once per frame
	void Update () {

		currTime += Time.deltaTime;
		material.SetTextureOffset("_MainTex", new Vector2(currTime * offsetSpeed.x, currTime * offsetSpeed.y));	
	}
}
