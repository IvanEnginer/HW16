using UnityEngine;

public class Bilding : SelectableObject
{
    public int Price;
    public int XSize = 3;
    public int ZSize = 3;

    public Renderer Renderer;

    private Color _startColor;

    public Resources Resources;

    private void Awake()
    {
        _startColor = Renderer.material.color;
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindAnyObjectByType<BildingPlacer>().CellSize;

        for(int x = 0; x < XSize; x++)
        {
            for(int z = 0; z < ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
            }
        }

    }

    public void DisplayUnacceptablePosition()
    {
        Renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        Renderer.material.color = _startColor;
    }

    public void Init(Resources resources)
    {
        Resources = resources;
    }
}
