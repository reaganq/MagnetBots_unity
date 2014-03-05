using UnityEngine;
using System.Collections;

public class NetworkDebugging : MonoBehaviour {

	public  float updateInterval = 0.5F;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft;

	public string fpscounter;
	public Rect GuiRect;
	// Use this for initialization
	void Start () {
		GuiRect = new Rect(0, 20, Screen.width / 5, Screen.height - 100);
		timeleft = updateInterval; 
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (PhotonNetwork.connectionStateDetailed != PeerState.Joined)
		{
			return;
		}
		
		GUILayout.BeginArea(GuiRect);
		GUILayout.Label(fpscounter);
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

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("{0:F2} FPS",fps);
			fpscounter = format;

            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
