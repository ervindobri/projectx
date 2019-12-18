using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

	private AudioSource audioSource;
	[Header("Pause Sound")]
	public AudioClip audioClip;
	public static float pausedTime;
	private float startTime;
	public GameObject panelObject;
	public MoveTimer moveTimer;

	private GameObject pausePanel;
	private Client client;
	private CanvasGroup chatPanelCanvas;
	public  bool isPaused;

	public GameObject readyPrefab;
	private GameObject readyPanel;


	public static PauseMenuController Instance { set; get; }
	private void Start()
	{
		Instance = this;
		audioSource = GetComponent<AudioSource>();
		//UnityEngine.Debug.Log(panelObject);


		//UnityEngine.Debug.Log(moveTimer);
		pausePanel = this.gameObject;
		readyPanel = GameObject.FindGameObjectWithTag("ReadyPanel");

		//Reach the client for further message sending solutions
		client = FindObjectOfType<Client>();
		chatPanelCanvas = GameObject.Find("ChatPanel").GetComponent<CanvasGroup>();
	}
	private void Update()
	{
		//Debug.Log(pausePanel.GetComponent<Canvas>().sortingLayerName);
		//You can only stop the game if its your turn
		if ( ConnectLines.Instance.isMyTurn && Input.GetKey("p") && !isPaused && chatPanelCanvas.alpha != 1 )
		{
			client.Send("serverclientsystpause");
		}
		if (client.players[0].isReady && client.players[1].isReady && isPaused)
		{
			HidePanel();
			isPaused = false;
		}
	}
	public void ShowPanel()
	{
		audioSource.PlayOneShot(audioClip);
		startTime = Time.time - pausedTime;
		GameTimer.isTicking = false;
		GameTimer.audioSource.volume = 0f;
		//moveTimer.isTicking = false;
		pausePanel.GetComponent<Canvas>().sortingLayerName = "Pause";
		pausePanel.GetComponentInChildren<Animator>().SetTrigger("fadeIn");
	}
	public void HidePanel()
	{
		pausedTime = Time.time - startTime;
		GameTimer.isTicking = true;
		GameTimer.audioSource.volume = 0.4f;
		foreach (Transform child in readyPanel.transform)
		{
			Destroy(child.gameObject);
		}
		foreach (var item in client.players)
		{
			item.isReady = false;
		}
		pausePanel.GetComponent<Canvas>().sortingLayerName = "Default";
	}
	//Resume button will send a signal that the player is ready -> if it receives the second signal the panel will hide, and game continues
	public void ResumeButton()
	{
		client.Send("serverclientsyst" + "ready" + client.clientName);
	}
}
