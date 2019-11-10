using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ConnectLines : MonoBehaviour
{
    private int mapLength=5;
    private int mapWidth=4;
	public LineRenderer line;
    
	private Vector3 mousePos;
	public Material material;
	public uint numberOfPoints; // we tell him how many points there are 
	private uint currentLines = 0; // number of current lines
    private uint currentPoints = 0;
    public GameObject point;
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
        drawPoints();
		allPoints = GameObject.FindGameObjectsWithTag("Point");		
		currentPoint = allPoints[49];
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
            float dist = Vector3.Distance(currentPoint.transform.position, allPoints[i].GetComponent<CircleCollider2D>().transform.position);          
			if ( dist <= 1.42  && dist != 0 && !isLineBetweenTwoPoints(currentPoint,allPoints[i])) // 1.42 because distance is  ~ 1 * square 2
			{
                //Debug.Log("rose");
				allPoints[i].GetComponent<SpriteRenderer>().color = rose;
                ++possibleStepcounter;
			}
			else
			{
                //Debug.Log("white");
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
            if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == startingPoint.GetComponent<CircleCollider2D>().transform.position ||
                lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == endingPoint.GetComponent<CircleCollider2D>().transform.position)
            {
                if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == endingPoint.GetComponent<CircleCollider2D>().transform.position ||
                    lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == startingPoint.GetComponent<CircleCollider2D>().transform.position)
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
            if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == point.GetComponent<CircleCollider2D>().transform.position ||
                lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == point.GetComponent<CircleCollider2D>().transform.position)
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
        if((point.GetComponent<CircleCollider2D>().transform.position.x == mapLength+1) || (point.GetComponent<CircleCollider2D>().transform.position.x == -mapLength-1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void drawMapLines()
    {
        Vector3 pointCoordinates1;
        Vector3 pointCoordinates2;
        bool stop = false;
        bool stop2 = false;
        pointCoordinates1.z = 0;
        pointCoordinates1.x = -mapLength;
        pointCoordinates1.y = -mapWidth;
        pointCoordinates2.z = 0;
        pointCoordinates2.x = mapLength;
        pointCoordinates2.y = -mapWidth;
        //Vertival limits
        while (pointCoordinates1.y < mapWidth)
        { 
            createLine();
            if (pointCoordinates1.y == -1 && !stop)
            {
                line.SetPosition(0, pointCoordinates1);
                --pointCoordinates1.x;
                line.SetPosition(1, pointCoordinates1);
                line = null;
                currentLines++;
                stop = true;
            }
            else
            {
                if (pointCoordinates1.y == 1 && !stop)
                {
                    line.SetPosition(0, pointCoordinates1);
                    ++pointCoordinates1.x;
                    line.SetPosition(1, pointCoordinates1);
                    line = null;
                    currentLines++;
                    stop = true;
                }
                else
                {
                    line.SetPosition(0, pointCoordinates1);
                    ++pointCoordinates1.y;
                    line.SetPosition(1, pointCoordinates1);
                    line = null;
                    currentLines++;
                    stop = false;
                }
            }
            createLine();
            if (pointCoordinates2.y == -1 && !stop2)
            {
                line.SetPosition(0, pointCoordinates2);
                ++pointCoordinates2.x;
                line.SetPosition(1, pointCoordinates2);
                line = null;
                currentLines++;
                stop2 = true;
            }
            else
            {
                if (pointCoordinates2.y == 1 && !stop2)
                {
                    line.SetPosition(0, pointCoordinates2);
                    --pointCoordinates2.x;
                    line.SetPosition(1, pointCoordinates2);
                    line = null;
                    currentLines++;
                    stop2 = true;
                }
                else
                {
                    line.SetPosition(0, pointCoordinates2);
                    ++pointCoordinates2.y;
                    line.SetPosition(1, pointCoordinates2);
                    line = null;
                    currentLines++;
                    stop2 = false;
                }
            }
        }
        //vertical limits
        pointCoordinates1.x = -mapLength;
        pointCoordinates1.y = -mapWidth;
        pointCoordinates2.x = -mapLength;
        pointCoordinates2.y = mapWidth;
        while (pointCoordinates1.x < mapLength)
        {
            createLine();
            line.SetPosition(0, pointCoordinates1);
            ++pointCoordinates1.x;
            line.SetPosition(1, pointCoordinates1);
            line = null;
            currentLines++;
            createLine();

            line.SetPosition(0, pointCoordinates2);
            ++pointCoordinates2.x;
            line.SetPosition(1, pointCoordinates2);
            line = null;
            currentLines++;
        }

        pointCoordinates1.x = -mapLength;
        pointCoordinates1.y = -2;
        pointCoordinates2.x = -mapLength-1;
        pointCoordinates2.y = -1;
        createLine();
        line.SetPosition(0, pointCoordinates1);
        line.SetPosition(1, pointCoordinates2);
        line.sortingLayerName = "Default";
        line.sortingOrder = -2;
        line = null;
        currentLines++;

        pointCoordinates1.x = -mapLength;
        pointCoordinates1.y = 2;
        pointCoordinates2.x = -mapLength - 1;
        pointCoordinates2.y = 1;
        createLine();
        line.SetPosition(0, pointCoordinates1);
        line.SetPosition(1, pointCoordinates2);
        line.sortingLayerName = "Default";
        line.sortingOrder = -2;
        line = null;

        pointCoordinates1.x = mapLength;
        pointCoordinates1.y = 2;
        pointCoordinates2.x = mapLength + 1;
        pointCoordinates2.y = 1;
        createLine();
        line.SetPosition(0, pointCoordinates1);
        line.SetPosition(1, pointCoordinates2);
        line.sortingLayerName = "Default";
        line.sortingOrder = -2;
        line = null;
        currentLines++;

        pointCoordinates1.x = mapLength;
        pointCoordinates1.y = -2;
        pointCoordinates2.x = mapLength + 1;
        pointCoordinates2.y = -1;
        createLine();
        line.SetPosition(0, pointCoordinates1);
        line.SetPosition(1, pointCoordinates2);
        line.sortingLayerName = "Default";
        line.sortingOrder = -2;
        line = null;
        currentLines++;
    }
    public int numberOfLinesFromPoint(GameObject point)
    {
        int counter = 0;
        GameObject[] lineObjects = GameObject.FindGameObjectsWithTag("Line");
        for (int i = 0; i < lineObjects.Length; ++i)
        {

            if (lineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == point.GetComponent<CircleCollider2D>().transform.position ||
                lineObjects[i].GetComponent<LineRenderer>().GetPosition(1) == point.GetComponent<CircleCollider2D>().transform.position)
            {
                ++counter;
            }
        }
        return counter;
    }
    public void createPoint(int x, int y)
    {
        Vector3 pointCoordinate;
        pointCoordinate.x = x;
        pointCoordinate.y = y;
        pointCoordinate.z = 0;
        point = new GameObject("Circle" + currentPoints);
        point.AddComponent<PointLife>();
        point.AddComponent<SpriteRenderer>();
        point.AddComponent<CircleCollider2D>();
        point.tag = "Point";
        point.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Circle", typeof(Sprite)) as Sprite;
        point.GetComponent<SpriteRenderer>().sortingLayerName = "Points";
        point.GetComponent<SpriteRenderer>().sortingOrder = 1;
        point.transform.position = pointCoordinate;
        point.transform.localScale = new Vector3(0.5f, 0.5f,0.5f);
        ++currentPoints;

    }
    public void drawPoints()
    {
        for (int i = -mapLength; i <= mapLength; ++i)
        {
            for (int j = -mapWidth; j <= mapWidth; ++j)
            {
                createPoint(i, j);
            }
        }
        for (int i = -1; i <= 1; ++i)
        {
            createPoint(-mapLength - 1, i);
            createPoint(mapLength + 1, i);
        }
    }
}


