using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameplayBehavior : MonoBehaviour
{
    public float enemiesPerSecond = 0;
    public List<GameObject> enemies;
    GameObject player;
    float timeRemaining, waveTime;
    float spawnTimer;
    public static int level;
    GameObject clockHand;

    /// <summary>
    /// How much time is left in the level
    /// </summary>
    public float TimeRemaining
    {
        get
        {
            return timeRemaining;
        }

        set
        {
            timeRemaining = value;
        }
    }

    /// <summary>
    /// How long this wave lasts
    /// </summary>
    public float WaveTime
    {
        get { return waveTime; }
        set { waveTime = value; }
    }

    /// <summary>
    /// Time since last spawn of enemy
    /// </summary>
    public float SpawnTimer
    {
        get
        {
            return spawnTimer;
        }

        set
        {
            spawnTimer = value;
        }
    }

    /// <summary>
    /// The clock hand that this uses to show time left
    /// </summary>
    public GameObject ClockHand
    {
        get { return clockHand; }
        set { clockHand = value; }
    }

    /// <summary>
    /// Returns the current position of the mouse in world coordinates
    /// </summary>
    /// <returns></returns>
    public static Vector3 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Start()
    {
        level += 1;
        GameObject[] gos = FindObjectsOfType<GameObject>();
        foreach(GameObject go in gos)
        {
            if(go.name.Equals("Player"))
            {
                player = go;
            }
        }
        if(!player)
        {
            player = (GameObject)Instantiate(Resources.Load("Prefabs/Player"));
            player.name = "Player";
        }
        foreach(GameObject go in Resources.LoadAll("Prefabs/Enemies"))
        {
            enemies.Add(go);
        }
        WaveTime = 10 * level;
        GameObject.Find("Day Text").GetComponent<Text>().text = "Day " + level;
        TimeRemaining = WaveTime;
        ClockHand = GameObject.Find("Clock Hand");
    }

    /// <summary>
    /// Returns the quaternion facing from position "position" toward position "target"
    /// </summary>
    /// <param name="position">The position of the beginning</param>
    /// <param name="target">The position to face</param>
    /// <returns></returns>
    static public Quaternion Facing(Vector3 position, Vector3 target)
    {
        float AngleRad = Mathf.Atan2(target.y - position.y, target.x - position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad; // convert to degrees
        return Quaternion.Euler(0, 0, AngleDeg); // manipulate quarternion safely
    }

    void Update()
    {
        SpawnTimer += Time.deltaTime;
        if (TimeRemaining > 0)
        {
            if (SpawnTimer > 0.5f && Random.Range(0, 1f) < enemiesPerSecond * Time.deltaTime)
            {
                SpawnRandomEnemy();
                SpawnTimer = 0;
            }
            AdjustTimer();
        }
        else if (GameObject.FindGameObjectsWithTag("Scrap").Length == 0 
            && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            GoToUpgradeScene();
        }
    }

   void SpawnRandomEnemy()
    {

        GameObject enemy = enemies[Random.Range(0, enemies.Count)];
        Vector3 spawnPos = SpawnPosition(enemy);
        Instantiate(enemy, spawnPos, GameplayBehavior.Facing(spawnPos, player.transform.position));
    }

    void AdjustTimer()
    {
        TimeRemaining -= Time.deltaTime;
        float fractionRemaining = TimeRemaining / WaveTime;
        ClockHand.transform.localRotation = Quaternion.Euler(0, 0, fractionRemaining * 360);
    }

    /// <summary>
    /// Deactivate player and load upgrade screen
    /// </summary>
    void GoToUpgradeScene()
    {
        player.GetComponent<PlayerBehavior>().enabled = false;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponentInChildren<GunBehavior>().enabled = false;
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        SceneManager.LoadScene("Upgrade");
    }

    public static void ChangeSize(Transform transform, float change)
    {
        transform.localScale *= (transform.lossyScale.magnitude + change) / transform.lossyScale.magnitude;
    }

    /// <summary>
    /// Returns the initial position of an object to spawn just offscreen
    /// Assumes camera is at origin
    /// </summary>
    /// <param name="go"></param>
    /// <returns>Position just off screen for the given gameobject</returns>
    static Vector3 SpawnPosition(GameObject go)
    {
        SpriteRenderer spriteR = go.GetComponent<SpriteRenderer>();
        Vector3 goSize = new Vector3(); // assume object has no image
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        Vector3 spawnPos = new Vector3();
        if (spriteR) // if object has an image
        {
            goSize = spriteR.sprite.bounds.size; //adjust for image
        }
        int side = Random.Range(0, 4);
        
        if(side == 0) // top
        {
            spawnPos.y = camHeight + goSize.y;
            spawnPos.x = Random.Range(-(camWidth + goSize.x), camWidth + goSize.x);
        }
        if(side == 1) // right
        {
            spawnPos.y = Random.Range(-(camHeight + goSize.y), camHeight + goSize.y);
            spawnPos.x = camWidth + goSize.x;
        }
        if(side == 2) // bottom
        {
            spawnPos.y = -(camHeight + goSize.y);
            spawnPos.x = Random.Range(-(camWidth + goSize.x), camWidth + goSize.x);
        }
        if(side == 3) // left
        {
            spawnPos.y = Random.Range(-(camHeight + goSize.y), camHeight + goSize.y);
            spawnPos.x = -(camWidth + goSize.x);
        }

        spawnPos += Camera.main.transform.position;
        spawnPos.z = 0;
        return spawnPos;
    }
}
