using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{ 
	public DrawLine drawLine; // Draw line for elimination
	public GameObject tilePrefab; // Prefab for tile
	public List<Tile> tiles; // Instantiation for storing tiles
	public List<Tile> _tiles; // Random tiles	
	public List<Tile> tilesEdge; // Invisible tiles for corners
	public int x, y; 
	
	private Tile firstTile = null;
	private Tile anotherTile = null;

	private bool destroy;
	private Vector3 mousePos;
	
	private int threshold = 2;
	
	public List<Tile> comparePointsA = new List<Tile>();
	public List<Tile> sameTiles = new List<Tile>();
	

	void Start()
	{
		this.gameObject.transform.position = Vector3.zero;
		Spawn();
		
	}

	// Instantiate the tiles
	private void Spawn() {
		float num = (x * y - (2 * x + 2 * y - 4)) * 0.5f;

		for(int i = 0; i < num; i++) {

			int idTex = Random.Range (20, 45);
			GameObject obj1 = Instantiate(tilePrefab) as GameObject;
			GameObject obj2 = Instantiate(tilePrefab) as GameObject;
			Tile tile1 = obj1.GetComponent<Tile>();
			Tile tile2 = obj2.GetComponent<Tile>();
			tile1.Init(idTex);
			tile2.Init(idTex);
			tiles.Add(tile1);
			tiles.Add(tile2);
		}

		for(int i = 0; i < ((2 * x + 2 * y) - 4); i++) {

			GameObject obj = Instantiate(tilePrefab) as GameObject;
			obj.name = "edage";
			Tile tile = obj.GetComponent<Tile>();
			tilesEdge.Add(tile);
		}

		CreatTile();

		for(int i = 0; i < _tiles.Count; i++) {

			_tiles[i].transform.name = i.ToString();
			_tiles[i].id = i;
		}

		this.transform.position = new Vector3(-(x / 2.0f - 0.5f), -(y / 2.0f - 0.5f), 0);
	}
	// Randomly create tiles
	private void CreatTile() {

		float _y = 0.0f;

		for(int i = 0; i < y; i++) {

			float _x = 0.0f;

			for(int j = 0; j < x; j++) {

				if(i == 0 || j == 0 || i == y - 1 || j == x - 1) {

					tilesEdge[0].transform.position = new Vector3(_x, _y, 0);
					tilesEdge[0].pos = new Vector2(_x, _y);
					tilesEdge[0].transform.rotation = new Quaternion(0, 0, 180, 0);
					tilesEdge[0].transform.parent = this.gameObject.transform;

					_tiles.Add(tilesEdge[0]);

					tilesEdge[0].transform.localScale = Vector3.zero;
					tilesEdge[0].type = false;
					tilesEdge.RemoveAt(0);
				} 
				else {

					int id = Mathf.FloorToInt(Random.Range(0, tiles.Count));
					tiles[id].transform.position = new Vector3(_x, _y, 0);
					tiles[id].pos = new Vector2(_x, _y);
					tiles[id].transform.rotation = new Quaternion(0, 0, 180, 0);
					tiles[id].transform.parent = this.gameObject.transform;
					_tiles.Add(tiles[id]);

					tiles.RemoveAt(id);
				}
				_x += 1;
			}
			_y += 1;
		}
	}
	// Select first touch tile, with Ray Cast
	private void SelectFirstTile() {
		
		//Ray ray = Camera.mainCamera.ScreenPointToRay(mousePos);
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hit;
		int mask = 1 << 8;
		
		if(Physics.Raycast(ray, out hit, mask)) {

			if(firstTile == null) {
				
				firstTile = hit.transform.GetComponent<Tile>();
				firstTile.SetTileTexture(1);
				addTile(firstTile);
				print("firstTile = hit.transform.GetComponent<Tile>();" + firstTile.transform.name);
				
			}
		}
	}

	// Select other touch tile, with Ray Cast, check the tile and save the same tile into list
	private void SelectAnotherTile() {
		
		//Ray ray = Camera.mainCamera.ScreenPointToRay(mousePos);
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hit;
		int mask = 1 << 8;
		
		if(Physics.Raycast(ray, out hit, mask)) {
			if(firstTile != null) {

				anotherTile = hit.transform.GetComponent<Tile>();
				print ("otherTile = hit.transform.GetComponent<Tile>();" + anotherTile.transform.name);
				
				if(Compare(firstTile, anotherTile)) {
					// add the same tile to the List and mark the new added tile as the "first" tile.
					addTile(anotherTile);
					anotherTile.SetTileTexture(1);
					firstTile = anotherTile;
				}
			}
		}	
	}

	// Set default null to firstTile and anotherTile
	private void SetDefaultTiles() {
	
		firstTile = null;
		anotherTile = null;
	}

	// Add the same title to List
	private void addTile(Tile tile) {
		
		sameTiles.Add(tile);
	}
	
	private bool Compare(Tile tile1, Tile tile2) {  

		destroy = false;
		// same tile: finger is still touching the same tile.
		if(tile1.pos.x == tile2.pos.x && tile1.pos.y == tile2.pos.y) {

			firstTile = anotherTile;
			anotherTile = null;
			return false;
		} 
		
		// Two tiles have the same x
		else if(tile1.pos.x == tile2.pos.x && tile1.pos.y != tile2.pos.y) {

			destroy = CheckY(tile1, tile2);
		} 
		
		// Two tiles have the same y
		else if(tile1.pos.x != tile2.pos.x && tile1.pos.y == tile2.pos.y) {

			destroy = CheckX(tile1, tile2);

		}

		// Two tiles are adjacent diagonal

		else if(Mathf.Abs (tile1.pos.x - tile2.pos.x) == 1 && Mathf.Abs (tile1.pos.y - tile2.pos.y) == 1) {
		
			destroy = CheckXY(tile1, tile2);
		}

		return destroy;
	}

	// Eliminate all the selected same tiles and also draw lines
	private void CleanAllTiles(List<Tile> targetTiles) {

		if(targetTiles.Count != 0) {

			drawLine.transform.position = targetTiles[0].transform.position;
			
			for(int i = 0; i <= targetTiles.Count - 1; i++) {

				targetTiles[i].transform.localScale = Vector3.zero;
				targetTiles[i].type = false;
				targetTiles[i] = null;
				if (i <= targetTiles.Count - 2) {

					drawLine.waypoints.Add (targetTiles[i + 1].transform);
				}

			}

			SetDefaultTiles();

			drawLine.MoveToWaypoint();
		}
	}

	private bool CheckX(Tile a, Tile b) {

		bool flag = true;
		int _min, _max;

		if(a.idTex == b.idTex) {

			CompareID(a, b, out _min, out _max);
			_min += 1;

			if(_min == _max)
				return true;

			for(int i = _min; i < _max; i++) {

				if(_tiles[i].type == true) {

					flag = false;
					break;
				}
			}
			return flag;
		} else
			return false;
	}
	
	private bool CheckY(Tile a, Tile b) {

		bool flag = true;
		int _min, _max;

		if(a.idTex == b.idTex) {

			CompareID(a, b, out _min, out _max);

			_min += x;
			if(_min == _max)
				return true;

			for(int i = _min; i < _max; i+=x) {

				if(_tiles[i].type == true) {
					flag = false;
					break;
				}
			} 
			return flag;
		} 
		else
			return false;
	}

	// For adjacent diagonal
	private bool CheckXY(Tile a, Tile b) {
	
		bool flag = false;
		if(a.idTex == b.idTex) {
		
			flag = true;
			return flag;
		} 
		else 
			return false;
	}
	
	private void CompareID(Tile a, Tile b, out int min, out int max) {
		
		if (a.id < b.id) {
			min = a.id;
			max = b.id;
		} 
		else {
			min = b.id;
			max = a.id;
		}
	}

	void Update()
	{
		// Detect the mouse click and trigger the first tile selection. 
		if(Input.GetMouseButtonDown(0)) {
			
			mousePos = Input.mousePosition;
			SelectFirstTile ();
		}
		// The given mouse held down and it is to select the second tile.
		if(Input.GetMouseButton(0)) {
			
			mousePos = Input.mousePosition;
			SelectAnotherTile();
		}
		
		if(Input.GetMouseButtonUp(0)) {

			if(sameTiles != null && sameTiles.Count >= threshold) {

				CleanAllTiles(sameTiles);
				sameTiles.Clear();

			}
			else {

				drawLine.ClearPath();

				// Release selected tiles and clean the list
				for (int i = 0; i <= sameTiles.Count - 1; i++) {

					sameTiles[i].SetTileTexture(0);
				}
				sameTiles.Clear();

				SetDefaultTiles();

			}
		}
	}
}