using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GunInfoScript : MonoBehaviour
{
    public Text NameText, DescriptionText, MagazineSizeText, FireRateText, ReloadTimeText, DamageText, KnockbackText, UnlockText;
    private GameObject player;
    private GameObject selectedGun;
    
    public int PlayerScrap
    {
        get
        {
            return player.GetComponent<PlayerBehavior>().Scrap;
        }
        set
        {
            player.GetComponent<PlayerBehavior>().Scrap = value;
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
    }

    public void DisplayGunInfo(GameObject gun)
    {
        GunBehavior gStats = gun.GetComponent<GunBehavior>();
        AdjustTextBox(NameText, gun.name);
        AdjustTextBox(DescriptionText, gStats.description);
        AdjustTextBox(MagazineSizeText, "Magazine Size: " + gStats.Magazine.ToString());
        AdjustTextBox(FireRateText, "Fire Rate: " + gStats.FireRate.ToString());
        AdjustTextBox(ReloadTimeText, "Reload Time: " + gStats.ReloadTime.ToString());
        AdjustTextBox(DamageText, "Damage: " + gStats.BulletDamage.ToString());
        AdjustTextBox(KnockbackText, "Knockback: " + gStats.BulletKnockback.ToString());
        if (player.GetComponent<PlayerBehavior>().GunsAvailable.Contains(gun))
        {
            AdjustTextBox(UnlockText, "Unlocked");
        }
        else
        {
            AdjustTextBox(UnlockText, "Build\n" + gStats.Cost.ToString() + " S");
        }
        selectedGun = gun;
    }

    void AdjustTextBox(Text textBox, string text)
    {
        textBox.text = text;
    }

    public void UnlockSelectedGun()
    {
        int cost = selectedGun.GetComponent<GunBehavior>().Cost;
        if (!player.GetComponent<PlayerBehavior>().GunsAvailable.Contains(selectedGun))
        {
            if (PlayerScrap >= cost)
            {
                player.GetComponent<PlayerBehavior>().GunsAvailable.Add(selectedGun);
                PlayerScrap -= cost;
                AdjustTextBox(UnlockText, selectedGun.name + " unlocked!");
                GetComponent<AudioSource>().Play();
            }
            else
            {
                AdjustTextBox(UnlockText, (cost - PlayerScrap) + " more Scrap needed");
            }
        }
    }

    public void NextNight()
    {
        SceneManager.LoadScene("Game");
        PlayerBehavior pStats = player.GetComponent<PlayerBehavior>();
        pStats.enabled = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponentInChildren<GunBehavior>().enabled = true;
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        pStats.Health = PlayerBehavior.FULL_HEALTH;
    }

    void Update()
    {
        GameObject.Find("Scrap Text").GetComponent<Text>().text = 
            player.GetComponent<PlayerBehavior>().Scrap + " S";
    }

    public void WinGame()
    {
        if (PlayerScrap > 100)
        {
            SceneManager.LoadScene("WinGame");
        }
        else
        {
            GameObject.Find("Win Game Button Text").GetComponent<Text>().text =
                (100 - PlayerScrap) + " more Scrap needed";
        }
    }
}
