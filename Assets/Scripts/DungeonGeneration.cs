using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class DungeonGeneration : MonoBehaviour
{
    private Room[,] dungeonRooms;
    public int numberOfRooms;
    private bool bossRoomGenerated = false;
    public Room firstRoom;
    public Room startingRoom;
    public BossRoom bossRoom;
    public List<Room> availableRooms;
    [SerializeField] public Dictionary<int,List<Room>> leftOpeningRooms;
    [SerializeField] public Dictionary<int, List<Room>> rightOpeningRooms;
    [SerializeField] public Dictionary<int, List<Room>> topOpeningRooms;
    [SerializeField] public Dictionary<int, List<Room>> botOpeningRooms;
    private List<Room> roomToTreat;
    public float xOffSet = 0;
    public float yOffSet =20;
    private Random random;
    private int maxDepth = 5;
    private int order = 1;
    private int numberOfRoomsGenerated = 1;
    public int minNumberOfRooms = 10;


    // Start is called before the first frame update
    void Start()
    {
        
        random = new Random();
        leftOpeningRooms = new Dictionary<int, List<Room>>();
        rightOpeningRooms = new Dictionary<int, List<Room>>();
        topOpeningRooms = new Dictionary<int, List<Room>>();
        botOpeningRooms = new Dictionary<int, List<Room>>();
        roomToTreat = new List<Room>();
        sortRooms();
        dungeonRooms = new Room[maxDepth*2+1,maxDepth*2+1];
        startingRoom=Instantiate(firstRoom, new Vector2(0,0), Quaternion.identity);
        roomToTreat.Add(startingRoom);
        startingRoom.x = maxDepth;
        startingRoom.y = maxDepth;
        startingRoom.alreadyVisited = true;
        dungeonRooms[maxDepth,maxDepth] = startingRoom;
        generateDungeon();
       // var graphToScan = AstarPath.active.data.gridGraph;
      //  AstarPath.active.Scan(graphToScan);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void sortRooms()
    {

        foreach (Room room in availableRooms)
        {
            if (room.isBotOpening)
            {
                if(!botOpeningRooms.ContainsKey(room.numberOfOpenings))
                    botOpeningRooms.Add(room.numberOfOpenings, new List<Room>());
                botOpeningRooms[room.numberOfOpenings].Add(room);
            }
            if (room.isLeftOpening)
            {
                if (!leftOpeningRooms.ContainsKey(room.numberOfOpenings))
                    leftOpeningRooms.Add(room.numberOfOpenings, new List<Room>());
                leftOpeningRooms[room.numberOfOpenings].Add(room);
            }
            if (room.isRightOpening)
            {
                if (!rightOpeningRooms.ContainsKey(room.numberOfOpenings))
                    rightOpeningRooms.Add(room.numberOfOpenings, new List<Room>());
                rightOpeningRooms[room.numberOfOpenings].Add(room);
            }
            if (room.isTopOpening)
            {
                if (!topOpeningRooms.ContainsKey(room.numberOfOpenings))
                    topOpeningRooms.Add(room.numberOfOpenings, new List<Room>());
                topOpeningRooms[room.numberOfOpenings].Add(room);
            }
        }
    }

 
    
    public void generateDungeon()
    {
        while (roomToTreat.Count != 0)
        {
            Room roomTreated = roomToTreat[0];
            if (roomTreated.isBotOpening && roomTreated.botRoom==null )
            {
                roomTreated.botRoom = getRandomRoom(roomTreated, topOpeningRooms,Direction.Bot);
                roomTreated.botRoom.setGraphDepth(roomTreated.getDepthInGraph()+1);
                roomTreated.botRoom.x = roomTreated.x+1;
                roomTreated.botRoom.y = roomTreated.y;
                dungeonRooms[roomTreated.botRoom.x, roomTreated.botRoom.y] = roomTreated.botRoom;
                AddNeighbor(roomTreated.botRoom);
                roomToTreat.Add(roomTreated.botRoom);
                initBossRoom(roomTreated.botRoom);
                numberOfRooms++;
                
            }
            if (roomTreated.isTopOpening && roomTreated.topRoom == null)
            {
                roomTreated.topRoom = getRandomRoom(roomTreated, botOpeningRooms,Direction.Top);
                roomTreated.topRoom.setGraphDepth(roomTreated.getDepthInGraph()+1);
                roomTreated.topRoom.x = roomTreated.x - 1;
                roomTreated.topRoom.y = roomTreated.y ;
                dungeonRooms[roomTreated.topRoom.x, roomTreated.topRoom.y] = roomTreated.topRoom;
                AddNeighbor(roomTreated.topRoom);
                roomToTreat.Add(roomTreated.topRoom);
                initBossRoom(roomTreated.topRoom);
                numberOfRooms++;
            }
            if (roomTreated.isLeftOpening && roomTreated.leftRoom == null)
            {
                roomTreated.leftRoom = getRandomRoom(roomTreated, rightOpeningRooms,Direction.Left);
                roomTreated.leftRoom.setGraphDepth(roomTreated.getDepthInGraph()+1);
                roomTreated.leftRoom.x = roomTreated.x;
                roomTreated.leftRoom.y = roomTreated.y-1;
                dungeonRooms[roomTreated.leftRoom.x, roomTreated.leftRoom.y] = roomTreated.leftRoom;
                AddNeighbor(roomTreated.leftRoom);
                roomToTreat.Add(roomTreated.leftRoom);
                initBossRoom(roomTreated.leftRoom);
                numberOfRooms++;
            }
            if (roomTreated.isRightOpening && roomTreated.rightRoom == null)
            {
                roomTreated.rightRoom = getRandomRoom(roomTreated, leftOpeningRooms,Direction.Right);
                roomTreated.rightRoom.setGraphDepth(roomTreated.getDepthInGraph()+1);
                roomTreated.rightRoom.x = roomTreated.x;
                roomTreated.rightRoom.y = roomTreated.y+1;
                dungeonRooms[roomTreated.rightRoom.x, roomTreated.rightRoom.y] = roomTreated.rightRoom;
                AddNeighbor(roomTreated.rightRoom);
                roomToTreat.Add(roomTreated.rightRoom);
                initBossRoom(roomTreated.rightRoom);
                numberOfRooms++;

            };
            roomToTreat.RemoveAt(0); 
        }
    }
    private void AddNeighbor(Room room)
    {
        if ((dungeonRooms[room.x - 1, room.y] != null))
        {
            room.topRoom = dungeonRooms[room.x - 1, room.y];
            room.topRoom.botRoom = room;
        }
        if ((dungeonRooms[room.x + 1, room.y] != null))
        {
            room.botRoom = dungeonRooms[room.x + 1, room.y];
            room.botRoom.topRoom = room;
        }
        if ((dungeonRooms[room.x, room.y + 1] != null))
        {
            room.rightRoom = dungeonRooms[room.x, room.y + 1];
            room.rightRoom.leftRoom = room;
        }
        if ((dungeonRooms[room.x, room.y - 1] != null))
        {
            room.leftRoom = dungeonRooms[room.x, room.y - 1];
            room.leftRoom.rightRoom = room;
        }
    }

    public void initBossRoom(Room room)
    {
        if(room.GetType().Equals(typeof(BossRoom)))
        {
            BossRoom br = (BossRoom)room;
            br.closeOtherDoor();
        }
    }

    private Room getRandomRoom(Room currentRoom, Dictionary<int, List<Room>> rooms, Direction direction)
    {
        DirectionTile directiontile = new DirectionTile(direction);
        Room selectedRoom = null;
        int numberOfOpenings = 0;
        int depth = currentRoom.getDepthInGraph();
        int randomNumberOfRoomSelected;
        int test = 0;
        test = 0;
        int i = 1;
        int j = 0;
        

        if (currentRoom.getDepthInGraph() >= 3)
        {
            while(i < 3 && (selectedRoom == null))
            {
                j = 0;
                while( (j < rooms[i].Count) && (selectedRoom == null)){
                    if (i > 1) Debug.Log("-------------------- I SUP A 1");

                    if (canPlaceRoomOnTile(rooms[i][j], currentRoom.x + (int)directiontile.getNextTileInArray().x, currentRoom.y + (int)directiontile.getNextTileInArray().y))
                    {
                        if (i > 1) Debug.Log("-------------------- AND CAN PLACE I ");
                        Debug.Log("i et  j : " + i + " " + j);
                        selectedRoom = Instantiate(rooms[i][j], translateToUnityPos(currentRoom.x + (int)directiontile.getNextTileInArray().x, currentRoom.y + (int)directiontile.getNextTileInArray().y), Quaternion.identity);
                        if (i > 1) Debug.Log("-------------------- AND CAN PLACE I " + selectedRoom);
                        if (i > 1) Debug.Log("-------------------- -------------");
                    }
                    j++;
                }
                i++;
            }

        }
        else
        {
            while (selectedRoom == null && test<100){
            Room room;
          
                do
                {
                    randomNumberOfRoomSelected = getRandomNumberOfRoomSelected(depth);
                    test++;
                }
                while (!rooms.ContainsKey(randomNumberOfRoomSelected) && test < 100);

                if ((currentRoom.getDepthInGraph() >= 2) && (currentRoom.getDepthInGraph() <= 4) && !bossRoomGenerated && 
                    ( direction == Direction.Right || direction == Direction.Left) &&
                    canPlaceRoomOnTile(bossRoom,currentRoom.x+ (int)directiontile.getNextTileInArray().x, currentRoom.y + (int)directiontile.getNextTileInArray().y ))
                {
                    Debug.Log("B------------------------------------OSS SPAWNED");
                    room = bossRoom;
                    bossRoomGenerated = true;
                }
                else
                {
                    room = rooms[randomNumberOfRoomSelected][random.Next(0, rooms[randomNumberOfRoomSelected].Count)];
                }
               
                if((direction == Direction.Top) && canPlaceRoomOnTile(room,currentRoom.x-1,currentRoom.y) )
                {
                    selectedRoom=Instantiate(room, translateToUnityPos(currentRoom.x-1,currentRoom.y), Quaternion.identity);

                } else if ((direction == Direction.Bot) && canPlaceRoomOnTile(room,currentRoom.x+1,currentRoom.y))
                {
                    selectedRoom=Instantiate(room, translateToUnityPos(currentRoom.x + 1, currentRoom.y), Quaternion.identity);

                } else if ((direction == Direction.Right) && canPlaceRoomOnTile(room,currentRoom.x,currentRoom.y+1))
                {
                    selectedRoom=Instantiate(room, translateToUnityPos(currentRoom.x, currentRoom.y+1), Quaternion.identity);

                } else if ((direction == Direction.Left) && canPlaceRoomOnTile(room,currentRoom.x,currentRoom.y-1))
                {
                    selectedRoom=Instantiate(room, translateToUnityPos(currentRoom.x, currentRoom.y-1), Quaternion.identity);
                }
                test++;
            }
        }
        
        return selectedRoom;
    }

    private Vector2 translateToUnityPos(int x,int y)
    {
        //maxdepth correspond (0,0) dans unity
        float xUnity;
        float yUnity;
        xUnity =( y - (maxDepth))*xOffSet;
        yUnity = ((maxDepth) - x) * yOffSet;
        return new Vector2(xUnity, yUnity);
    }

    private int getRandomNumberOfRoomSelected(int depth){
        var randPercentage = random.NextDouble();
        Dictionary<int,float> associatedPercentage = new Dictionary<int,float>();
        // INIT POURCENTAGE NORMAUX
        for (int i = 1; i < 5; i++)
        {
            associatedPercentage.Add(i, 0.25f);
        }

        // INIT POURCENTAGE NB MINI DE SALLES
        Dictionary<int, float> associatedPercentageForMinRoom = new Dictionary<int, float>();

        associatedPercentageForMinRoom.Add(2, 0.2f);
        associatedPercentageForMinRoom.Add(3, 0.4f);
        associatedPercentageForMinRoom.Add(4, 0.4f);

        bool allRoomToTreatOpeningsClosed = false;
       
        foreach(Room room in roomToTreat)
        {
            if (room.numberOfOpenings != 1) allRoomToTreatOpeningsClosed = true;
        }
        if (allRoomToTreatOpeningsClosed && (numberOfRooms < minNumberOfRooms))
        {
            for (int i = 2; i < 5; i++)
            {
                if (randPercentage < associatedPercentageForMinRoom[i])
                {
                    return i;
                }
                randPercentage -= associatedPercentageForMinRoom[i];
            }

        } else
        {


            if (depth > 1)
            {
                associatedPercentage[1] = associatedPercentage[1] + Mathf.Pow(0.5f, depth);
                associatedPercentage[2] = associatedPercentage[2] + Mathf.Pow(0.25f, depth);
                associatedPercentage[3] = associatedPercentage[3] - Mathf.Pow(0.25f, depth);
                associatedPercentage[4] = associatedPercentage[4] - Mathf.Pow(0.5f, depth);
            }

            for (int i = 1; i < 5; i++)
            {
                if (randPercentage < associatedPercentage[i])
                {
                    return i;
                }
                randPercentage -= associatedPercentage[i];
            }

        } 
         throw new InvalidOperationException(
            "");
    }


    private bool canPlaceRoomOnTile(Room room, int x, int y)
    {
   
        bool canBePlaced = true;
        if ( (dungeonRooms[x - 1, y] != null))
        {
            if ((room.isTopOpening) && !dungeonRooms[x-1, y].isBotOpening)
            {
                canBePlaced = false;
            }
            if(dungeonRooms[x-1, y].isBotOpening && !(room.isTopOpening))
            {
                canBePlaced = false;
            }
        }
        if ((dungeonRooms[x + 1, y] != null))
        {
            if ((room.isBotOpening) && !dungeonRooms[x+1, y].isTopOpening)
            {
                canBePlaced = false;
            }
            if(dungeonRooms[x+1, y].isTopOpening && !(room.isBotOpening))
            {
                canBePlaced = false;
            }
        }
        if ((dungeonRooms[x, y + 1] != null))
        {

            if ((room.isRightOpening) && !dungeonRooms[x, y+1].isLeftOpening)
            {
                canBePlaced = false;
            }
            if(dungeonRooms[x, y+1].isLeftOpening && !(room.isRightOpening))
            {
                canBePlaced = false;
            }
        }
        if ((dungeonRooms[x, y - 1] != null))
        {
            if ((room.isLeftOpening) && !dungeonRooms[x, y-1].isRightOpening)
            {
                canBePlaced = false;
            }
            if(dungeonRooms[x, y-1].isRightOpening && !(room.isLeftOpening))
            {
                canBePlaced = false;
            }
        }
        return canBePlaced;
    }
    
}
