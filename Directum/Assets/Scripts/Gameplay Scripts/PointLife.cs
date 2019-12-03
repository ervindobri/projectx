using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLife : MonoBehaviour
{

	private SpriteRenderer pointSprite;
	private GameObject countDownPanel;
	private GameObject pausePanel;
	private GameObject point;
	//When mouse is over a point and clicks , it selects it
	//making it the start point of the line
	ConnectLines connectLines;
	public bool wasSelected;

	Collider2D pointCollider;
	private Vector3 linePos;
	private bool lineCreated;
	private bool busy;

	public ParticleSystem particleSystem;

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
		countDownPanel = GameObject.Find("CountdownPanel");
		pausePanel = GameObject.Find("PausePanel");
		point = this.gameObject;
		particleSystem = GameObject.Find("Particle System").GetComponent<ParticleSystem>();
	}
	
	private void OnMouseDown()
	{
		if (!countDownPanel.activeInHierarchy && pausePanel.GetComponent<Canvas>().sortingLayerName != "Pause" )
		{
			if (!connectLines.isMyTurn)
			{
				Debug.Log("Wait for your turn!");
				return;
			}
			particleSystem.Play();
			connectLines.nextPoint = point;
			Color rose = new Color(1.0f, 0, 0.2282262f, 1f);
			if (connectLines.nextPoint.GetComponent<SpriteRenderer>().color == rose)
			{
				wasSelected = true;
				//Play sound:
				point.GetComponent<AudioSource>().Play();
				if (connectLines.isWin(point))
				{
					//Stop timer, display panel
					//Send to server that this player won -> clients display panel
					ConnectLines.gameWon = true;
					return;
				}
				else
				{
					// Game is continuing
				}
				connectLines.drawLines();
				string msg = "serverclientmove" + point.GetComponent<CircleCollider2D>().transform.position.x + "|"
							+ point.GetComponent<CircleCollider2D>().transform.position.y;
				Debug.Log(connectLines.client.clientName + ": " + msg);
				connectLines.client.Send(msg);
				if (!connectLines.isAnotherLineFromThisPoint(point))
				{
					connectLines.isMyTurn = false;
					//Debug.Log(connectLines.isMyTurn);
				}
		}
			else
			{
				;
			}
		}
	}
}
