using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OxygenBubbleBehavior : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerBehavior player = other.GetComponent<PlayerBehavior>();
            player.HasOxygen = true;
            player.BreathLeft = player.MaxBreath;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerBehavior>().HasOxygen = false;
        }
    }
}
