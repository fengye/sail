using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public float fireInterval = 1.0f;
	public Transform bulletObject;

	float currTime;
	int bulletRemains;

	bool permitToFire;
	ParticleSystem smokeParticleSystem;
	// Use this for initialization
	void Start () {
	
		currTime = 0;
		bulletRemains = 0;
		permitToFire = false;

		smokeParticleSystem = GetComponent<ParticleSystem>();
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
		smokeParticleSystem.Play();
		yield return new WaitForSeconds(0.15f);
		smokeParticleSystem.Stop();
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
