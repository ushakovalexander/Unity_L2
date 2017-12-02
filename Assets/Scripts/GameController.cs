using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
  public int columns = 10;
  public int rows = 10;
  public float spacing = 1f;

  [Range(0f, 1.0f)]
  public float blockerSpawnProbability = 0.1f;

  private List<PieceController> _pieces;
  private List<PieceController> _blockers;
  private GameObject _piecesParent;
  private Vector2 _parentOffset;

  void Start() {
    InitGrid(this.columns, this.rows);
  }

  private void InitGrid(int columns, int rows) {
    _piecesParent = new GameObject("PiecesParent");
    _parentOffset = new Vector2(-columns * 0.5f * spacing + spacing * 0.5f, -rows * 0.5f * spacing + spacing * 0.5f);
    _piecesParent.transform.position = _parentOffset;

    Boolean isBlocker;
    for(int i = 0; i < columns; i++) {
      for(int j = 0; j < rows; j++) {
        isBlocker = (UnityEngine.Random.Range(0.01f, 1.0f)) < blockerSpawnProbability;
        InitPiece(i, j, isBlocker);
      }
    }
  }

  private PieceController InitPiece(int colIndex, int rowIndex, Boolean isBlocker = false) {
    if(_piecesParent == null) {
      Debug.Log("Invalid piece parent");
      return null;
    }
    var position = GetPiecePositionOnGrid(colIndex, rowIndex);
    var prefab = isBlocker ? GetBlockerPrefab() : GetPiecePrefab();
    PieceController piece = null;
    if(prefab != null) {
      piece = Instantiate<PieceController>(prefab, position, Quaternion.identity, _piecesParent.transform);
      piece.Initialize(colIndex, rowIndex);
      piece.OnPieceClickedEvent += OnPieceClicked;
      if(!isBlocker) {
        AddPiece(piece);
      } else {
        AddBlocker(piece);
      }
    }
    return piece;
  }

  private Vector2 GetPiecePositionOnGrid(float colIndex, float rowIndex) {
    return new Vector2(colIndex * spacing, rowIndex * spacing) + _parentOffset;
  }

  private void AddPiece(PieceController piece) {
    if(piece != null) {
      if(_pieces == null) {
        _pieces = new List<PieceController>(columns * rows);
      }
      if(!_pieces.Contains(piece)) {
        _pieces.Add(piece);
      }
    }
  }

  private void AddBlocker(PieceController blocker) {
    if(blocker != null) {
      if(_blockers == null) {
        _blockers = new List<PieceController>(columns * rows);
      }
      if(!_blockers.Contains(blocker)) {
        _blockers.Add(blocker);
      }
    }
  }

  private PieceController GetPiece(int colIndex, int rowIndex) {
    if(_pieces != null) {
      foreach(PieceController piece in _pieces) {
        if(piece != null && piece.colIndex == colIndex && piece.rowIndex == rowIndex) {
          return piece;
        }
      }
    }
    return null;
  }

  private PieceController GetPiecePrefab() {
    return GetPiecePrefab(UnityEngine.Random.Range(0, 6));
  }

  private PieceController GetPiecePrefab(int id) {
    return Resources.Load<PieceController>("Prefabs/" + "Piece" + id);
  }

  private PieceController GetBlockerPrefab() {
    return Resources.Load<PieceController>("Prefabs/" + "Blocker");
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
      if(_pieces != null && _pieces.Contains(piece)) {
        _pieces.Remove(piece);
      }
      MovePiecesDown(piece);
    }
  }

  private void MovePiecesDown(PieceController removedPiece) {
    if(removedPiece != null && _pieces != null) {
      int maxRowIndex = rows;
      int minRowIndex = removedPiece.rowIndex;

      if(_blockers != null) {
        foreach(PieceController blocker in _blockers) {
          if(blocker.colIndex == removedPiece.colIndex) {
            if(blocker.rowIndex > removedPiece.rowIndex) {
              maxRowIndex = Math.Min(maxRowIndex, blocker.rowIndex);
            } else {
              minRowIndex = Math.Max(minRowIndex, blocker.rowIndex);
            }
          }
        }
      }

      foreach(PieceController piece in _pieces) {
        if(piece.colIndex == removedPiece.colIndex && piece.rowIndex < maxRowIndex && piece.rowIndex > minRowIndex) {
          piece.UpdateRow(piece.rowIndex - 1, GetPiecePositionOnGrid(piece.colIndex, piece.rowIndex - 1));
        }
      }
      if(maxRowIndex == rows) {
        AddPieceToTop(removedPiece.colIndex);
      }
    }
  }

  private void AddPieceToTop(int colIndex) {
    int initialRowIndex = rows;
    var newPiece = InitPiece(colIndex, initialRowIndex);
    if(newPiece != null) {
      newPiece.UpdateRow(newPiece.rowIndex - 1, GetPiecePositionOnGrid(colIndex, newPiece.rowIndex - 1));
    }
  }
}
