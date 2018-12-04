using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUnitContoller : MonoBehaviour
{

    //[SerializeField] private List<BaseUnit> selectedUnits = new List<BaseUnit>();

    AstarPath astarPath;
    Vector2 startMousePos;
    Vector2 endMousePos;
    public Texture selectionTexture;
    [SerializeField] LayerMask selectionMask;

    [SerializeField] static public List<Collider2D> previousSelectedUnits = new List<Collider2D>();
    [SerializeField] static public List<Collider2D> selectedUnits = new List<Collider2D>();

    [SerializeField] static public List<Collider2D> highlightedUnits = new List<Collider2D>();
    [SerializeField] static public List<Collider2D> previousCurrentSelectedUnits = new List<Collider2D>();
    [SerializeField] static List<Collider2D> previouslyHighlightedUnits = new List<Collider2D>();

    [SerializeField] private Transform clickTransform;
    [SerializeField] private Transform clickTransformSaved;


    [SerializeField] private GameObject winPanel, losePanel,gate;
    
    [SerializeField] static List<BaseUnit> aliveEnemyUnits = new List<BaseUnit>();


    private float BoxWidth, BoxHeight, BoxLeft, BoxTop;
    private Vector2 BoxStart, BoxFinish;

    private void OnGUI()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            BoxWidth = Camera.main.WorldToScreenPoint(startMousePos).x - Camera.main.WorldToScreenPoint(endMousePos).x;
            BoxHeight = Camera.main.WorldToScreenPoint(startMousePos).y - Camera.main.WorldToScreenPoint(endMousePos).y;
            BoxLeft = Input.mousePosition.x;
            BoxTop = (Screen.height - Input.mousePosition.y) - BoxHeight;

            GUI.Box(new Rect(BoxLeft, BoxTop, BoxWidth, BoxHeight), "");
        }
    }

    private void Awake()
    {
        aliveEnemyUnits = FindObjectsOfType<BaseUnit>().ToList();
    }

    private void Start()
    {
        clickTransformSaved = clickTransform;
        astarPath = FindObjectOfType<AstarPath>();
    }

    private void Update()
    {
        CheckForWinLose();

        AstarPath.active.Scan();

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

            selectedUnits = Physics2D.OverlapAreaAll(startMousePos, endMousePos, selectionMask).ToList(); // get current selectedUnits
            if (previouslyHighlightedUnits != null)
            {
                foreach (Collider2D unitCollider in previouslyHighlightedUnits)
                {
                    unitCollider.GetComponentInParent<BaseUnit>().UnselectUnit();
                }
            }

            foreach (Collider2D unitCollider in selectedUnits) // enable highlight for selectedUnits
            {
                unitCollider.GetComponentInParent<BaseUnit>().SelectUnit();
            }


            previouslyHighlightedUnits = previousCurrentSelectedUnits.Except(selectedUnits).ToList(); // get Unit from previousUnits, except selected Units

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

                foreach (Collider2D unitCollider in selectedUnits)
                {
                    BaseUnit playerUnit = unitCollider.GetComponentInParent<BaseUnit>();
                    playerUnit.overrideTarget = true;
                    playerUnit.target = null;
                    playerUnit.target = enemyUnit.transform;
                    playerUnit.unitState = BaseUnit.UnitState.Approaching;
                }
            }
            else
            {
                foreach (Collider2D unitCollider in selectedUnits)
                {
                    BaseUnit playerUnit = unitCollider.GetComponentInParent<BaseUnit>();
                    playerUnit.overrideTarget = true;
                    playerUnit.target = clickTransform;
                    playerUnit.unitState = BaseUnit.UnitState.Moving;
                }
            }


        }


    }

    private void CheckForWinLose()
    {
        if (aliveEnemyUnits.Count <=0)
        {
            winPanel.SetActive(true);

        }
        if (gate == null)
        {
            losePanel.SetActive(true);
        }
    }

    private void DrawSelectionBox(Vector2 startMousePos, Vector2 endMousePos, Texture selectionTexture)
    {
        GUI.DrawTexture(new Rect(startMousePos.x,startMousePos.y,endMousePos.x,endMousePos.y), selectionTexture);
    }

    public static void RemoveUnitFromLists(Collider2D UnitSelectionSprite)
    {
        previousSelectedUnits.Remove(UnitSelectionSprite);
        selectedUnits.Remove(UnitSelectionSprite);
        highlightedUnits.Remove(UnitSelectionSprite);
        previousCurrentSelectedUnits.Remove(UnitSelectionSprite);
        previouslyHighlightedUnits.Remove(UnitSelectionSprite);

        Debug.Log(previousSelectedUnits.Count);
        Debug.Log(selectedUnits.Count);
        Debug.Log(highlightedUnits.Count);
        Debug.Log(previousCurrentSelectedUnits.Count);
        Debug.Log(previouslyHighlightedUnits.Count);

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
