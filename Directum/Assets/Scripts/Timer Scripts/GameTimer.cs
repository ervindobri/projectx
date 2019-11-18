using UnityEngine;
using UnityEngine.UI;


public class GameTimer : MonoBehaviour
{
	// Static Instance of this Timer (since you seem to want a single Timer)
	private static GameTimer instance;

	private float startTime; // sets how much time the player has to start with
	private float timePassed;
	private bool stopTimer; 


	public float RemainingSeconds;

	[Header("Timer sound")]
	private AudioClip audioClip;
	public static AudioSource audioSource;

	public static bool isTicking;
	private Text timerText;
	private GameObject countDownPanel;
	private bool soundOn;

	// Use this for initialization
	void Awake()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
		countDownPanel = GameObject.Find("CountdownPanel");
		timerText = GameObject.Find("Timer").GetComponent<Text>();
		// Another timer exists, kill this one
		if ( instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			// assign Singleton
			instance = this;
		}

		// Init Timer
		startTime = 120f;
		timePassed = 0f;
		timerText.text = timePassed.ToString("#0.00");
		RemainingSeconds = startTime;
		isTicking = true;
		audioSource.loop = true;
		soundOn = true;
	}

	//Update is called once per frame
	void Update()
	{

		if (isTicking && !countDownPanel.activeSelf )// already a bool no check for true needed
		{
			if ( soundOn )
			{
				audioSource.Play();
				soundOn = false;
			}
			// add frame time to passed time
			timePassed += Time.deltaTime;
			timerText.text = timePassed.ToString("#0.00");
			RemainingSeconds = startTime - timePassed;

			// Clamp time to start time
			if (RemainingSeconds >= startTime)
			{
				RemainingSeconds = startTime;
			}

			// No time left
			if (RemainingSeconds <= 0)
			{
				// This is Game Over
				RemainingSeconds = 0;
				stopTimer = true;
				return;
			}
		}
	}

	// Use this Method to access your timer
	public static GameTimer Get()
	{
		// failsafe
		if (instance = null)
		{
			GameObject go = new GameObject();
			instance = go.AddComponent<GameTimer>();
		}
		return instance;
	}
	public void ResetTimer()
	{
		timePassed = 0;
	}
}
