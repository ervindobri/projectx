using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private Client client;
	private GameObject parent;
	private GameObject playerMessages;
	private GameObject messagePanelObject;
	public MessagePanelController messagePanel;
	public GameObject textPrefab;

	public GameObject contentParent;
	public InputField inputField;

	private GameObject sendButton;
	private bool wasPressed;
	public GameObject messagePrefab;

	private void Awake()
	{
		sendButton = GameObject.Find("SendButton");
		parent = GameObject.FindGameObjectWithTag("Content");

		playerMessages = GameObject.FindGameObjectWithTag("Message");

		client = FindObjectOfType<Client>();
	}

	private void Update()
	{
		if ( SceneManager.GetActiveScene().name == "Lobby" )
		{
			if (Input.GetKey(KeyCode.Return) && !wasPressed)
			{
				sendButton.GetComponent<Button>().onClick.Invoke();
				Debug.Log("invoked");
				wasPressed = true;
				StartCoroutine(ResetBool());
			}
		}
		else
		{
			if (inputField.isFocused)
			{
				OnPointerSetAlpha(1);
			}
		}

	}
	public void SendMessage()
	{
		//Send to server -> format: MSG|clientName|message|messagecolor: r-g-b-a
		if (inputField.text != "")
		{
			client.Send("serverclientsystchat" + client.clientName + inputField.text + "|" +
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
		client.Send("serverclientsyst" + client.clientName + "ready");
	}
	public void StartButton()
	{
		if ( client.isHost == "host"  )
		{
			//Check if the 2 - players pressed the ready button and start the game if so
			if ( client.players[0].isReady && client.players[1].isReady)
			{
				//Start the game
				GameObject go = Instantiate(messagePrefab, playerMessages.transform) as GameObject;
				go.GetComponentInChildren<Text>().text = "Starting game!";

				//Send a message -> GS
				//client.Send("GS|" + "Game is starting!");
				//Debug.Log("Starting the game!");
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
		}
	}

	public void OnPointerSetAlpha(float value)
	{
			this.GetComponent<CanvasGroup>().alpha = value;
	}
	IEnumerator ResetBool()
	{
		yield return new WaitForSeconds(0.5f);
		wasPressed = false;
	}
}
