using UnityEngine;

public class ClearScene : MonoBehaviour
{
	private Client client;

	public void BackButton()
	{
		GameObject connectionManager = GameObject.FindGameObjectWithTag("ConnManager");
		if (connectionManager != null)
		{
			Destroy(connectionManager);
		}
		GameObject clientObject = GameObject.FindGameObjectWithTag("Client");
		if (clientObject != null)
		{
			Destroy(clientObject);
		}
		client = FindObjectOfType<Client>();
		if ( client.isHost == "host")
		{
			foreach (var process in System.Diagnostics.Process.GetProcessesByName("ConcurentTCP"))
			{
				process.Kill();
			}
		}
	}
}
