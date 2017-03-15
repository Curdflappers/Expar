using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GunBehavior : MonoBehaviour
{

    GameObject bullet;
    float cooldownLeft, reloadingLeft;
    public float cooldown, reload;
    public int magazine;
    int bulletsInClip;
    Slider bulletSlider;
    public float bulletSpeed, bulletRange, bulletDamage;
    public Sprite bulletSprite;
    public float bulletKnockback;
    public string description;
    public int cost;
    bool isReloading;

    /// <summary>
    /// The bullet of this gameobject
    /// </summary>
    public GameObject Bullet
    {
        get
        {
            return bullet;
        }

        set
        {
            bullet = value;
        }
    }

    /// <summary>
    /// Current bullets in the clip of this gun
    /// </summary>
    public int BulletsInClip
    {
        get
        {
            return bulletsInClip;
        }

        set
        {
            bulletsInClip = value;
            AdjustBulletSlider();
        }
    }

    /// <summary>
    /// Time between each shot fired for this gun
    /// </summary>
    public float FireTime
    {
        get
        {
            return cooldownLeft;
        }

        set
        {
            cooldownLeft = value;
        }
    }

    /// <summary>
    /// Time left to reload this gun
    /// </summary>
    public float ReloadRemaining
    {
        get
        {
            return reloadingLeft;
        }

        set
        {
            reloadingLeft = value;
        }
    }

    /// <summary>
    /// Speed at which bullets fired from this gun travel
    /// </summary>
    public float BulletSpeed
    {
        get
        {
            return bulletSpeed;
        }

        set
        {
            bulletSpeed = value;
            if(Bullet) { Bullet.GetComponent<BulletBehavior>().Speed = value; }
        }
    }

    /// <summary>
    /// Distance bullets fired from this gun travel
    /// </summary>
    public float BulletRange
    {
        get
        {
            return bulletRange;
        }

        set
        {
            bulletRange = value;
            if (Bullet) { Bullet.GetComponent<BulletBehavior>().Range = value; }
        }
    }

    /// <summary>
    /// Damage bullets from this gun deal
    /// </summary>
    public float BulletDamage
    {
        get
        {
            return bulletDamage;
        }

        set
        {
            bulletDamage = value;
            if (Bullet) { Bullet.GetComponent<BulletBehavior>().Damage = value; }
        }
    }

    /// <summary>
    /// The time in between each shot for this gun
    /// </summary>
    public float Cooldown
    {
        get
        {
            return cooldown;
        }

        set
        {
            cooldown = value;
        }
    }

    /// <summary>
    /// The time required to reload this gun
    /// </summary>
    public float ReloadTime
    {
        get
        {
            return reload;
        }

        set
        {
            reload = value;
        }
    }

    /// <summary>
    /// The total bullets this gun holds per clip
    /// </summary>
    public int Magazine
    {
        get
        {
            return magazine;
        }

        set
        {
            magazine = value;
        }
    }

    /// <summary>
    /// How many bullets this can fire per second
    /// </summary>
    public float FireRate
    {
        get
        {
            return 1 / Cooldown;
        }
    }

    /// <summary>
    /// The power this bullet hits objects with
    /// </summary>
    public float BulletKnockback
    {
        get
        {
            return bulletKnockback;
        }

        set
        {
            bulletKnockback = value;
        }
    }

    /// <summary>
    /// Whether this gun is currently reloading
    /// </summary>
    public bool IsReloading
    {
        get { return isReloading; }
        set { isReloading = value; }
    }

    void Start ()
    {
        FireTime = 0;
        BulletsInClip = Magazine;
        IsReloading = false;
        Bullet = (GameObject)Resources.Load("Prefabs/Bullet");
    }
	
	/// <summary>
    /// Face the mouse, attempt to fire, adjust timers and slider
    /// </summary>
	void Update ()
    {
        transform.rotation = GameplayBehavior.Facing(transform.position, GameplayBehavior.MousePos());

        if (!isReloading)
        {
            if (Input.GetMouseButtonDown(1)) // reload button
            {
                StartReload();
            }

            if (Input.GetMouseButton(0) && CanFire) // fire button
            {
                Fire();
            }
        }

        else
        {
            if (ReloadRemaining <= 0)
            {
                isReloading = false;
                BulletsInClip = Magazine;
            }
        }

        ReloadRemaining -= Time.deltaTime;
        FireTime -= Time.deltaTime;
    }

    void PlaySound(string soundName)
    {
        foreach (AudioSource sound in GetComponents<AudioSource>())
        {
            if (sound.clip.name == soundName)
            {
                sound.PlayOneShot(sound.clip);
            }
        }
    }

    void StartReload()
    {
        ReloadRemaining = ReloadTime;
        IsReloading = true;
    }

    /// <summary>
    /// Returns whether this gun can presently fire
    /// </summary>
    /// <returns>Has cooled down, has bullets, is not reloading</returns>
    public bool CanFire
    {
        get
        {
            return FireTime <= 0 && BulletsInClip > 0 && !isReloading;
        }
    }

    /// <summary>
    /// How much scrap is required to unlock this gun.
    /// </summary>
    public int Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }

    /// <summary>
    /// Shoots a bullet matching BulletDamage, BulletRange, etc., from this gun
    /// Decrease bullets in clip, update slider
    /// </summary>
    protected virtual void Fire()
    {
        BulletBehavior bullet = 
            ((GameObject)Instantiate(Bullet, transform.position, transform.rotation)).GetComponent<BulletBehavior>();
        Adjust(bullet);
        PlaySound("Pew");
        FireTime = Cooldown;
        BulletsInClip -= 1;
    }

    /// <summary>
    /// Sets the bullet to match the gun's stats
    /// </summary>
    /// <param name="bStats"></param>
    protected virtual void Adjust(BulletBehavior bStats)
    {
        bStats.Damage = BulletDamage;
        bStats.Range = BulletRange;
        bStats.Speed = BulletSpeed;
        bStats.Knockback = BulletKnockback;
    }
    
    /// <summary>
    /// Adjusts the bullet slider based on bullets remaining in this clip.
    /// </summary>
    void AdjustBulletSlider()
    {
        if(!bulletSlider)
        {
            bulletSlider = GameObject.Find("Bullet Slider").GetComponent<Slider>();
            bulletSlider.maxValue = Magazine;
        }

        bulletSlider.value = BulletsInClip;
    }

}
