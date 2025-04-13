using UnityEngine;

public class WorldVisualizer : MonoBehaviour
{
    public int width;
    public int height;
    public Color borderColor = Color.red;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = borderColor;
        lineRenderer.endColor = borderColor;

        UpdateBorder();
    }

    public void SetWorldSize(int newWidth, int newHeight)
    {
        width = newWidth;
        height = newHeight;
        UpdateBorder();
    }

    private void UpdateBorder()
    {
        if (lineRenderer == null) return;

        Vector3[] corners = new Vector3[5];
        corners[0] = new Vector3(-width / 2f, -height / 2f, 0);
        corners[1] = new Vector3(-width / 2f, height / 2f, 0);
        corners[2] = new Vector3(width / 2f, height / 2f, 0);
        corners[3] = new Vector3(width / 2f, -height / 2f, 0);
        corners[4] = corners[0]; // terug naar begin

        lineRenderer.SetPositions(corners);
    }
}