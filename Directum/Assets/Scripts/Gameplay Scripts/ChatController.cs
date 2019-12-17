using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private Client client;
	private CanvasGroup chatPanel;
	private MessagePanelController messagePanelController;
	public GameObject textPrefab;

	public GameObject contentParent;
	public InputField inputField;

	private bool wasPressed;
	public GameObject messagePrefab;

	private void Awake()
	{
		messagePanelController = FindObjectOfType<MessagePanelController>();
		client = FindObjectOfType<Client>();
		if ( SceneManager.GetActiveScene().name == "GameMain")
		{
			chatPanel = GetComponent<CanvasGroup>();
		}
	}
	IEnumerator ResetBool()
	{
		yield return new WaitForSeconds(0.5f);
		wasPressed = false;
	}
	private void Update()
	{
		if ( SceneManager.GetActiveScene().name == "Lobby" )
		{
			if (Input.GetKey(KeyCode.Return) && !wasPressed)
			{
				SendMessage();
				wasPressed = true;
				StartCoroutine(ResetBool());
			}
		}
		else
		{
			//Press T to chat
			if ( Input.GetKey(KeyCode.T) )
			{
				OnPointerSetAlpha(1);
				inputField.Select();
			}
			if (chatPanel.alpha == 1)
			{
				if (Input.GetKey(KeyCode.Return) && !wasPressed)
				{
					SendMessage();
					wasPressed = true;
					StartCoroutine(ResetBool());
				}
			}
		}

	}
	public void SendMessage()
	{
		//Let the server know that someone sent a message
		if (inputField.text != "")
		{
			client.Send("serverclientchat" + client.clientName + "|" + inputField.text + "|" +
			client.playerColor.r.ToString() + "-" + client.playerColor.g.ToString() + "-" +
			client.playerColor.b.ToString() + "-" + "0.69");

			inputField.Select();
			inputField.text = "";
		}
		else
		{
			Debug.Log("Your message is empty!");
			messagePanelController.SetMessageAndNotify("YOUR MESSAGE IS EMPTY!");
		}

	}
	public void ReadyToPlay()
	{
		//client.Send("SMSG|"+ client.clientName + "|" + client.isReady);
		client.Send("serverclientsyst" + "ready" + client.clientName);
		UnityEngine.Debug.Log("READY");
	}
	public void StartButton()
	{
		if ( client.isHost == "host" )
		{
			if ( client.players.Count == 2)
			{
				//Check if the 2 - players pressed the ready button and start the game if so
				if (client.players[0].isReady && client.players[1].isReady)
				{
					//Start the game
					messagePanelController.SetMessageAndNotify("STARTING GAME!");
					AudioClip clip = Resources.Load("Audio/down", typeof(AudioClip)) as AudioClip;
					gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
					client.Send("serverclientsyststart");
				}
				else
				{
					messagePanelController.SetMessageAndNotify("EVERYONE MUST BE READY!");
					Debug.Log("Everyone must be ready!");
				}
			}
			else
			{
				messagePanelController.SetMessageAndNotify("WAIT FOR ANOTHER PLAYER!");
				Debug.Log("Wait for another player before you can start!");
			}
			
		}
		else
		{
			messagePanelController.SetMessageAndNotify("YOU ARE NOT THE HOST!");
			//If you aren't host you can't start the game
			Debug.Log("You are not the host!");
			AudioClip clip = Resources.Load("Audio/down", typeof(AudioClip)) as AudioClip;
			gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
		}
	}

	public void OnPointerSetAlpha(float value)
	{
			this.GetComponent<CanvasGroup>().alpha = value;
	}
}
