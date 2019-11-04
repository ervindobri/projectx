using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ConnectLines : MonoBehaviour
{
    private int mapLength=12;
    private int mapWidth=8;
	public LineRenderer line;
	private Vector3 mousePos;
	public Material material;
	public uint numberOfPoints; // we tell him how many points there are 
	private uint currentLines = 0; // number of current lines
    public GameObject[] allPoints;
	public SpriteRenderer[] allPointsSprite;
	public GameObject currentPoint;
    public GameObject nextPoint;
    PointLife pointController;
	public GameObject[] renderedLineObjects;
    Color white = new Color(1, 1, 1, 1f);
    Color black = new Color(0, 0, 0, 1f);
    Color rose = new Color(0.7264151f, 0.2775454f, 0.2775454f, 1f);
    int myScore = 0;
    //int enemyScore=0;
    void Awake()
    {
		allPoints = GameObject.FindGameObjectsWithTag("Point");		
		currentPoint = allPoints[0];
		if (currentPoint != null )
		{
			pointController = currentPoint.GetComponent<PointLife>();
		}
		else
		{
		Debug.Log("Could not find 'PointLife' script...");
		}
        drawMapLines();
        displayAllPossibleMoves(currentPoint); // displays all the possible moves
    }
    private void Update()
	{		
	}
    public void drawLines()
    {
        //Debug.Log("aktualis pont:");
        // Debug.Log(currentPoint);
        // Debug.Log("kovetkezo pont:");
        // Debug.Log(nextPoint);              
        if (line == null)
        {
            createLine();
        }
        mousePos = currentPoint.transform.position;
        mousePos.z = 0;
        line.SetPosition(0, mousePos);
        mousePos = nextPoint.transform.position;
        mousePos.z = 0; //2D
        line.SetPosition(1, mousePos);
        line = null;
        currentLines++;
        currentPoint = nextPoint;
        displayAllPossibleMoves(nextPoint);      
    }   
    void createLine()
	{
		line = new GameObject("Line" + currentLines).AddComponent<LineRenderer>();
		line.material = material;
		line.sortingLayerName = "Lines";
		line.tag = "Line";
		line.positionCount = 2;
		line.startWidth = 0.15f;
		line.endWidth = 0.15f;
		line.useWorldSpace = false;
		line.numCapVertices = 50; //rounded corners
	}

	//Check where we can move from this current point -> color available points(red) and color current point(black)
	public void displayAllPossibleMoves(GameObject currentPoint)
	{
        int possibleStepcounter = 0;
        for (int i = 0; i < allPoints.Length; i++)
		{
			float dist = Vector3.Distance(currentPoint.GetComponent<CircleCollider2D>().bounds.center, allPoints[i].GetComponent<CircleCollider2D>().bounds.center);
			//Debug.Log("Distance is: " + dist);
			if ( dist <= 1.42  && dist != 0 && !isLineBetweenTwoPoints(currentPoint,allPoints[i])) // 1.42 because distance is  ~ 1 * square 2
			{
				allPoints[i].GetComponent<SpriteRenderer>().color = rose;
                ++possibleStepcounter;
			}
			else
			{
                allPoints[i].GetComponent<SpriteRenderer>().color = white;
			}
		}
        currentPoint.GetComponent<SpriteRenderer>().color = black;
        if (possibleStepcounter == 0)
        {
            Debug.Log("DeadEnd=Lose");
        }
    }

    public bool isLineBetweenTwoPoints(GameObject startingPoint, GameObject endingPoint)
    {
        //Debug.Log("In isThereline");
        GameObject[] lineObjects = GameObject.FindGameObjectsWithTag("Line");
        for (int i = 0; i < lineObjects.Length; ++i)
        {
            if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == startingPoint.GetComponent<CircleCollider2D>().bounds.center ||
                lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == endingPoint.GetComponent<CircleCollider2D>().bounds.center)
            {
                if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == endingPoint.GetComponent<CircleCollider2D>().bounds.center ||
                    lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == startingPoint.GetComponent<CircleCollider2D>().bounds.center)
                {
                    //Debug.Log("out isThereline true");
                    return true;
                }
                else
                {
                    ;
                }
            }
            else
            {
                ;
            }
        }
        //Debug.Log("out isThereline false");
        return false;
    }

    public bool isAnotherLineFromThisPoint(GameObject point)
    {
        int counter = 0;
        GameObject[] lineObjects = GameObject.FindGameObjectsWithTag("Line");
        for (int i = 0; i < lineObjects.Length; ++i)
        {
            if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == point.GetComponent<CircleCollider2D>().bounds.center ||
                lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == point.GetComponent<CircleCollider2D>().bounds.center)
            {
                ++counter;
                if (counter == 2)
                {
                    return true;
                }
                else
                {
                    ;
                }
            }
            else
            {
                ;
            }
        }
        //Debug.Log("out isThereline false");
        return false;
    }
    public bool isWin(GameObject point)
    {
        if((point.GetComponent<CircleCollider2D>().bounds.center.x == 7) || (point.GetComponent<CircleCollider2D>().bounds.center.x == -7))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isLose(GameObject point)
    {
        if (isDeadEnd(point))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isDeadEnd(GameObject point)
    {
        if (point.name == "circle (122)" || point.name == "circle (119)" || point.name == "circle (83)" || point.name == "circle (80)")
        {
            return true;
        }
        else
        {
            if ((point.GetComponent<CircleCollider2D>().bounds.center.y == mapWidth || point.GetComponent<CircleCollider2D>().bounds.center.y == -mapWidth ||
                point.GetComponent<CircleCollider2D>().bounds.center.y == 14 || point.GetComponent<CircleCollider2D>().bounds.center.y == -10)
                && (numberOfLinesFromPoint(point) == 5))
            {
                return true;
            }
            else
            {
                if (numberOfLinesFromPoint(point) == 8)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void drawMapLines()
    {
        Vector3 pointCoordinates;
        pointCoordinates.z = 0;
        pointCoordinates.y = -mapWidth;
        //Debug.Log("keret");
        for (int i = -mapWidth; i < mapWidth - 1; i+=2) {
            if (line == null)
            {
                Debug.Log("linecreated");
                createLine();
            }
            if (Mathf.Abs(i) < mapWidth / 2)
            {
                pointCoordinates.x = -12;
            }
            else
            {
                pointCoordinates.x = -10;
            }
            Debug.Log("keret");
            line.SetPosition(0, pointCoordinates);
            pointCoordinates.y = i+2;
            line.SetPosition(1, pointCoordinates);
            line = null;
            currentLines++;
            //if (line == null)
            //{
            //    createLine();
            //}
            //if (i < mapWidth / 2)
            //{
            //    pointCoordinates.x = 16;
            //}
            //else
            //{
            //    pointCoordinates.x = 14;
            //}
            //pointCoordinates.y-=2;
            //line.SetPosition(0, pointCoordinates);
            //pointCoordinates.y+=2;
            //line.SetPosition(1, pointCoordinates);
            //line = null;
            //currentLines++;

        }
    }
    public int numberOfLinesFromPoint(GameObject point)
    {
        int counter = 0;
        GameObject[] lineObjects = GameObject.FindGameObjectsWithTag("Line");
        for (int i = 0; i < lineObjects.Length; ++i)
        {

            if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == point.GetComponent<CircleCollider2D>().bounds.center ||
                lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == point.GetComponent<CircleCollider2D>().bounds.center)
            {
                ++counter;
            }
        }
        return counter;
    }

}


