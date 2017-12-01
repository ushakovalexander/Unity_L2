using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public int columns = 10;
    public int rows = 10;
    public float spacing = 1f;

    private List<PieceController> _pieces;

    void Start () {
      InitGrid(this.columns, this.rows);
    }

    private void InitGrid(int columns, int rows) {
      var pieceParent = new GameObject("PiecesParent");
      var offset = new Vector2(-columns * 0.5f * spacing + spacing * 0.5f, -rows * 0.5f * spacing  + spacing * 0.5f);
      pieceParent.transform.position = offset;

      for(int i = 0; i < columns; i++) {
        for(int j = 0; j < rows; j++) {
          InitPiece(i, j, pieceParent, offset);
        }
      }
      Debug.Log("Init grid done");
    }

    private void InitPiece(int colIndex, int rowIndex, GameObject parent, Vector2 offset) {
      var position = new Vector2(colIndex * spacing, rowIndex * spacing) + offset;
      var prefab = GetPiecePrefab();
      if(prefab != null) {
        var piece = Instantiate<PieceController>(prefab, position, Quaternion.identity, parent.transform);
        piece.Initialize(colIndex, rowIndex);
        piece.OnPieceClickedEvent += OnPieceClicked;
        AddPiece(piece);
      }
    }

    private void AddPiece(PieceController piece) {
      if(piece != null) {
        if(_pieces == null) {
          _pieces = new List<PieceController>();
        }
        if(!_pieces.Contains(piece)) {
          _pieces.Add(piece);
        }
      }
    }

    private PieceController GetPiecePrefab() {
      return GetPiecePrefab(UnityEngine.Random.Range(0, 6));
    }

    private PieceController GetPiecePrefab(int id) {
      return Resources.Load<PieceController>("Prefabs/" + "Piece" + id);
    }

    private void OnPieceClicked(PieceController piece) {
      if(piece != null) {
        RemovePiece(piece);
      }
    }

    private void RemovePiece(PieceController piece) {
      if(piece != null) {
        piece.OnPieceClickedEvent -= OnPieceClicked;
        piece.Remove();
      }
    }
}
