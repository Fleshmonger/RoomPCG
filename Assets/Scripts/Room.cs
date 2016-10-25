using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public float size = 1f;
    public List<RoomOpening> SideOpenings;

    public RoomOpening RandomOpening()
    {
        if (SideOpenings.Count > 0)
        {
            return SideOpenings[Random.Range(0, SideOpenings.Count)];
        }
        else
        {
            return null;
        }
    }

    public bool CollisionCheck(Room other)
    {
        Vector3 offset = other.transform.position - transform.position;
        if (offset.magnitude <= size + other.size)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveOpening(RoomOpening roomOpening)
    {
        SideOpenings.Remove(roomOpening);
    }
}