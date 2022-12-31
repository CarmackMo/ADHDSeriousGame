using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptableMask : MaskableGraphic
{
    public List<Vector2> spaceVertexes;

    private List<Vector2> leftMaskVertexes = new List<Vector2>();
    private List<Vector2> midUpVertexes = new List<Vector2>();
    private List<Vector2> midDownVectexes = new List<Vector2>();
    private List<Vector2> rightMaskVertexes = new List<Vector2>();
    private float width = Screen.width;
    private float height = Screen.height;

    private Action callBack;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        InitVertex(vh);
        DrawMask(leftMaskVertexes, vh, 0);
        DrawMask(midUpVertexes, vh, 4);
        DrawMask(midDownVectexes, vh, 8);
        DrawMask(rightMaskVertexes, vh, 12);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void AddCallBack(Action callBack)
    {
        this.callBack += callBack;
    }

    public void RemoveCallBack(Action callBack)
    {
        this.callBack -= callBack;
    }

    public void OnClickMask()
    {
        callBack?.Invoke();
    }

    private void InitVertex(VertexHelper helper)
    {
        Vector2 spaceLeftDown = spaceVertexes[0];
        Vector2 spaceRightUp = spaceVertexes[1];

        leftMaskVertexes.Clear();
        leftMaskVertexes.Add(new Vector2(0,               0));
        leftMaskVertexes.Add(new Vector2(0,               height));
        leftMaskVertexes.Add(new Vector2(spaceLeftDown.x, height));
        leftMaskVertexes.Add(new Vector2(spaceLeftDown.x, 0));

        midUpVertexes.Clear();
        midUpVertexes.Add(new Vector2(spaceLeftDown.x, spaceRightUp.y));
        midUpVertexes.Add(new Vector2(spaceLeftDown.x, height));
        midUpVertexes.Add(new Vector2(spaceRightUp.x,  height));
        midUpVertexes.Add(new Vector2(spaceRightUp.x,  spaceRightUp.y));

        midDownVectexes.Clear();
        midDownVectexes.Add(new Vector2(spaceLeftDown.x, 0));
        midDownVectexes.Add(new Vector2(spaceLeftDown.x, spaceLeftDown.y));
        midDownVectexes.Add(new Vector2(spaceRightUp.x,  spaceLeftDown.y));
        midDownVectexes.Add(new Vector2(spaceRightUp.x,  0));

        rightMaskVertexes.Clear();
        rightMaskVertexes.Add(new Vector2(spaceRightUp.x, 0));
        rightMaskVertexes.Add(new Vector2(spaceRightUp.x, height));
        rightMaskVertexes.Add(new Vector2(width,          height));
        rightMaskVertexes.Add(new Vector2(width,          0));
    }

    public void DrawMask(List<Vector2> vertexList, VertexHelper helper, int offset)
    {
        foreach (Vector2 vertex in vertexList)
        {
            helper.AddVert(new UIVertex { position = vertex, color = color });
        }

        helper.AddTriangle(0 + offset, 1 + offset, 2 + offset);
        helper.AddTriangle(0 + offset, 2 + offset, 3 + offset);
    }



}
