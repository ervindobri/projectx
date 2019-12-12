using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveTimer : MonoBehaviour
{
	public float timeLeft;
	private float timePassed;
	public float RemainingSeconds;

	private GameObject countDownPanel;
	//public bool alreadyMoved;
	public bool isTicking;
	private Text moveTimerText;
	private GameObject thisPlayerPanel;
	private GameObject thisMoveTimer;
	public bool resetTrigger;
	private bool busy;

	private void Start()
	{
		countDownPanel = GameObject.Find("CountdownPanel");
		thisPlayerPanel = this.gameObject;
		thisMoveTimer = thisPlayerPanel.transform.Find("MoveTimer").gameObject;
		moveTimerText = thisMoveTimer.GetComponent<Text>();
		timeLeft = 30f;
		RemainingSeconds = timeLeft;
		moveTimerText.text = RemainingSeconds.ToString("#0");
		//alreadyMoved = false;
	}
	private void Update()
	{
		//Debug.Log(thisMoveTimer.transform.parent + " - " + alreadyMoved);
		if ( !countDownPanel.activeSelf)
		{	
			if ( isTicking )
			{
				// if this player has to move -> start timer
				timePassed += Time.deltaTime;
				RemainingSeconds = timeLeft - timePassed;
				moveTimerText.text= RemainingSeconds.ToString("#0");
				//Debug.Log(RemainingSeconds);
				//He can still move
				if (RemainingSeconds > 0 )
				{
						//Trigger will activate if player made a move
						if ( resetTrigger )
						{
							timePassed = 0;
							RemainingSeconds = timeLeft;
							moveTimerText.text = RemainingSeconds.ToString("#0");
							resetTrigger = false;
						}
						
				}
				//Time out!
				else
				{
					timePassed = 0;
					RemainingSeconds = timeLeft;
					moveTimerText.text = RemainingSeconds.ToString("#0");
					//if ( !busy )
					//{
					//	StartCoroutine(PlaySound());
					//	string msg = "serverclientmove" + ConnectLines.Instance.currentPoint.GetComponent<CircleCollider2D>().transform.position.x + "|"
					//		+ ConnectLines.Instance.currentPoint.GetComponent<CircleCollider2D>().transform.position.y;
					//	ConnectLines.Instance.client.Send(msg);
					//	ConnectLines.Instance.isMyTurn = false;
					//	busy = true;
					//}
					Debug.Log("Your time ran out! Next player has to move");
					return;
				}
			}
			else
			{
				//The other player has to move!
				Debug.Log("The other player has to move!");
				return;
			}
		}
	}
	IEnumerator PlaySound()
	{
		AudioClip reallyn = Resources.Load("Audio/reallyn", typeof(AudioClip)) as AudioClip;
		gameObject.GetComponent<AudioSource>().PlayOneShot(reallyn);
		yield return new WaitForSeconds(1f);
	}
}
