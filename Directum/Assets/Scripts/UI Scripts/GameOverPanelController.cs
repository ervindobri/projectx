using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
	public bool gameWon;
	private bool busy;
	private AudioSource audioSource;

	public static GameOverPanelController Instance { set; get; }

	//private Text gameTimerText;
	private Text finalTimerText;
	private Text winnerText;

	private Client client;
	void Start()
    {
		busy = false;
		Instance = this;
		try
		{
			//gameTimerText = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<Text>();
			finalTimerText = GameObject.FindGameObjectWithTag("FinalTimer").GetComponent<Text>();
			winnerText = GameObject.FindGameObjectWithTag("Winner").GetComponent<Text>();
			audioSource = gameObject.GetComponent<AudioSource>();
		}
		catch (System.Exception e)
		{

			Debug.Log("Start: " + e.Message);
		}
		client = FindObjectOfType<Client>();
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log(busy);
		if (gameWon)
		{
			// If GameOverPanel is active play clip, stop all timers
			if (!busy)
			{
				gameObject.GetComponent<Canvas>().sortingLayerName = "GameOver";
				gameObject.GetComponent<Animator>().enabled = true;

				AudioClip ac = Resources.Load("Audio/WINNER", typeof(AudioClip)) as AudioClip;
				audioSource.PlayOneShot(ac);
				GameTimer.isTicking = false;
				GameTimer.audioSource.volume = 0f;

				//Stop all timers
				ManageTimers.Instance.moveTimers[0].isTicking = false;
				ManageTimers.Instance.moveTimers[1].isTicking = false;
				// Set winner name and time
				winnerText.text = ManageTimers.Instance.currentMovingPlayerName + " won";
				Debug.Log("ft: " + finalTimerText.text + " gt: " + GameTimer.Instance.timePassed.ToString("#0.00"));
				try
				{
					finalTimerText.text = GameTimer.Instance.timePassed.ToString("#0.00");
				}
				catch (System.Exception)
				{
					throw;
				}
				busy = true;
			}

		}
		if (ConnectLines.Instance.deadEnd)
		{
			if (!busy)
			{
				//Debug.Log("Deadend" + Time.deltaTime);
				gameObject.GetComponent<Canvas>().sortingLayerName = "GameOver";
				gameObject.GetComponent<Animator>().enabled = true;

				AudioClip ac = Resources.Load("Audio/LOSER", typeof(AudioClip)) as AudioClip;
				audioSource.PlayOneShot(ac);
				GameTimer.isTicking = false;
				GameTimer.audioSource.volume = 0f;
				//Stop all timers
				ManageTimers.Instance.moveTimers[0].isTicking = false;
				ManageTimers.Instance.moveTimers[1].isTicking = false;
				// Set winner name and final time
				string winnerName = ManageTimers.Instance.currentMovingPlayerName;
				foreach (GameClient p in client.players)
				{
					if (p.playerName != ManageTimers.Instance.currentMovingPlayerName)
					{
						winnerName = p.playerName;
					}
				}
				winnerText.text = winnerName + " won";
				Debug.Log(/*"ft: " + finalTimerText.text +*/ " gt: " + GameTimer.Instance.timePassed.ToString("#0.00"));
				try
				{
					finalTimerText.text = GameTimer.Instance.timePassed.ToString("#0.00");
				}
				catch (System.Exception)
				{
					throw;
				}
				busy = true;
			}
		}
	}
}
