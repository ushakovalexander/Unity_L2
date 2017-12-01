using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public int columns = 10;
	public int rows = 10;
	public float spacing = 1f;

	void Start () {
		InitGrid(this.columns, this.rows);
	}

	private void InitGrid(int columns, int rows) {
		var pieceParent = new GameObject("PiecesParent");
		var offset = new Vector2(-columns * 0.5f * spacing + spacing * 0.5f, -rows * 0.5f * spacing	+ spacing * 0.5f);
		pieceParent.transform.position = offset;

		for (int i = 0; i < columns; i++) {
			for (int j = 0; j < rows; j++){
				InitPiece(i, j, pieceParent, offset);
			}
		}
	}

    private void InitPiece(int colIndex, int rowIndex, GameObject parent, Vector2 offset) {
        // var prefab = GetPiecePrefab();
		var position = new Vector2(colIndex * spacing, rowIndex * spacing) + offset;
		var prefab = Resources.Load("Prefabs/" + "Piece" + Random.Range(0, 6));
		var piece = Instantiate(prefab, position, Quaternion.identity, parent.transform);
    }

	// private Prefab GetPiecePrefab() {
	// 	return GetPiecePrefab(Random.Range(0, 6));
	// }

    // private Prefab GetPiecePrefab(int id) {
    //     var prefab = Resources.Load("Prefabs/" + "Piece" + id);
	// 	var pieceInstance = Instantiate(prefab);
	// 	return prefab;
    // }
}
