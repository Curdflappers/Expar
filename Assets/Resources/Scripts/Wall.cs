using UnityEngine;
using System.Collections;

public class Wall : Structure
{
    Wall[] _neighbors;

    public Wall[] Neighbors
    {
        get { return _neighbors; }
        set { _neighbors = value; }
    }

	// Use this for initialization
	void Start ()
    {
        _neighbors = new Wall[4];
	}

    /// <summary>
    /// Notify neighbors to destroy their barrier thing
    /// </summary>
    void OnDestroy()
    {
        NotifyNeighbors();
    }

    /// <summary>
    /// Advise all neighbors to destroy their barrier thing
    /// </summary>
    void NotifyNeighbors()
    {
        foreach(Wall neighbor in Neighbors)
        {
            neighbor.Remove(this);
        }
    }

    /// <summary>
    /// Remove the barrier thing because the neighbor has been eliminated
    /// </summary>
    /// <param name="neighborToRemove"></param>
    void Remove(Wall neighborToRemove)
    {
        // adjust barrier thing that this spawns not sure how that works please help
    }
}
