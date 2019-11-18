using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	public GameObject[] clients;
	private GameObject parent;
	public GameObject textPrefab;
	private void Start()
	{
		clients = GameObject.FindGameObjectsWithTag("Client");
		parent = GameObject.Find("Scroll View/Viewport/Content");
		// Display clients in scrollview
		foreach (var client in clients)
		{
			textPrefab.GetComponent<Text>().text = client.GetComponent<Client>().clientName;
			Instantiate(textPrefab, parent.transform);
		}
	}
	private void Update()
	{

	}
}
