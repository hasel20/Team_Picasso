using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInfo : MonoBehaviour
{
    public int number;

    public Color myColor;

    public float myWidth;

    private void Start()
    {
        number = GameManager.instance.AddLine(this);
    }
}
