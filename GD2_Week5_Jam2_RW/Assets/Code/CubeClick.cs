using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClick : MonoBehaviour
{
    public Renderer myRenderer;
    Color originalColor;

    public Color clickColor;

    // Start is called before the first frame update
    private void Start()
    {
        originalColor = myRenderer.material.color;
    }
    public void Reset()
    {
        myRenderer.material.color = originalColor;
    }
    public void OnClick()
    {
        myRenderer.material.color = clickColor;
    }
}
