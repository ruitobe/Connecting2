using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public int id;
	public int idTex; // ID used to distinguish if the tiles are the same
	public Vector2 pos ;
	public bool type = true; // The status of tile, when need to eliminate it, set it to false
	public float x, y;
	public Texture texA, texB; // Texture used 
	public GameObject mask; // For highlight the selected same tiles

	public void Init(int _idTex) {
		idTex = _idTex;
		Vector2 offset = TexOffset(_idTex);
		this.renderer.material.SetTextureOffset("_MainTex", offset);
		this.renderer.material.SetTextureScale("_MainTex", new Vector2(0.2f, 0.1f));
		
	}
	
	public void SetTileTexture(int i) {

		if(i == 0) {
			this.renderer.material.mainTexture = texA;
			mask.transform.localScale = Vector3.zero;
		}
		
		if(i == 1) {
			this.renderer.material.mainTexture = texB;
			mask.transform.localScale = new Vector3(0.1380835f, 0.1380835f, 0.1380835f);
		}
	}

	Vector2 TexOffset(int _idTex) {

		int a = (int)(_idTex / x);
		int b = (int)(_idTex % x);
		Vector2 offset = new Vector2(b / x, (y - 1 - a) / y);
		return offset;	
	}

	Vector2 TexSize() {

		Vector2 size = new Vector2 (1 / x, 1 / y);
		return size;
	}

}
