using UnityEngine;
using System.Collections;

public class RockSpawner : MonoBehaviour {

	public GameObject rockObject;
	public GameObject player;
	public int rockCount_min;
	public int rockCount_max;

	private int x;
	private int y;
	private int gridWidth;
	private int gridHeight;

	private int[] registry = new int[100*100];

	void Start() {
		x = y = 50;
		gridWidth = gridHeight = 40;
//		GenerateGrids ();
	}

	void Update() {
		int new_x = (int)player.transform.position.x / gridWidth + 50;
		int new_y = (int)player.transform.position.y / gridHeight + 50;
		if (new_x != x || new_y != y) {
			x = new_x;
			y = new_y;
			Debug.Log ("update: " + x + " " + y);
			GenerateGrids ();
		}
	}

	void GenerateGrids() {
		for(int i = x+2; i >= x-2; i--) {
			for(int j = y+2; j >= y; j--) {

				if (registry [i * 100 + j] > 0)
					continue;
				
				GenerateGrid (i-50, j-50);

				registry [i * 100 + j] = 1;
			}
		}
	}

	void GenerateGrid(int grid_x, int grid_y) {
		
		
		int rockCount = Random.Range (rockCount_min, rockCount_max);
		for(int i = 0; i < rockCount; i++) {
			Vector3 pos = new Vector3 (Random.Range (grid_x*gridWidth, (grid_x+1)*gridWidth), Random.Range(grid_y*gridHeight, (grid_y+1)*gridHeight));
			Instantiate (rockObject, pos, Quaternion.identity);
		}


	}
}
