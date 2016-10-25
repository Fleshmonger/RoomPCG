using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
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

    public void RemoveOpening(RoomOpening roomOpening)
    {
        SideOpenings.Remove(roomOpening);
    }
}