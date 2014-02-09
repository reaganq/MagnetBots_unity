using UnityEngine;
using System.Collections;

public class ArenaManager : MonoBehaviour {

    public Transform[] arenaSpawnPos;
    public Transform enemySpawnPos;
    public string arenaName;
    public GameObject enemyPrefab;
    public GameObject enemy;

	public void Initialise()
    {
        enemy = GameObject.Instantiate(enemyPrefab, enemySpawnPos.position, Quaternion.identity) as GameObject;
    }
}
