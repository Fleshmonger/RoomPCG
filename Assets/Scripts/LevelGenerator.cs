using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public bool perfect = false;
    public int roomAmount = 5;
    public Room[] roomPrefabs;

    private void Awake()
    {
        List<Room> rooms = null;
        do
        {
            ClearRooms(rooms);
            rooms = GenerateLevel();
        }
        while (perfect && rooms.Count < roomAmount);
        if (rooms.Count < roomAmount)
        {
            Debug.Log("Interrupted!");
        }
        else
        {
            Debug.Log("It just werks.");
        }
    }

    private void ClearRooms(List<Room> rooms)
    {
        if (rooms != null)
        {
            foreach (Room room in rooms)
            {
                Destroy(room.gameObject);
            }
        }
    }

    private List<Room> GenerateLevel()
    {
        // Initialize with starting room.
        List<Room> rooms = new List<Room>(), roomTypes = new List<Room>(roomPrefabs);
        Room lastRoom = InstantiateRoom(RandomRoomPrefab(), Vector3.zero, Quaternion.identity);
        RoomOpening lastOpening = lastRoom.RandomOpening();
        rooms.Add(lastRoom);

        // Loop to generate rest of the level.
        while (lastOpening != null && rooms.Count < roomAmount)
        {
            // Check if we have more rooms to check.
            if (roomTypes.Count > 0)
            {
                // Next room type.
                Room roomPrefab = roomTypes[Random.Range(0, roomTypes.Count)];
                roomTypes.Remove(roomPrefab);

                // Instantiate
                Room nextRoom = InstantiateRoom(roomPrefab, Vector3.zero, Quaternion.identity);
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
                    roomTypes = new List<Room>(roomPrefabs);
                }
                else
                {
                    Destroy(nextRoom.gameObject);
                    lastOpening = lastRoom.RandomOpening();
                }
            }
            else
            {
                // Pick a new opening.
                lastRoom.RemoveOpening(lastOpening);
                lastOpening = lastRoom.RandomOpening();
                roomTypes = new List<Room>(roomPrefabs);
            }
        }
        return rooms;
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