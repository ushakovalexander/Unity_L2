using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PieceController : MonoBehaviour {
  public int colIndex;
  public int rowIndex;

  public float tweenRemoveDuration = 0.5f;
  public float tweenMoveDonwDuration = 0.5f;

  public event Action<PieceController> OnPieceClickedEvent;

  private SpriteRenderer spriteRenderer;

  void Start() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void Initialize(int colIndex, int rowIndex) {
    this.colIndex = colIndex;
    this.rowIndex = rowIndex;
  }

  public void OnMouseDown() {
    if(OnPieceClickedEvent != null) {
      OnPieceClickedEvent(this);
    }
  }

  public void UpdateRow(int newRowIndex, Vector2 targetPosition) {
    rowIndex = newRowIndex;
    transform.DOMove(targetPosition, tweenMoveDonwDuration);
  }

  public void Remove() {
    spriteRenderer.sortingOrder++;
    AnimateRemove();
    Destroy(gameObject, tweenRemoveDuration);
  }

  public void AnimateRemove() {
    transform.DOScale(3f, tweenRemoveDuration);
    spriteRenderer.DOFade(0f, tweenRemoveDuration);
  }
}
