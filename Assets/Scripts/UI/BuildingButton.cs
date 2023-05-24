using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    public BildingPlacer BildingPlacer;
    public Bilding BuildingPrefab;

    private Resourses _resources;

    private void Start()
    {
        _resources = FindObjectOfType<Resourses>();
    }

    public void TryBuy()
    {
        int price = BuildingPrefab.Price;

        if (_resources.Money >= price)
        {
            _resources.Money -= price;

            BildingPlacer.CreatBuilding(BuildingPrefab);
        }
        else
        {
            Debug.Log("Low money");
        }
    }
}
