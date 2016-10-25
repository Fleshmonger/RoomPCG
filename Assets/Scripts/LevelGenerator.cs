using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int roomAmount = 5;
    public Room[] roomPrefabs;

    private void Awake()
    {
        // Initialize with starting room.
        int roomCount = 1;
        Room lastRoom = InstantiateRoom(RandomRoomPrefab(), Vector3.zero, Quaternion.identity);
        RoomOpening lastOpening = lastRoom.RandomOpening();

        // Loop to generate rest of the level.
        while (roomCount < roomAmount)
        {
            // Instantiate
            Room nextRoom = InstantiateRoom(RandomRoomPrefab(), Vector3.zero, Quaternion.identity);
            RoomOpening nextOpening = nextRoom.RandomOpening();

            // Position and Orientation.
            Vector3 target = -lastOpening.transform.forward;
            Quaternion rotation = Quaternion.FromToRotation(nextOpening.transform.forward, target);
            nextRoom.transform.rotation = rotation;
            Vector3 position = lastOpening.transform.position + (nextRoom.transform.position - nextOpening.transform.position);
            nextRoom.transform.position = position;

            // Loop
            roomCount++;
            nextRoom.RemoveOpening(nextOpening);
            lastRoom = nextRoom;
            lastOpening = nextRoom.RandomOpening();
        }
    }

    private Room InstantiateRoom(Room roomPrefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(roomPrefab, position, rotation) as Room;
    }

    private Room InstantiateRoom(Room roomPrefab, Vector3 position, Vector3 orientation)
    {
        Quaternion rotation = Quaternion.identity;
        rotation.SetLookRotation(orientation);
        return InstantiateRoom(roomPrefab, position, rotation); 
    }

    private Room RandomRoomPrefab()
    {
        if (roomPrefabs.Length > 0)
        {
            return roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        }
        else
        {
            return null;
        }
    }
}