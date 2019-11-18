using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	public GameObject clientObject;
	private Client client;
	private GameObject parent;
	private GameObject messagePanelObject;
	public MessagePanelController messagePanel;
	public GameObject textPrefab;
	private void Awake()
	{
		clientObject = GameObject.FindGameObjectWithTag("Client");
		parent = GameObject.FindGameObjectWithTag("Content");
		client = clientObject.GetComponent<Client>();
		displayPlayerName(client);

		// Display clients in scrollview
	}
	public void displayPlayerName(Client client)
	{
		textPrefab.GetComponent<Text>().text = client.GetComponent<Client>().clientName;
		Instantiate(textPrefab, parent.transform);
	}
}
