using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    GameObject target;
    GameObject exp;
    public bool deathByPlayer;
    public AudioClip deathSound;
    public float health, strength, speed;
    public int scrap;

    /// <summary>
    /// The experience point this drops
    /// </summary>
    public GameObject Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    /// <summary>
    /// The number of scrap this drops upon death
    /// </summary>
    public int Scrap
    {
        get
        {
            return scrap;
        }

        set
        {
            scrap = value;
        }
    }

    /// <summary>
    /// The sound this plays upon death
    /// </summary>
    public AudioClip DeathSound
    {
        get
        {
            return deathSound;
        }

        set
        {
            deathSound = value;
        }
    }

    void Start()
    {
        target = GameObject.Find("Player");
        deathByPlayer = false;
        Exp = (GameObject)Resources.Load("Prefabs/Scrap");
    }

    void SetExperience(GameObject xp)
    {
        ExperienceBehavior expStats = xp.GetComponent<ExperienceBehavior>();
        expStats.MinSize = 0.05f;
        expStats.MinSpeed = 3f;
        expStats.MaxSpeed = 6f;
        expStats.MinAngle = 0;
        expStats.MaxAngle = 6.28f; // 2pi
        expStats.TimeFull = 2f;
        expStats.TimeShrinking = 2f;
    }

	// Update is called once per frame
	void Update()
    {
        if(health <= 0)
        {
            SpawnSound();
            SpawnScrap();
            Destroy(gameObject);
        }

        if (target)
        {
            transform.rotation = GameplayBehavior.Facing(transform.position, target.transform.position);
            GetComponent<Rigidbody2D>().AddForce(transform.right*speed, ForceMode2D.Force);
        }
	}

    void SpawnSound()
    {
        GameObject child = new GameObject();
        child.AddComponent<AudioSource>();
        child.GetComponent<AudioSource>().clip = DeathSound;
        Instantiate(child);
    }

    void SpawnScrap()
    {
        Exp.GetComponent<ExperienceBehavior>().parent = gameObject;
        for (int i = 0; i < Scrap; i++)
        {
            SetExperience((GameObject)Instantiate(Exp, transform.position, new Quaternion()));
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name.Equals("Player"))
        {
            Attack(other.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.name.Equals("Player"))
        {
            Hurt(other.gameObject);
        }
    }

    void Attack(GameObject obstacle)
    {
        if (obstacle.tag.Equals("Player"))
        {
            var stats = obstacle.GetComponent<PlayerBehavior>();
            stats.Health -= strength;
            stats.GetComponent<Rigidbody2D>().AddForce(transform.right * strength, ForceMode2D.Impulse);
        }
    }

    void Hurt(GameObject obstacle)
    {
        if(obstacle.tag.Equals("Player"))
        {
            var stats = obstacle.GetComponent<PlayerBehavior>();
            stats.Health -= strength*Time.deltaTime;
        }
    }
}
