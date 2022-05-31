using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Top,
    Bot,
    Right,
    Left
}

public class DirectionTile 
{
    public int xOffSet = 28;
    public int yOffSet = 20;
    
    private Direction currentDirection;

    public DirectionTile(Direction direction)
    {
        currentDirection = direction;
    }

    public Vector2 getNextTileInArray()
    {
        switch (currentDirection)
        {
            case Direction.Bot : return new Vector2(1, 0);
            case Direction.Top : return new Vector2(-1, 0);
            case Direction.Left : return new Vector2(0, -1);
            case Direction.Right : return new Vector2(0, 1);
            default: return Vector2.zero;
        }
    }
    
    
}
