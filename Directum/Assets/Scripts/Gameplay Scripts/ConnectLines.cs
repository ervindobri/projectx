using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectLines : MonoBehaviour
{
    private readonly int mapLength=5;
    private readonly int mapWidth =4;

	public LineRenderer line;
    
	[Header("Prefabs and materials:")]
	public Material material;
	public GameObject particleSystemObject;
	public GameObject spriteGlow;

	private Vector3 mousePos;
	private uint currentLines = 0; // number of current lines
    private uint currentPoints = 0; // number of current points
    public GameObject point;
    public GameObject[] allPoints;

	public GameObject currentPoint;
	public GameObject nextPoint;

	Color white = Color.white;
    Color red = new Color(1.0f, 0, 0.2282262f, 1f);


	public bool isMyTurn = false;
	public bool deadEnd;
	public Client client;
	public bool lineCreated;

	public static ConnectLines Instance { set; get; }
	void Awake()
    {
		Instance = this;
		particleSystemObject = GameObject.Find("Particle System");
		drawPoints();
		allPoints = GameObject.FindGameObjectsWithTag("Point");		
		currentPoint = allPoints[49];
		//Drawing map lines and displaying the possible moves for the current point 
        drawMapLines();
        displayAllPossibleMoves(currentPoint); // displays all the possible moves

		//Reaching the actual gameclient
		client = FindObjectOfType<Client>();
		if (client.isHost == "host")
		{
			isMyTurn = true;
		}
	}
    private void Update()
	{
		CheckForUselessObjects();
	}

	private void CheckForUselessObjects()
	{
		if (GameObject.Find("New Game Object") != null )
		{
			GameObject go = GameObject.Find("New Game Object").gameObject;
			Destroy(go);
		}
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
		currentPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;
		currentPoint = nextPoint;
		displayAllPossibleMoves(nextPoint);
		lineCreated = true;
		StartCoroutine(ResetLC(1f));
	}
	IEnumerator ResetLC(float delay)
	{
		yield return new WaitForSeconds(delay);
		lineCreated = false;
	}
    void createLine()
	{
		line = new GameObject("Line" + currentLines).AddComponent<LineRenderer>();
		line.transform.parent = GameObject.Find("Lines").transform;
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
			if (allPoints[i].transform.childCount > 0)
			{
				foreach (Transform child in allPoints[i].transform)
				{
					//child.parent = null;
					Destroy(child.gameObject);
				}
			}
			float dist = Vector3.Distance(currentPoint.transform.position, allPoints[i].GetComponent<CircleCollider2D>().transform.position);          
			if ( dist <= 1.42  && dist != 0 && !isLineBetweenTwoPoints(currentPoint,allPoints[i])) // 1.42 because distance is  ~ 1 * square 2
			{
                //Debug.Log("rose");
				allPoints[i].GetComponent<SpriteRenderer>().color = red;
				Instantiate(spriteGlow, allPoints[i].transform);
				spriteGlow.transform.position = new Vector3(0, 0, 0);
				++possibleStepcounter;
			}
			else
			{
				//Debug.Log("white");
				
				allPoints[i].GetComponent<SpriteRenderer>().color = white;
				point.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;
			}
		}
		currentPoint.GetComponent<SpriteRenderer>().color = new Color(0.3867925f, 0.3867925f, 0.3867925f);
		currentPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/selectedpointocska", typeof(Sprite)) as Sprite;
		particleSystemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPoint.transform.position.x, currentPoint.transform.position.y);
		if (possibleStepcounter == 0  )
        {
			// The current player has gone to a point that has no possible moves -> lose
			// The other player won
			deadEnd = true;
			Debug.Log("Dead end");
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

    public bool IsAnotherLineFromThisPoint(GameObject point)
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
	//public bool isWin(GameObject point)
	//{
	//    if((point.GetComponent<CircleCollider2D>().transform.position.x == mapLength+1) || (point.GetComponent<CircleCollider2D>().transform.position.x == -mapLength-1))
	//    {
	//        return true;
	//    }
	//    else
	//    {
	//        return false;
	//    }
	//}

	public bool isWin(GameObject point)
	{
		if (client.isHost == "host")
		{
			if ((point.GetComponent<CircleCollider2D>().transform.position.x == mapLength + 1))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if ((point.GetComponent<CircleCollider2D>().transform.position.x == -mapLength - 1))
			{
				return true;
			}
			else
			{
				return false;
			}
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
		pointCoordinates2.x = -mapLength - 1;
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
		currentLines++;

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
		Vector3 pointCoordinate = new Vector3(x, y, 0);
        point = new GameObject("Circle" + currentPoints);
		point.transform.parent = GameObject.Find("Points").transform;
		point.tag = "Point";
        point.AddComponent<PointLife>();
        point.AddComponent<SpriteRenderer>();
        point.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;
        point.GetComponent<SpriteRenderer>().sortingLayerName = "Points";
		point.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Sprites/Default"));

		point.GetComponent<SpriteRenderer>().sortingOrder = 1;
        point.transform.position = pointCoordinate;
        point.transform.localScale = new Vector3(0.25f, 0.25f,0.25f);
        point.AddComponent<CircleCollider2D>();
        point.AddComponent<Animator>();
		point.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("vibrating", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
		point.GetComponent<Animator>().enabled = false;

		point.AddComponent<AudioSource>().clip = Resources.Load("Audio/uh", typeof(AudioClip)) as AudioClip;
		point.GetComponent<AudioSource>().playOnAwake = false;
		point.GetComponent<AudioSource>().volume = 0.33f;

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
	public void drawMove(int x, int y)
	{
		for (int i = 0; i < allPoints.Length; i++)
		{
			if (allPoints[i].GetComponent<CircleCollider2D>().transform.position.x == x && allPoints[i].GetComponent<CircleCollider2D>().transform.position.y == y)
			{
				nextPoint = allPoints[i];
			}
		}
		if (nextPoint != currentPoint)
		{
			drawLines();
			if (!IsAnotherLineFromThisPoint(nextPoint))
			{
				isMyTurn = true;
			}
		}
	}
}


