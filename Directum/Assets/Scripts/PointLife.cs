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
		wasSelected = true;
		//Debug.Log(gameObject.name + ": "+ wasSelected);

		pointSprite.color = new Color(0f, 0f, 0f, 1f);

		// This point becomes the current point from which we have to make a line
		connectLines.currentPoint = point;
	}
}
