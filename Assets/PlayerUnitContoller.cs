using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnitContoller : MonoBehaviour
{

    //[SerializeField] private List<BaseUnit> selectedUnits = new List<BaseUnit>();

    Vector2 startMousePos;
    Vector2 endMousePos;
    public Texture selectionTexture;
    [SerializeField] LayerMask selectionMask;

    public Collider2D[] previousSelectedUnits;
    public Collider2D[] selectedUnits;

    public Collider2D[] highlightedUnits;
    public Collider2D[] previousCurrentSelectedUnits;
    Collider2D[] previouslyHighlightedUnits;

    [SerializeField] private Transform clickTransform;
    [SerializeField] private Transform clickTransformSaved;
    private void Start()
    {
        clickTransformSaved = clickTransform;
    }

    private void OnGUI()
    {
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IEnumerable<Collider2D> notIncludedUnits = previousSelectedUnits.Except(selectedUnits);

            foreach (Collider2D collider in notIncludedUnits)
            {
                BaseUnit unit = collider.GetComponentInParent<BaseUnit>();
                unit.target = null;
            }

            previousSelectedUnits = selectedUnits;

            startMousePos = new Vector2(0,0);
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            endMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            selectedUnits = Physics2D.OverlapAreaAll(startMousePos, endMousePos, selectionMask); // get current selectedUnits
            if (previouslyHighlightedUnits != null)
            {
                for (int i = 0; i < previouslyHighlightedUnits.Length; i++)
                {
                    BaseUnit unit = previouslyHighlightedUnits[i].GetComponentInParent<BaseUnit>();
                    unit.selectionSprite.enabled = false;
                }
            }
            
            for (int i = 0; i < selectedUnits.Length; i++) // enable highlight for selectedUnits
            {
                BaseUnit unit = selectedUnits[i].GetComponentInParent<BaseUnit>();
                unit.selectionSprite.enabled = true;
            }


            previouslyHighlightedUnits = previousCurrentSelectedUnits.Except(selectedUnits).ToArray(); // get Unit from previousUnits, except selected Units

            previousCurrentSelectedUnits = selectedUnits; //assigning previous as current


            DebugDrawBox(startMousePos,endMousePos,Color.red,0.1f);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D mouseCollisionPoint = Physics2D.OverlapPoint(mousePos,selectionMask);


            if (mouseCollisionPoint == null)
            {
                clickTransform = clickTransformSaved;
                clickTransform.position = mousePos;
            }


            if (mouseCollisionPoint?.GetComponentInParent<BaseUnit>().unitControl == BaseUnit.UnitControl.AI)
            {
                BaseUnit enemyUnit = mouseCollisionPoint.GetComponentInParent<BaseUnit>();

                for (int i = 0; i < selectedUnits.Length; i++)
                {
                    BaseUnit playerUnit = selectedUnits[i].GetComponentInParent<BaseUnit>();
                    playerUnit.overrideTarget = true;
                    playerUnit.target = null;
                    playerUnit.target = enemyUnit.transform;
                    playerUnit.unitState = BaseUnit.UnitState.Approaching;
                
                }
            }
            else
            {
                for (int i = 0; i < selectedUnits.Length; i++)
                {
                    BaseUnit playerUnit = selectedUnits[i].GetComponentInParent<BaseUnit>();
                    playerUnit.overrideTarget = true;
                    playerUnit.target = clickTransform;
                    playerUnit.unitState = BaseUnit.UnitState.Moving;
                }
            }


        }


    }

    private void DrawSelectionBox(Vector2 startMousePos, Vector2 endMousePos, Texture selectionTexture)
    {
        GUI.DrawTexture(new Rect(startMousePos.x,startMousePos.y,endMousePos.x,endMousePos.y), selectionTexture);
    }

    void DebugDrawBox(Vector2 start, Vector2 end, Color color, float duration = 1f)
    {

        float horizontal = end.x - start.x;
        float vertical = end.y - start.y;


        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(start,start + new Vector2(horizontal,0), color, duration);
        Debug.DrawLine(start, start + new Vector2(0, vertical), color, duration);
        Debug.DrawLine(end, end + new Vector2(-horizontal, 0), color, duration);
        Debug.DrawLine(end, end + new Vector2(0, -vertical), color, duration);

    }
}
