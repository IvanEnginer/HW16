using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public enum SeletionState{
    UnitsSelected,
    Frame,
    Building,
    Other
}

public class Managment : MonoBehaviour
{
    public Camera Camera;
    public SelectableObject Howered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    public Image FrameImage;

    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    public SeletionState CurrentSelectionState;

    private void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectebelCollider>())
            {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectebelCollider>().SelectableObject;

                if (Howered)
                {
                    if (Howered != hitSelectable)
                    {
                        Howered.OnUnhover();
                        Howered = hitSelectable;
                        Howered.OnHowur();
                    }
                }
                else
                {
                    Howered = hitSelectable;
                    Howered.OnHowur();
                }
            }
            else
            {
                OnhoverCurrent();
            }

            if (CurrentSelectionState == SeletionState.UnitsSelected)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));


                        for (int i = 0; i < ListOfSelected.Count; i++)
                        {
                            int row = i / rowNumber;
                            int column = i % rowNumber;

                            Vector3 point = hit.point + new Vector3(row, 0f, column);

                            ListOfSelected[i].WhenClickOnGround(point);
                        }
                    }
                }
            }
        } else
        {
            OnhoverCurrent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Howered)
            {
                if (Input.GetKey(KeyCode.LeftControl) == false)
                {
                    UnselectAll();
                }
                
                CurrentSelectionState = SeletionState.UnitsSelected;

                Select(Howered);
            }
        }



        if (Input.GetMouseButtonDown(1))
        {
            UnselectAll();
        }

        if(Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;

            if(size.magnitude > 10)
            {
                FrameImage.enabled = true;

                FrameImage.rectTransform.anchoredPosition = min;

                FrameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnselectAll();

                Unit[] allUnits = FindObjectsOfType<Unit>();

                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPosition = Camera.WorldToScreenPoint(allUnits[i].transform.position);

                    if (rect.Contains(screenPosition))
                    {
                        Select(allUnits[i]);
                    }
                }

                CurrentSelectionState = SeletionState.Frame;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            FrameImage.enabled = false;

            if(ListOfSelected.Count > 0)
            {
                CurrentSelectionState = SeletionState.UnitsSelected;
            }
            else
            {
                CurrentSelectionState = SeletionState.Other;
            }
        }
    }

    public void UnSelect(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Remove(selectableObject);
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if(selectableObject.GetComponent<Bilding>())
        {
            selectableObject.WhenClickOnBuilding();

            CurrentSelectionState = SeletionState.Building;
        }

        if(ListOfSelected.Contains(selectableObject) == false)
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    private void UnselectAll()
    {
        for (int i = 0; i < ListOfSelected.Count; i++)
        {
            ListOfSelected[i].Unselect();

            if (ListOfSelected[i].CompareTag("Barrack"))
            {
                ListOfSelected[i].HideMenu();
            }
        }

        ListOfSelected.Clear();


        CurrentSelectionState = SeletionState.Other;
    }

    private void OnhoverCurrent()
    {
        if (Howered)
        {
            Howered.OnUnhover();
            Howered = null;
        }
    }
}
