using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int roomAmount = 5, maxAttempts = 5;
    public Room[] roomPrefabs;

    private void Awake()
    {
        // Initialize with starting room.
        List<Room> rooms = new List<Room>();
        Room lastRoom = InstantiateRoom(RandomRoomPrefab(), Vector3.zero, Quaternion.identity);
        RoomOpening lastOpening = lastRoom.RandomOpening();
        rooms.Add(lastRoom);

        // Loop to generate rest of the level.
        int attempts = 0;
        while (attempts < maxAttempts && rooms.Count < roomAmount)
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

            // Check
            bool valid = true;
            foreach (Room room in rooms)
            {
                if (!room.Equals(lastRoom) && room.CollisionCheck(nextRoom))
                {
                    valid = false;
                }
            }

            // Loop
            if (valid)
            {
                rooms.Add(nextRoom);
                nextRoom.RemoveOpening(nextOpening);
                lastRoom = nextRoom;
                lastOpening = nextRoom.RandomOpening();
            }
            else
            {
                attempts++;
                Destroy(nextRoom.gameObject);
            }
        }
        if (attempts < maxAttempts)
        {
            Debug.Log("Level generation complete.");
        }
        else
        {
            Debug.Log("Level generation interrupted.");
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