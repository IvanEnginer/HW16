using System.Collections.Generic;
using UnityEngine;

public class BildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;

    public Camera RaycastCamera;

    public Resources Resources;

    public Bilding CurrentBuilding;

    public Dictionary<Vector2Int, Bilding> BuildingsDictionary = new Dictionary<Vector2Int, Bilding>();

    private Plane _plane;

    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (CurrentBuilding == null)
        {
            return;
        }

        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0f, z) * CellSize;

        if(CheakAllow(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();

            if (Input.GetMouseButtonDown(0))
            {
                InstaleBuilding(x, z, CurrentBuilding);

                CurrentBuilding = null;
            }
        }
        else
        {
            CurrentBuilding.DisplayUnacceptablePosition();
        }
    }

    public void CreatBuilding(Bilding buildinPrefab)
    {
        Bilding newBuilding = Instantiate(buildinPrefab);

        newBuilding.Init(Resources);

        CurrentBuilding = newBuilding;
    }

    private void InstaleBuilding(int xPosition, int zPosition, Bilding building)
    {
        for(int x = 0; x < building.XSize; x++)
        {
            for(int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition+ z);

                BuildingsDictionary.Add(coordinate, CurrentBuilding);
            }
        }

        foreach (var item in BuildingsDictionary)
        {
            Debug.Log(item);
        }
    }

    private bool CheakAllow(int xPosition, int zPosition, Bilding building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);

                if (BuildingsDictionary.ContainsKey(coordinate))
                {
                    return false;
                } 
            }
        }
        return true;
    }
}
