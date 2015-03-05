using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public GameObject wall;
	public int mapHeight;
	public int mapWidth;
	int minX = 3;
	int minY = 3;
	public float percentWalls = 0.05f;

	// Use this for initialization
	void Start () {
		Generate();
	}
	
	void Generate() {
		// Require sizes to be large enough
		if(mapHeight < minY || mapWidth < minX) {
			print("Dimensions too small");
			return;
		}
		
		// build perimeter
		for(int x = 0; x < mapWidth; x++) {
			create_wall_at(x, 0);
			create_wall_at(x, mapHeight - 1);
		}
		for(int y = 1; y < mapHeight - 1; y++) {
			create_wall_at(0,y);
			create_wall_at(mapWidth - 1,y);
		}
		
		// throw walls in the middle
		for(int x = 1; x < mapWidth - 1; x++) {
			for(int y = 1; y < mapHeight - 1; y++) {
				if(wallChance()) {
					create_wall_at(x, y);
				}
			}
		}
	}
	
	bool coord_is_wall(int x, int y) {
		return (x == 0 || x == mapWidth || y == 0 || y == mapHeight);
	}
	
	bool wallChance () {
		int range = (int)(1f / percentWalls);
		return (Random.Range(0, range) == 0);
	}
	void create_wall_at(int x, int y) {
		GameObject go = Instantiate(wall) as GameObject;
		go.transform.localPosition = new Vector3(x, 0.5f, y);
		go.transform.parent = transform;
	}
}
