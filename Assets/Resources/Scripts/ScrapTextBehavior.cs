using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrapTextBehavior : MonoBehaviour
{
    PlayerBehavior _player;

    public PlayerBehavior Player
    {
        get { return _player; }
        set { _player = value; }
    }

	// Use this for initialization
	void Start ()
    {
        SetPlayer();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!Player) { SetPlayer(); }
        if(Player) { GetComponent<Text>().text = Player.Scrap + " S"; }
	}

    void SetPlayer()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    }
}
