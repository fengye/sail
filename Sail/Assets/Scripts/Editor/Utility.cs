using UnityEngine;
using System.IO;
using UnityEditor;

public class Utility
{
	public static void writeStringToFile(string filename, string str) {

		Debug.Log("Write to: " + Application.dataPath +"/" + filename);

	   File.WriteAllText(Application.dataPath + "/" + filename , str);
	}

	public static string readStringFromFile(string filename)
	{

		return File.ReadAllText(Application.dataPath +"/" + filename);
	}

	[MenuItem("Pacjoy/Rebuild last level")]
	public static void RecreateScene()
	{
		GameObject root = GameObject.Find("/LevelRoot");

		string line;
		System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath +"/leveldata.txt" );
		while((line = file.ReadLine()) != null)
		{
			string[] values = line.Split(',');

			if (values != null && values.Length > 0)
			{
				float x, y, z;
				float qx, qy, qz, qw;

				x = float.Parse(values[0]);
				y = float.Parse(values[1]);
				z = float.Parse(values[2]);
				qx = float.Parse(values[3]);
				qy = float.Parse(values[4]);
				qz = float.Parse(values[5]);
				qw = float.Parse(values[6]);

				Vector3 pos = new Vector3(x, y, z);
				Quaternion rot = new Quaternion(qx, qy, qz, qw);

				Object obj = Resources.Load("BoxObject");
				GameObject go = (GameObject)GameObject.Instantiate(obj, pos, rot);
				go.transform.parent = root.transform;
			}
		}

		file.Close();
	}
}
