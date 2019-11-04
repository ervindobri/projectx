using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLife : MonoBehaviour
{

	private SpriteRenderer pointSprite;
	private GameObject point;
	//When mouse is over a point and clicks , it selects it
	//making it the start point of the line
	ConnectLines connectLines;
	public bool wasSelected;

	Collider2D pointCollider;
	private Vector3 linePos;

	private void Start()
	{
		pointCollider = GetComponent<Collider2D>();
		pointSprite = GetComponent<SpriteRenderer>();
		GameObject connectLinesObject = GameObject.FindWithTag("MainCamera");
		if ( connectLinesObject != null)
		{
			connectLines = connectLinesObject.GetComponent<ConnectLines>();
		}
		if (connectLinesObject == null)
		{
			Debug.Log("Could not find 'ConnectLines' script...");
		}
		point = this.gameObject;
	}
	private void OnMouseDown()
	{
        connectLines.nextPoint = point;
        Color rose = new Color(0.7264151f, 0.2775454f, 0.2775454f, 1f);
        if (connectLines.nextPoint.GetComponent<SpriteRenderer>().color == rose)
        {
            wasSelected = true;
            if (connectLines.isWin(point))
            {
                Debug.Log("Winner");
            }
            else
            {
                //if (connectLines.isLose(point))
                //{
                //    Debug.Log("Looser");
                //}
                //else
                //{

                //}
                //Debug.Log("No win");
            }
            connectLines.drawLines();
        }
        else
        {
            ;
        }
        //serverCommunication();
        //processingRecievedDataFromServer();
	}
}
