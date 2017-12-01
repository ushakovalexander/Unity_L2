using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour {
    public int i;
    public int j;
    public event Action<PieceController> OnPieceClickedEvent;

    public void Initialize(int i, int j) {
      this.i = i;
      this.j = j;
    }

    public void OnMouseDown() {
      if(OnPieceClickedEvent != null) {
        OnPieceClickedEvent(this);
      }
    }

    public void Remove() {
      Destroy(gameObject);
    }
}
