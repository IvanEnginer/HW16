using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SelectionIndicator;

    public virtual void Start()
    {
        SelectionIndicator.SetActive(false);
    }

    public virtual void OnHowur()
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnhover()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Select()
    {
        SelectionIndicator.SetActive(true);
    }

    public virtual void Unselect()
    {
        SelectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point)
    {

    }

    public virtual void WhenClickOnBuilding()
    {

    }

    public virtual void HideMenu()
    {

    }
}
