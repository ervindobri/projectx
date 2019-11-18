using UnityEngine.UI;
using UnityEngine;

public class CountDown : MonoBehaviour
{
	private float timeLeft = 3.0f;
	private Text countDown;

	[Header("Sound")]
	public AudioSource audioSource;
	private void Start()
	{
		countDown = gameObject.GetComponentInChildren<Text>();
		audioSource = GetComponent<AudioSource>();
		audioSource.Play();
	}
	private void Update()
	{
		timeLeft -= Time.deltaTime;
		countDown.text = timeLeft.ToString("#0");
		if ( timeLeft <= 0)
		{
			// hide panel
			this.gameObject.SetActive(false);
			//GameTimer.isGameTime = true;
		}
	}

}
