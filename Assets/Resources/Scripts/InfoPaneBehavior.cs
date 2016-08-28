using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoPaneBehavior : MonoBehaviour
{
    GameObject playerObject;

    public GameObject PlayerObject
    {
        get { return playerObject; }
        set { playerObject = value; }
    }

    /// <summary>
    /// The player in the scene
    /// </summary>
    public PlayerBehavior Player
    {
        get { return playerObject.GetComponent<PlayerBehavior>(); }
    }

    /// <summary>
    /// The current gun of the player
    /// </summary>
    public GunBehavior Gun
    {
        get { return PlayerObject.transform.GetChild(0).gameObject.GetComponent<GunBehavior>(); }
    }

    /// <summary>
    /// The text on the info pane
    /// </summary>
    public string InfoText
    {
        set { GameObject.Find("Info Text").GetComponent<Text>().text = value; }
    }

    /// <summary>
    /// The slider on the info pane
    /// </summary>
    public Slider InfoSlider
    {
        get { return GameObject.Find("Info Slider").GetComponent<Slider>(); }
    }

    public float TimeRemaining
    {
        get { return GameObject.Find("Scene Behavior").GetComponent<GameplayBehavior>().TimeRemaining; }
    }

	/// <summary>
    /// Assign player
    /// </summary>
	void Start ()
    {
        PlayerObject = GameObject.FindWithTag("Player");
	}
	
	/// <summary>
    /// Update pane to match situation
    /// </summary>
	void Update ()
    {
        if (!PlayerObject) { PlayerObject = GameObject.FindWithTag("Player"); }
        if (Player && !Player.HasOxygen) // player has no oxygen
        {
            UpdatePane("Return to the Oxygen Bubble", 0, Player.BreathLeft, Player.MaxBreath);
        }
        else if (Gun.IsReloading)
        {
            UpdatePane("Reloading", 0, Gun.ReloadRemaining, Gun.Reload);
        }
        // no scrap, no enemies, no time left
        else if (!GameObject.Find("Scrap") && !GameObject.FindWithTag("Enemy") && TimeRemaining <= 0)
        {
            UpdatePane("Return to the Ship");
        }
        else if (Gun.BulletsInClip < Gun.Magazine * .5f) // players gun is less than half full
        {
            UpdatePane("Right Click to Reload");
        }
        else if(TimeRemaining <= 0)
        {
            UpdatePane("Eliminate Remaining Enemies");
        }
        else // no information to display
        {
            UpdatePane("");
        }
    }

    /// <summary>
    /// Updates the contents of the info pane
    /// </summary>
    /// <param name="text">The string to display</param>
    /// <param name="min">The minimum value of the slider</param>
    /// <param name="value">The value of the slider</param>
    /// <param name="max">The maximum value of the slider</param>
    void UpdatePane(string text, float min = 0, float value = 1, float max = 1)
    {
        InfoText = text;
        InfoSlider.minValue = min;
        InfoSlider.value = value;
        InfoSlider.maxValue = max;
    }
}
