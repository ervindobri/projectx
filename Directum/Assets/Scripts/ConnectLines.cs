using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ConnectLines : MonoBehaviour
{
	public LineRenderer line;
	private Vector3 mousePos;
	public Material material;
	public uint numberOfPoints; // we tell him how many points there are 
	private uint currentLines = 0; // number of current lines


	
	// Store the points in an array
	public GameObject[] allPoints;
	public SpriteRenderer[] allPointsSprite;
	public GameObject currentPoint;
	PointLife pointController;



	private int currentIndex;


	public List<GameObject> selectedPoints;
	private bool hasLine;
	public GameObject[] renderedLineObjects;
	void Awake()
    {
		//Find all points initially
		allPoints = GameObject.FindGameObjectsWithTag("Point");
		selectedPoints = new List<GameObject>();
		selectedPoints.Add(allPoints[0]);

		currentIndex = 0;
		//Set the current point -> the startPosition of the first line / THE FIRST POINT
		currentPoint = allPoints[0];
		if (currentPoint != null )
		{
			// Get the controller of the current point
			pointController = currentPoint.GetComponent<PointLife>();
		}
		else
		{
			Debug.Log("Could not find 'PointLife' script...");
		}
		//createLine();
	}
	private void Update()
	{
		//Check for duplicate lines, ifthere are any,destroy them.
		destroyDuplicateLines();

		// Check available moves for the the CURRENT point
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		// Check which point are we now from array:
		for (int i = 0; i < allPoints.Length; i++)
		{
			if ( currentPoint.name  == allPoints[i].name)
			{
				currentIndex = i;
			}
		}
		// If two points were selected -> Click to draw line - from pointA to pointB
		if ( selectedPoints[0] != null )
		{
			checkAvailableMoves(allPoints[currentIndex]); // displays all the possible moves
			// Check if there were points selected:
			checkSelected();

			//Debug.Log("Start Point:" + selectedPoints[0] + "End point:" + selectedPoints[1]);
			if (selectedPoints.Count > 1  && selectedPoints[1] != null)
			{
				float dist = Vector3.Distance(selectedPoints[0].transform.position, mousePos);
				//Debug.Log("Start point - mousePos distance is: " + dist);
				if ( dist <= 1.42f )
				{

					//Debug.Log("Line: "+line);
					if (line == null)
					{
						createLine();

					}
					mousePos = selectedPoints[0].transform.position;
					mousePos.z = 0;
					line.SetPosition(0, mousePos);
					line.SetPosition(1, mousePos);


					mousePos = selectedPoints[1].transform.position;
					mousePos.z = 0; //2D
					line.SetPosition(1, mousePos);
					line = null;
					currentLines++;
					selectedPoints[0] = selectedPoints[1];
					selectedPoints[1] = currentPoint;

					// Closest point will be current point
					//selectedPoints[1] = currentPoint;

				}
				else
				{
					Debug.Log("Please select another point!");
				}
			}
		}
	}

	// Check if two points were selected
	void checkSelected()
	{
		for (int i = 0; i < allPoints.Length; i++)
		{
			if ( allPoints[i].GetComponent<PointLife>().wasSelected )
			{
				if (selectedPoints.Count >= 2 && selectedPoints[1] != null)
				{
					break;
				}
				else
				{
					Debug.Log(allPoints[i].name + " selected");
					selectedPoints.Add(allPoints[i]);
				}
			}
		}
	}
	// If there was a line created already -> destroy the others
	void destroyDuplicateLines()
	{
		renderedLineObjects = GameObject.FindGameObjectsWithTag("Line");
		for (int i = 0; i < renderedLineObjects.Length; i++)
		{
			if (renderedLineObjects[i].GetComponent<LineRenderer>().GetPosition(0) == renderedLineObjects[i].GetComponent<LineRenderer>().GetPosition(1))
			{
				Destroy(renderedLineObjects[i]);
			}
		}

	}
	// Function name describes all. -> Creates a new LineRenderer object
	void createLine()
	{
		line = new GameObject("Line" + currentLines).AddComponent<LineRenderer>();
		line.material = material;
		line.sortingLayerName = "Line";
		line.tag = "Line";
		line.positionCount = 2;
		line.startWidth = 0.15f;
		line.endWidth = 0.15f;
		line.useWorldSpace = false;
		line.numCapVertices = 50; //rounded corners
	}

	//Check where we can move from this current point -> color available points(red) and color current point(black)
	void checkAvailableMoves(GameObject currentPoint)
	{
		Color white = new Color(1, 1, 1, 1f);
		Color black = new Color(0, 0, 0, 1f);
		Color rose = new Color(0.7264151f, 0.2775454f, 0.2775454f, 1f);
		for (int i = 0; i < allPoints.Length; i++)
		{
			float dist = Vector3.Distance(currentPoint.GetComponent<CircleCollider2D>().bounds.center, allPoints[i].GetComponent<CircleCollider2D>().bounds.center);
			//Debug.Log("Distance is: " + dist);
			if ( dist <= 1.42  && dist != 0 ) // 1.42 because distance is  ~ 1 * square 2
			{


				allPoints[i].GetComponent<SpriteRenderer>().color = rose;
				//Debug.Log("");
			}
			else
			{
				allPoints[i].GetComponent<SpriteRenderer>().color = white;
				currentPoint.GetComponent<SpriteRenderer>().color = black;
				//canDrawLine = false;
			}

		}
	}
}
