using UnityEngine;
using System.Collections;

public class NetworkDebugging : MonoBehaviour {

	public Rect GuiRect;
	// Use this for initialization
	void Start () {
		GuiRect = new Rect(Screen.width / 4, 80, Screen.width / 2, Screen.height - 100);
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (PhotonNetwork.connectionStateDetailed != PeerState.Joined)
		{
			return;
		}
		
		GUILayout.BeginArea(GuiRect);
		GUILayout.Label(PhotonNetwork.playerList.Length + " players in this room.");
		foreach (PhotonPlayer player in PhotonNetwork.playerList)
		{
			GUILayout.Label(player.ToString());
		}
		if (GUILayout.Button("Leave"))
		{
			PhotonNetwork.LeaveRoom();
		}
		GUILayout.EndArea();
	
	}
}
