using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public float fireInterval = 1.0f;
	public Transform bulletObject;

	float currTime;
	int bulletRemains;

	bool permitToFire;
	ParticleSystem particleSystem;
	// Use this for initialization
	void Start () {
	
		currTime = 0;
		bulletRemains = 0;
		permitToFire = false;

		particleSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
		currTime += Time.deltaTime;

		if (permitToFire)
		{
			StartCoroutine(EmitParticle());
			Instantiate(bulletObject, transform.position, transform.parent.rotation);
			bulletRemains--;

			permitToFire = false;
		}
	}

	IEnumerator EmitParticle()
	{
		particleSystem.Play();
		yield return new WaitForSeconds(0.15f);
		particleSystem.Stop();
	}

	public void EnableCannon(bool enable)
	{
		if (enable)
		{
			bulletRemains = 30;
		}
		else
		{
			bulletRemains = 0;
		}
	}

	public void Fire()
	{
		if (currTime > fireInterval && bulletRemains > 0)
		{
			DoFire();
			currTime = 0;
		}
	}

	void DoFire()
	{
		permitToFire = true;
		
	}
}
