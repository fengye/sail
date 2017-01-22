using UnityEngine;
using System.Collections.Generic;

public class BoxDropper : MonoBehaviour {

	public float dropInterval = 1.0f;
	public Transform boxObject;

	float accumTime;

	// for serialization
	List<Vector3> positionList = new List<Vector3>();
	List<Quaternion> rotationList = new List<Quaternion>();

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

			positionList.Add(transform.position);
			rotationList.Add(transform.rotation);

			Instantiate(boxObject, this.transform.position, this.transform.rotation);

		}
	}

	void OnApplicationQuit()
	{
		Debug.Log("OnApplicationQuit");

		string str = "";

		for(int i = 0; i < positionList.Count; ++i)
		{
			str += string.Format("{0:0.000},{1:0.000},{2:0.000},", 
				positionList[i].x, positionList[i].y, positionList[i].z);
			str += string.Format("{0:0.000},{1:0.000},{2:0.000},{3:0.000}", 
				rotationList[i].x, rotationList[i].y, rotationList[i].z, rotationList[i].w);

			str += "\n";
		}

		Debug.Log(str);
		// Utility.writeStringToFile("leveldata.txt", str);
	}
}
