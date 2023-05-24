using TMPro;
using UnityEngine;

public class Resourses : MonoBehaviour
{
    public int Money;

    [SerializeField] private TextMeshProUGUI _money;


    private void Start()
    {
        _money.text = Money.ToString();
    }

    public void TryGetMoney()
    {

    }

    public void AddMoney(int money)
    {
        Money += money;
    }
}
