using UnityEngine;
using System.Collections;

public class ExperienceBehavior : MonoBehaviour
{
    public GameObject parent;
    float minSize;
    float timeFull, timeShrinking;
    float minSpeed, maxSpeed;
    float minAngle, maxAngle;

    /// <summary>
    /// The time this spends shrinking and ending destroyed
    /// </summary>
    public float TimeShrinking
    {
        get
        {
            return timeShrinking;
        }

        set
        {
            timeShrinking = value;
        }
    }

    /// <summary>
    /// How fast this must shrink to satisfy TimeShrinking
    /// </summary>
    public float RateOfSizeChange
    {
        get
        {
            return -1 / TimeShrinking / (1 - MinSize);
        }
    }

    /// <summary>
    /// How much longer this remains at full size
    /// </summary>
    public float TimeFull
    {
        get
        {
            return timeFull;
        }

        set
        {
            timeFull = value;
        }
    }

    /// <summary>
    /// The minimum initial speed of this
    /// </summary>
    public float MinSpeed
    {
        get
        {
            return minSpeed;
        }

        set
        {
            minSpeed = value;
        }
    }

    /// <summary>
    /// The maximum initial speed of this
    /// </summary>
    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }

        set
        {
            maxSpeed = value;
        }
    }

    /// <summary>
    /// The min angle this makes with the x-axis (before adding parent velocity) in radians
    /// </summary>
    public float MinAngle
    {
        get
        {
            return minAngle;
        }

        set
        {
            minAngle = value % Mathf.PI * 2;
            // account for min > max eventually
        }
    }

    /// <summary>
    /// The max angle this makes with the x-axis (before adding parent velocity) in radians
    /// </summary>
    public float MaxAngle
    {
        get
        {
            return maxAngle;
        }

        set
        {
            maxAngle = value % Mathf.PI * 2; 
            // account for max > min eventually
        }
    }

    /// <summary>
    /// The smallest this gets before being destroyed
    /// </summary>
    public float MinSize
    {
        get
        {
            return minSize;
        }

        set
        {
            minSize = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        float initialSpeed = Random.Range(MinSpeed, MaxSpeed);
        float angleRad = Random.Range(MinAngle, MaxAngle); // random direction of movement
        GetComponent<Rigidbody2D>().velocity = new Vector2(initialSpeed*Mathf.Cos(angleRad), initialSpeed*Mathf.Sin(angleRad));
        if(parent.GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().velocity += parent.GetComponent<Rigidbody2D>().velocity;
        }
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360)); // random angle of appearance
	}

    void Update()
    {
        if (timeFull <= 0)
        {
            GameplayBehavior.ChangeSize(transform, RateOfSizeChange * Time.deltaTime);
            if (transform.localScale.magnitude <= MinSize) { Destroy(gameObject); }
        }
        else { timeFull -= Time.deltaTime; }
    }

    

    /// <summary>
    /// If hits player, remove this from world (collected by player)
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name.Equals("Player"))
        {
            other.GetComponent<PlayerBehavior>().Scrap += 1;
            Destroy(gameObject); // destroy this
        }
    }
}
