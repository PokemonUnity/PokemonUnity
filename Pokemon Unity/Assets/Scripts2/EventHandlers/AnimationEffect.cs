/*using UnityEngine;
using System.Collections;

public class AnimationEffect : MonoBehaviour {
	public int totalTiles = 1;
	public int tilesPerRow = 5;
	public float speed = 10;
	float lifetime = 0;

	void Start(){
		renderer.material.mainTextureScale = new Vector2(1/(float)tilesPerRow, 1/Mathf.Ceil((float)totalTiles/(float)tilesPerRow));
	}

	void Update(){
		lifetime+=Time.deltaTime;
		int currentTile = (int)Mathf.Floor(lifetime*speed);
		float Xpos = (float)(currentTile%tilesPerRow) / (float)tilesPerRow;
		float Ypos = (float)(1+currentTile/tilesPerRow) / Mathf.Ceil((float)totalTiles/(float)tilesPerRow);
		renderer.material.mainTextureOffset = new Vector2(Xpos,Ypos);
		if (currentTile>=totalTiles)		Destroy(gameObject);
	}
}*/