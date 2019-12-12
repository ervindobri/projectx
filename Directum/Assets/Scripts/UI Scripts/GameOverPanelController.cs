using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
	public bool gameWon;
	private bool busy;
	private AudioSource audioSource;

	public static GameOverPanelController Instance { set; get; }

	private Text gameTimerText;
	private Text finalTimerText;
	private Text winnerText;

	void Start()
    {
		Instance = this;
		try
		{
			gameTimerText = GameObject.FindGameObjectWithTag("GameTimer").GetComponent<Text>();
			finalTimerText = GameObject.FindGameObjectWithTag("FinalTimer").GetComponent<Text>();
			winnerText = GameObject.FindGameObjectWithTag("Winner").GetComponent<Text>();
			audioSource = gameObject.GetComponent<AudioSource>();
		}
		catch (System.Exception e)
		{

			Debug.Log("Start: " + e.Message);
		}
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log("ft: " + finalTimerText.text + " gt: " + gameTimerText.text);
		if (gameWon)
		{
			// If GameOverPanel is active play clip, stop all timers
			if (!busy)
			{
				Debug.Log("Wnner" + Time.deltaTime);
				gameObject.GetComponent<Canvas>().sortingLayerName = "GameOver";
				gameObject.GetComponent<Animator>().enabled = true;

				AudioClip ac = Resources.Load("Audio/WINNER", typeof(AudioClip)) as AudioClip;
				audioSource.PlayOneShot(ac);
				GameTimer.isTicking = false;
				GameTimer.audioSource.volume = 0f;

				//Stop all timers
				ManageTimers.Instance.moveTimers[0].isTicking = false;
				ManageTimers.Instance.moveTimers[1].isTicking = false;
				Debug.Log("Timers stopped!");
				// Set winner name and time
				try
				{
					winnerText.text = ManageTimers.Instance.currentMovingPlayerName + " won";
					finalTimerText.text = gameTimerText.text;

				}
				catch (System.Exception e)
				{

					Debug.Log("Exception: " + e.Message);
				}
				busy = true;
			}

		}
		else if (ConnectLines.Instance.deadEnd)
		{
			if (!busy)
			{
				Debug.Log("Deadend" + Time.deltaTime);
				gameObject.GetComponent<Canvas>().sortingLayerName = "GameOver";
				gameObject.GetComponent<Animator>().enabled = true;

				AudioClip ac = Resources.Load("Audio/WINNER", typeof(AudioClip)) as AudioClip;
				audioSource.PlayOneShot(ac);
				GameTimer.isTicking = false;
				GameTimer.audioSource.volume = 0f;
				// Set winner name and time
				try
				{
					winnerText.text = ManageTimers.Instance.currentMovingPlayerName + " won";
					finalTimerText.text = gameTimerText.text;

				}
				catch (System.Exception e)
				{

					Debug.Log("Exception: " + e.Message);
				}
				busy = true;
			}
		}
	}
}
