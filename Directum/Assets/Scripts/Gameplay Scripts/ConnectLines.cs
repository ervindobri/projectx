using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectLines : MonoBehaviour
{
	private readonly int mapLength = 5;
	private readonly int mapWidth = 4;
	private readonly int goalLineBottomLimit = -1;
	private readonly int goalLineUpperLimit = 1;
	public LineRenderer line;

	[Header("Prefabs and materials:")]
	public Material material;
	public GameObject particleSystemObject;
	public GameObject spriteGlow;

	//private Vector3 mousePos;
	private uint currentLines = 0; // number of current lines
	private uint currentPoints = 0; // number of current points
	public GameObject point;
	public GameObject[] allPoints;

	public GameObject currentPoint;
	public GameObject nextPoint;

	Color white = Color.white;
	Color red = new Color(1.0f, 0, 0.2282262f, 1f);
	Color grey = new Color(0.3867925f, 0.3867925f, 0.3867925f);

	public bool isMyTurn = false;
	public bool deadEnd;
	public Client client;
	public bool lineCreated;

	public static ConnectLines Instance { set; get; }
	void Awake()
	{
		Instance = this;
		particleSystemObject = GameObject.Find("Particle System");
		DrawPoints();
		allPoints = GameObject.FindGameObjectsWithTag("Point");
		currentPoint = allPoints[49];
		//Drawing map lines and displaying the possible moves for the current point 
		DrawMapLines();

		//Reaching the actual gameclient
		client = FindObjectOfType<Client>();
		if (client.isHost == "host")
		{
			isMyTurn = true;
		}

		DisplayAllPossibleMoves(currentPoint); // displays all the possible moves
	}
	private void Update()
	{
		CheckForUselessObjects();
	}

	private void CheckForUselessObjects()
	{
		if (GameObject.Find("New Game Object") != null)
		{
			GameObject go = GameObject.Find("New Game Object").gameObject;
			Destroy(go);
		}
	}
	public void DrawLines()
	{
		Vector3 mousePos1, mousePos2;
		mousePos1 = currentPoint.transform.position;
		mousePos1.z = 0;
		mousePos2 = nextPoint.transform.position;
		mousePos2.z = 0; //2D
		CreateLine(mousePos1, mousePos2);
		currentPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;
		currentPoint = nextPoint;
		DisplayAllPossibleMoves(nextPoint);
		lineCreated = true;
		StartCoroutine(ResetLC(1f));
	}
	IEnumerator ResetLC(float delay)
	{
		yield return new WaitForSeconds(delay);
		lineCreated = false;
	}
	private int CreateLine(Vector3 startPosition, Vector3 endPosition, int sortingOrder = default(int), string sortingLayer = "Lines")
	{
		if (line == null)
		{
			line = new GameObject("Line" + currentLines).AddComponent<LineRenderer>();
			line.transform.parent = GameObject.Find("Lines").transform;
			line.material = material;
			line.sortingLayerName = sortingLayer;
			line.tag = "Line";
			line.positionCount = 2;
			line.startWidth = 0.15f;
			line.endWidth = 0.15f;
			line.useWorldSpace = false;
			line.numCapVertices = 50; //rounded corners
			line.SetPosition(0, startPosition);
			line.SetPosition(1, endPosition);
			line.sortingOrder = sortingOrder;
			line = null;
			currentLines++;
			return 1;
		}
		else
		{
			return -1;
		}
	}
	//Check where we can move from this current point -> color available points(red) and color current point(black)
	public void DisplayAllPossibleMoves(GameObject currentPoint)
	{
		// TODO: fix players available moves by color
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
			if (dist <= 1.42 && dist != 0 && !IsLineBetweenTwoPoints(currentPoint, allPoints[i])) // 1.42 because distance is  ~ 1 * square 2
			{
				//Debug.Log("rose");
				allPoints[i].GetComponent<SpriteRenderer>().color = red;
				Instantiate(spriteGlow, allPoints[i].transform);
				spriteGlow.transform.position = Vector3.zero;
				++possibleStepcounter;
			}
			else
			{
				if (allPoints[i].GetComponent<CircleCollider2D>().transform.position.x == mapLength + 1)
				{
					string[] colors = client.players[1].playerColor.Split('-');
					Color color = new Color(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]));
					allPoints[i].GetComponent<SpriteRenderer>().color = color;
					point.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;

				}

				else if (allPoints[i].GetComponent<CircleCollider2D>().transform.position.x == -mapLength - 1)
				{
					string[] colors = client.players[0].playerColor.Split('-');
					Color color = new Color(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]));
					allPoints[i].GetComponent<SpriteRenderer>().color = color;
					point.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;
				}
				//Debug.Log("white");
				else
				{
					allPoints[i].GetComponent<SpriteRenderer>().color = white;
					point.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/pointocska", typeof(Sprite)) as Sprite;
				}
			}
		}
		currentPoint.GetComponent<SpriteRenderer>().color = grey;
		currentPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/selectedpointocska", typeof(Sprite)) as Sprite;
		particleSystemObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPoint.transform.position.x, currentPoint.transform.position.y);
		if (possibleStepcounter == 0)
		{
			// The current player has gone to a point that has no possible moves -> lose
			// The other player won
			deadEnd = true;
			Debug.Log("Dead end");
		}
	}

	public bool IsLineBetweenTwoPoints(GameObject startingPoint, GameObject endingPoint)
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
	public bool IsWin(GameObject point)
	{
		if ((point.GetComponent<CircleCollider2D>().transform.position.x == mapLength + 1) || (point.GetComponent<CircleCollider2D>().transform.position.x == -mapLength - 1))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DrawMapLines()
	{
		DrawVerticalMapLines();
		DrawHorizontalMapLines();
		DrawHiddenLines();
	}
	public int NumberOfLinesFromPoint(GameObject point)
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
	public void CreatePoint(int x, int y)
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
		point.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		point.AddComponent<CircleCollider2D>();
		point.AddComponent<Animator>();
		point.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("vibrating", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
		point.GetComponent<Animator>().enabled = false;
		point.AddComponent<AudioSource>().clip = Resources.Load("Audio/uh", typeof(AudioClip)) as AudioClip;
		point.GetComponent<AudioSource>().playOnAwake = false;
		point.GetComponent<AudioSource>().volume = 0.33f;
		++currentPoints;
	}
	public void DrawPoints()
	{
		for (int i = -mapLength; i <= mapLength; ++i)
		{
			for (int j = -mapWidth; j <= mapWidth; ++j)
			{
				CreatePoint(i, j);
			}
		}
		for (int i = -1; i <= 1; ++i)
		{
			CreatePoint(-mapLength - 1, i);
			CreatePoint(mapLength + 1, i);
		}
	}
	public void DrawMove(int x, int y)
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
			DrawLines();
			if (IsWin(nextPoint))
			{
				GameOverPanelController.Instance.gameWon = true;
			}
			if (!IsAnotherLineFromThisPoint(nextPoint))
			{
				isMyTurn = true;
			}
		}
	}
	private void DrawHiddenLines()
	{
		CreateLine(new Vector3(-mapLength - 1, goalLineBottomLimit, 0), new Vector3(-mapLength, goalLineBottomLimit - 1, 0), -2, "Default");
		CreateLine(new Vector3(-mapLength, goalLineUpperLimit + 1, 0), new Vector3(-mapLength - 1, goalLineUpperLimit, 0), -2, "Default");
		CreateLine(new Vector3(mapLength, goalLineUpperLimit + 1, 0), new Vector3(mapLength + 1, goalLineUpperLimit, 0), -2, "Default");
		CreateLine(new Vector3(mapLength, goalLineBottomLimit - 1, 0), new Vector3(mapLength + 1, goalLineBottomLimit, 0), -2, "Default");
	}
	private void DrawHorizontalMapLines()
	{
		Vector3 pointCoordinates1 = new Vector3(-mapLength, -mapWidth, 0);
		Vector3 pointCoordinates2 = new Vector3(-mapLength, mapWidth, 0);
		while (pointCoordinates1.x < mapLength)
		{
			CreateLine(pointCoordinates1, new Vector3(++pointCoordinates1.x, pointCoordinates1.y, 0));
			CreateLine(pointCoordinates2, new Vector3(++pointCoordinates2.x, pointCoordinates2.y, 0));
		}
	}
	private void DrawVerticalMapLines()
	{
		Vector3 pointCoordinates1 = new Vector3(-mapLength, -mapWidth, 0);
		Vector3 pointCoordinates2 = new Vector3(mapLength, -mapWidth, 0);
		bool stop = false;
		bool stop2 = false;

		while (pointCoordinates1.y < mapWidth)
		{

			if (pointCoordinates1.y == goalLineBottomLimit && !stop)
			{
				CreateLine(pointCoordinates1, new Vector3(--pointCoordinates1.x, pointCoordinates1.y, 0));
				stop = true;
			}
			else
			{
				if (pointCoordinates1.y == goalLineUpperLimit && !stop)
				{
					CreateLine(pointCoordinates1, new Vector3(++pointCoordinates1.x, pointCoordinates1.y, 0));
					stop = true;
				}
				else
				{
					CreateLine(pointCoordinates1, new Vector3(pointCoordinates1.x, ++pointCoordinates1.y, 0));
					stop = false;
				}
			}

			if (pointCoordinates2.y == goalLineBottomLimit && !stop2)
			{
				CreateLine(pointCoordinates2, new Vector3(++pointCoordinates2.x, pointCoordinates2.y, 0));
				stop2 = true;
			}
			else
			{
				if (pointCoordinates2.y == goalLineUpperLimit && !stop2)
				{
					CreateLine(pointCoordinates2, new Vector3(--pointCoordinates2.x, pointCoordinates2.y, 0));
					stop2 = true;
				}
				else
				{
					CreateLine(pointCoordinates2, new Vector3(pointCoordinates2.x, ++pointCoordinates2.y, 0));
					stop2 = false;
				}
			}
		}
	}
}