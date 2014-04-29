using UnityEngine;
using System.Collections;

public class Zone : Photon.MonoBehaviour {

	public string Name;

	public Transform spawnPoint;
	public ZoneType type;
	public GameObject zoneObject;
}

public enum ZoneType
{
	town,
	arena
}