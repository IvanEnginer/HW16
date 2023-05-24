using UnityEngine;

public class Barrack : Bilding
{
    public Transform Spawn;

    public  Canvas Canvas;

    public override void WhenClickOnBuilding()
    {
        base.WhenClickOnBuilding();

        Canvas.gameObject.SetActive(true);
    }

    public override void HideMenu()
    {
        Canvas.gameObject.SetActive(false);
    }

    public void CrateUnit(Unit unitPrefab)
    {
       Unit newUnit = Instantiate(unitPrefab, Spawn.position, Quaternion.identity);

       Vector3 position = Spawn.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

       newUnit.GetComponent<Unit>().WhenClickOnGround(position);
    }
}
