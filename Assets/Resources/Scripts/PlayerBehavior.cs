using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerBehavior : MonoBehaviour
{
    
    float speed;
    private float health;
    public const float FULL_HEALTH = 10;
    int scrap;
    Text scrapText;
    List<GameObject> gunsAvailable;
    bool hasOxygen;
    float breathLeft;
    float maxBreath;
    float suffocationRate;

    /// <summary>
    /// The health remaining of this player
    /// </summary>
    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    /// <summary>
    /// The maximum amount of health of this player
    /// </summary>
    public float Full_Health
    {
        get { return FULL_HEALTH; }
    }

    /// <summary>
    /// The offset for each gun for this player
    /// </summary>
    public Vector2 GunOffset
    {
        get { return new Vector2(0.1f, 0.3f); }
    }

    /// <summary>
    /// The points this player has collected
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
    /// The guns that this player has available
    /// </summary>
    public List<GameObject> GunsAvailable
    {
        get
        {
            return gunsAvailable;
        }

        set
        {
            gunsAvailable = value;
        }
    }

    /// <summary>
    /// Whether this player has oxygen and can breathe easily
    /// </summary>
    public bool HasOxygen
    {
        get { return hasOxygen; }
        set { hasOxygen = value; }
    }

    /// <summary>
    /// How much longer this player can hold its breath
    /// </summary>
    public float BreathLeft
    {
        get { return breathLeft; }
        set { breathLeft = value; }
    }

    /// <summary>
    /// How long this player can hold its breath
    /// </summary>
    public float MaxBreath
    {
        get { return maxBreath; }
        set { maxBreath = value; }
    }

    /// <summary>
    /// The rate at which this player loses health when out of breath
    /// </summary>
    public float SuffocationRate
    {
        get { return suffocationRate; }
        set { suffocationRate = value; }
    }

    void Start()
    {
        GunsAvailable = new List<GameObject>();
        GunsAvailable.Add((GameObject)Resources.Load("Prefabs/Guns/Pistol"));
        Equip(GunsAvailable[0]);
        speed = 10f;
        Health = FULL_HEALTH;
        Scrap = 0;
        DontDestroyOnLoad(gameObject);
        HasOxygen= true;
        MaxBreath = 1.5f;
        BreathLeft = MaxBreath;
        SuffocationRate = 3;
    }
	
	/// <summary>
    /// Face the mouse
    /// Register movement
    /// </summary>
	void Update ()
    {
        if (!HasOxygen)
        {
            if (BreathLeft <= 0) { Health -= SuffocationRate * Time.deltaTime; }
            else
            {
                BreathLeft -= Time.deltaTime;
            }
        }
        else
        {
            if (BreathLeft != MaxBreath) { BreathLeft = MaxBreath; }
        }

        if (Health <= 0) { Die(); }

        Move();
        FaceMouse();
        if (Input.GetAxis("Mouse ScrollWheel") != 0) { ChangeGun(); }
    }

    /// <summary>
    /// Makes the player face the mouse position
    /// </summary>
    void FaceMouse()
    {
        transform.rotation = GameplayBehavior.Facing(transform.position, GameplayBehavior.MousePos());
    }

    /// <summary>
    /// Destroys the player
    /// </summary>
    void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Changes the player gun to the next on the rotation, if the player has scrolled
    /// </summary>
    void ChangeGun()
    {
        Equip(FindNextGun(Input.GetAxis("Mouse ScrollWheel") > 0));
    }

    /// <summary>
    /// Equips the new gun
    /// </summary>
    /// <param name="newGun"></param>
    void Equip(GameObject newGun)
    {
        if(transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        GameObject equipped = Instantiate(newGun);
        equipped.transform.parent = transform; // make it a child of this
        equipped.transform.localPosition = GunOffset;
        equipped.name = equipped.name.Remove(equipped.name.Length - "(Clone)".Length);

        equipped.transform.rotation = GameplayBehavior.Facing(equipped.transform.position, GameplayBehavior.MousePos());
    }

    /// <summary>
    /// Finds which gun is the next one the player possesses
    /// </summary>
    /// <param name="forward"></param>
    /// <returns></returns>
    GameObject FindNextGun(bool forward)
    {
        // the index of the currently held gun
        Transform child = transform.GetChild(0);
        GameObject childObj = child.gameObject;
        string childName = childObj.name;
        int i = GunsAvailable.FindIndex(x => x.name.Equals(childName));
        if(forward) { i++; } else { i--; }
        i %= GunsAvailable.Count;
        if (i < 0) { i += GunsAvailable.Count; }
        return GunsAvailable[i];
    }
    
    /// <summary>
    /// Displaces the character "speed*Time.deltaTime" world units based on WASD movement
    /// Accounts for diagonal movement still displacing a total "speed" world units
    /// If no WASD keys are pressed, no displacement/action is carried out
    /// </summary>
    void Move()
    {
        bool vertical = false, horizontal = false;
        Vector2 acceleration = new Vector2();
        if(Input.GetKey(KeyCode.W)) // up
        {
            acceleration.y += speed;
            vertical = true;
        }
        if(Input.GetKey(KeyCode.S)) // down
        {
            acceleration.y -= speed;
            vertical = true;
        }
        if(Input.GetKey(KeyCode.A)) // left
        {
            acceleration.x -= speed;
            horizontal = true;
        }
        if(Input.GetKey(KeyCode.D)) // right
        {
            acceleration.x += speed;
            horizontal = true;
        }

        if (horizontal && vertical) // diagonal movement
        {
            acceleration /= 1.41421f; // divide by root 2 (simplified)
        }
        GetComponent<Rigidbody2D>().AddForce(acceleration, ForceMode2D.Force);
    }

    /// <summary>
    /// What happens when this player dies: display the lose game scene
    /// </summary>
    void OnDestroy()
    {
        SceneManager.LoadScene("LoseGame");
    }
}
