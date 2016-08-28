using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthSliderBehavior : MonoBehaviour
{
    PlayerBehavior player;

    public PlayerBehavior Player
    {
        get { return player; }
        set { player = value; }
    }

	// Use this for initialization
	void Start ()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        Slider slider = GetComponent<Slider>();
        slider.value = Player.Health;
        slider.maxValue = Player.Full_Health;
    }
	
	
	void Update ()
    {
        if (Player)
        {
            Slider slider = GetComponent<Slider>();
            slider.value = Player.Health;
            slider.maxValue = Player.Full_Health;
        }
        else
        {
            Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        }
	}
}
