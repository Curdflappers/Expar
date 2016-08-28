using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour
{
    private float speed;
    private float angleRad;
    private float damage;
    private float knockback;
    float range, minSize;

    /// <summary>
    /// The damage this bullet deals upon hitting an object
    /// </summary>
    public float Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    /// <summary>
    /// The distance this bullet travels from its initial position
    /// </summary>
    public float Range
    {
        get
        {
            return range;
        }

        set
        {
            range = value;
        }
    }

    /// <summary>
    /// The initial speed at which this bullet travels
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    /// <summary>
    /// The smalles this bullet will become before disappearing
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

    /// <summary>
    /// The rate at which this grows (negative makes it shrink)
    /// </summary>
    public float RateOfSizeChange
    {
        get
        {
            return -1 / (Range / Speed) / (transform.lossyScale.magnitude - minSize);
        }
    }

    /// <summary>
    /// The power this hits obstacles with
    /// </summary>
    public float Knockback
    {
        get
        {
            return knockback;
        }

        set
        {
            knockback = value;
        }
    }

    void Start()
    {
        float angleDeg = transform.rotation.eulerAngles.z;
        angleRad = Mathf.Deg2Rad * angleDeg;
        GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(angleRad)*Speed, Mathf.Sin(angleRad)*Speed);
        if(Range == 0) { Range = 1f; } // Avoid dividing by 0
        if(Speed == 0) { Speed = 1f; } // Avoid dividing by 0
        if(MinSize == 0) { MinSize = 0.05f; } // Avoid dividing by 0
    }

    void Update()
    {
        GameplayBehavior.ChangeSize(transform, RateOfSizeChange*Time.deltaTime);
        if(transform.localScale.x < MinSize) { Destroy(gameObject); }
    }

    /// <summary>
    /// Hit the other object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag.Equals("Enemy"))
	    {
            Impact(other.gameObject);
		}
    }

    /// <summary>
    /// Damage enemies
    /// </summary>
    /// <param name="other"></param>
    void Impact(GameObject other)
    {
        if(other.GetComponent<EnemyBehavior>())
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            enemy.health -= Damage;
            if(enemy.health <= 0)
            {
                enemy.deathByPlayer = true;
            }
            other.GetComponent<Rigidbody2D>().AddForce(GetComponent<Rigidbody2D>().velocity.normalized*Knockback, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
}
