using UnityEngine;
using System.Collections;

public class ShotgunBehavior : GunBehavior
{
    public float spread;
    public int bullets;

    /// <summary>
    /// The max angle (in degrees) that a bullet from this gun varies from directly forward
    /// </summary>
    public float Spread
    {
        get
        {
            return spread;
        }

        set
        {
            spread = value;
        }
    }

    /// <summary>
    /// The number of bullets this gun shoots on every fire
    /// </summary>
    public int Bullets
    {
        get
        {
            return bullets;
        }

        set
        {
            bullets = value;
        }
    }

    /// <summary>
    /// Fires a set number of bullets varying randomly
    /// </summary>
    protected override void Fire()
    {
        for (int i = 0; i < Bullets; i++)
        {
            BulletBehavior bullet =
                    ((GameObject)Instantiate(Bullet, transform.position, transform.rotation)).GetComponent<BulletBehavior>();
            Adjust(bullet);
        }
        GetComponent<AudioSource>().Play();
        FireTime = Cooldown;
        BulletsInClip -= 1;
    }

    /// <summary>
    /// Adjusts each bullet like a normal gun, and also adds spread
    /// </summary>
    /// <param name="bStats"></param>
    protected override void Adjust(BulletBehavior bStats)
    {
        base.Adjust(bStats);
        var newAngle = new Vector3(0, 0, gameObject.transform.rotation.eulerAngles.z + Random.Range(-Spread, Spread));
        bStats.gameObject.transform.rotation = Quaternion.Euler(newAngle);
    }

}
