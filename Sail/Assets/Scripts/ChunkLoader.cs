using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ChunkLoader : MonoBehaviour {

	public GameObject[] chunks;
	public GameObject player;

	private int x;
	private int y;
	private int[] registry = new int[100*100];

	private const int gridSize = 40;

	// Use this for initialization
	void Start () {
		x = y = 50;
	}
	
	// Update is called once per frame
	void Update () {
		int update_x = (int)(player.transform.position.x / gridSize) + 50;
		int update_y = (int)(player.transform.position.y / gridSize) + 50;

		if(update_x != x || update_y != y) {
			x = update_x;
			y = update_y;
			LoadChunks ();
		}
	}

	void LoadChunks() {
		for(int i = (x + 1); i >= (x - 2); i--) {
			for(int j = (y + 2); j >= y; j--) {

				if (registry [i * 100 + j] > 0)
					continue;
				
				int grid_x = i - 50;
				int grid_y = j - 50;
				// this needs to change to factor in distance traveled - order array in order of difficulty
				int cIndex = Random.Range (0, chunks.Length);
//				Quaternion rot = new Quaternion ();
//				rot.eulerAngles = new Vector3 (0, 0, Random.Range (0, 4) * 90);
				Vector3 pos = new Vector3 (grid_x * gridSize, grid_y * gridSize);
				Instantiate (chunks [cIndex], pos, Quaternion.identity);//rot);

				registry [i * 100 + j] = 1;
			}
		}
	}
}
