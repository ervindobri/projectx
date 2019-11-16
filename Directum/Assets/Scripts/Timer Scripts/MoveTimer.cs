using UnityEngine;
using UnityEngine.UI;

public class MoveTimer : MonoBehaviour
{
	public float timeLeft;
	private float timePassed;
	public float RemainingSeconds;

	private string thisPlayerPanelName;
	private GameObject countDownPanel;
	public bool alreadyMoved;
	public bool isTicking;
	private Text moveTimerText;
	private GameObject thisPlayerPanel;
	private GameObject thisMoveTimer;
	private void Start()
	{
		countDownPanel = GameObject.Find("CountdownPanel");

		thisPlayerPanel = this.gameObject;
		thisPlayerPanelName = thisPlayerPanel.transform.Find("Text").GetComponent<Text>().text;

		thisMoveTimer = thisPlayerPanel.transform.Find("MoveTimer").gameObject;
		moveTimerText = thisMoveTimer.GetComponent<Text>();
		timeLeft = 30f;
		RemainingSeconds = timeLeft;
		moveTimerText.text = RemainingSeconds.ToString("#0");
		alreadyMoved = false;

		//Debug.Log(thisPlayerPanelName + "," + thisMoveTimer);
	}
	private void Update()
	{
		if ( !countDownPanel.activeSelf)
		{
			// check if this player has to move or not -> GameplayManager tells which player has to move
			//Debug.Log(thisPlayerPanelName + " and " + GameplayManager.movingPlayerName);
			//Debug.Log(thisMoveTimer + " and " + GameplayManager.currentMoveTimer);
			if ( thisPlayerPanel== GameplayManager.currentMovingPlayer && thisMoveTimer == GameplayManager.currentMoveTimer )
			{
				//Debug.Log(thisPlayerPanelName + "-" +  thisMoveTimer);	
				if ( isTicking )
				{
					//Debug.Log("Your time to move: " + thisPlayerPanelName);
					// if this player has to move -> start timer
					timePassed += Time.deltaTime;
					RemainingSeconds = timeLeft - timePassed;
					moveTimerText.text= RemainingSeconds.ToString("#0");
					//Debug.Log(RemainingSeconds);
					//He can still move
					if (RemainingSeconds > 0 )
					{
						//Now It's the time to move!!
						if (!alreadyMoved && Input.GetMouseButtonDown(1))
						{
							//Debug.Log("I moved!");
							timePassed = 0;
							RemainingSeconds = timeLeft;
							moveTimerText.text = RemainingSeconds.ToString("#0");
							alreadyMoved = true;
						}
					}
					//Time out!
					else
					{
						alreadyMoved = false;
						Debug.Log("Your time ran out! Next player has to move");
						return;
					}
				}
			}
			else
			{
				//The other player has to move!
				//Debug.Log("The other player has to move!");
				return;
			}
		}
	}
}
