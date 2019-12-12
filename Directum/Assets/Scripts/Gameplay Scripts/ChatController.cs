using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private Client client;
	private GameObject playerMessages;
	public MessagePanelController messagePanel;
	public GameObject textPrefab;

	public GameObject contentParent;
	public InputField inputField;

	private bool wasPressed;
	public GameObject messagePrefab;

	private void Awake()
	{
		playerMessages = GameObject.FindGameObjectWithTag("Message");

		client = FindObjectOfType<Client>();
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
			if (this.GetComponent<CanvasGroup>().alpha == 1)
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
		//Send to server -> format: MSG|clientName|message|messagecolor: r-g-b-a
		if (inputField.text != "")
		{
			client.Send("serverclientchat" + client.clientName + "|" + inputField.text + "|" +
			client.playerColor.r.ToString() + "-" + client.playerColor.g.ToString() + "-" +
			client.playerColor.b.ToString() + "-" + "0.69");
			//client.Send("MSG|" + client.clientName + "|" + inputField.text + "|" +
			//client.playerColor.r.ToString() + "-" + client.playerColor.g.ToString() + "-" +
			//client.playerColor.b.ToString() + "-" + "0.69");
			inputField.Select();
			inputField.text = "";
		}
		else
		{
			Debug.Log("Your message is empty!");
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
		if ( client.isHost == "host"  )
		{
			//Check if the 2 - players pressed the ready button and start the game if so
			if (client.players[0].isReady && client.players[1].isReady)
			{
				//Start the game
				GameObject go = Instantiate(messagePrefab, playerMessages.transform) as GameObject;
				go.GetComponentInChildren<Text>().text = "Starting game!";
				AudioClip clip = Resources.Load("Audio/down", typeof(AudioClip)) as AudioClip;
				gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
				client.Send("serverclientsyststart");
			}
			else
			{
				GameObject go = Instantiate(messagePrefab, playerMessages.transform) as GameObject;
				go.GetComponentInChildren<Text>().text = "Everyone must be ready!";
				Debug.Log("Everyone must be ready!");
			}
		}
		else
		{
			GameObject go = Instantiate(messagePrefab, playerMessages.transform) as GameObject;
			go.GetComponentInChildren<Text>().text = "You are not the host!";
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
