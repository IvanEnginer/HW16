using TMPro;
using UnityEngine;

public class CreatButton : MonoBehaviour
{
    public Unit UnitPrefab;
    public Barrack Barrack;
    public TextMeshProUGUI Price;

    private Resourses _resources;

    [SerializeField] private float _minRangeSpawn = 0f;
    [SerializeField] private float _maxRangeSpawn = 1f;

    private void Start()
    {
        Price.text = UnitPrefab.Price.ToString();
        _resources = FindObjectOfType<Resourses>();
    }

    public void TryBuy()
    {
        int price = UnitPrefab.Price;

        if (_resources.Money >= price)
        {
            _resources.Money -= price;


            Barrack.CrateUnit(UnitPrefab);
        }
        else
        {
            Debug.Log("Low money");
        }
    }
}
