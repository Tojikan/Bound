using UnityEngine;
using System.Collections;

public class CanvasVisualizer : MonoBehaviour
{
    [SerializeField] private bool disableSpriteOnStart = true;
    [SerializeField] private bool enable = true;
    [SerializeField] private Color canvasBoxColour = new Color(1, 0, 0, .35f);

    void Start()
    {
        if (disableSpriteOnStart)
        {
            var spr = GetComponent<SpriteRenderer>();
            spr.enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        if (enable)
        {
            var canvas = GetComponent<SpriteRenderer>();
            Vector2 areaSize = new Vector2(canvas.bounds.size.x, canvas.bounds.size.y);
            Gizmos.color = canvasBoxColour;
            Gizmos.DrawCube(transform.position, areaSize);
        }
    }
}
