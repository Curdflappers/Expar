using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour
{

    GameObject target;
    public float margin;
    bool recentering;
    public float maxSpeed;

    /// <summary>
    /// What this camera is tracking
    /// </summary>
    public GameObject Target
    {
        get { return target; }
    }

    /// <summary>
    /// The tolerable distance between this and its target before this starts to recenter on the target
    /// </summary>
    public float Margin
    {
        get { return margin; }
    }

    /// <summary>
    /// Whether this is moving back toward its target
    /// </summary>
    public bool Recentering
    {
        get { return recentering; }
    }

    /// <summary>
    /// The speed at which this moves to center the target
    /// </summary>
    public float MaxSpeed
    {
        get { return maxSpeed; }
    }
    
	// Use this for initialization
	void Start ()
    {
        target = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Target)
        {
            Vector2 targetFromCamera = transform.position - Target.transform.position;
            if (targetFromCamera.magnitude > Margin) // player is outside of margin
            {
                recentering = true;
            }
            if (Recentering)
            {
                float offsetMagnitude = Mathf.Max(targetFromCamera.magnitude - (maxSpeed * Time.deltaTime), 0);
                if (offsetMagnitude == 0)
                {
                    recentering = false;
                    transform.position = target.transform.position + new Vector3(0, 0, -10);
                }
                else
                {
                    Vector3 offset = targetFromCamera.normalized * offsetMagnitude;
                    transform.position = target.transform.position + offset + new Vector3(0, 0, -10);
                }
            }
        }
	}
}
