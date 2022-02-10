using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UICanvas
{
    [SerializeField]
    private CanvasName _name = CanvasName.Null;
    [SerializeField]
    private Canvas _canvas = null;

    public CanvasName Name => _name;
    public Canvas Canvas => _canvas;
}
