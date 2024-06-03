using System.Collections.Generic;
using UnityEngine;

public static class BuildingConfig
{
    
    public enum BuildDir
    {
        Forward,
        Left,
        Back,
        Right,
    }
    
    public struct BuildCfg
    {
        public Vector2Int occupyXZ;
    }

    public static BuildDir GetNextDir(BuildDir dir)
    {
        if (dir != BuildDir.Right)
            dir++;
        else
            dir = BuildDir.Forward;
        return dir;
    }

    public static int GetRotationAngle(BuildDir dir)
    {
        switch (dir)
        {
            default:
            case BuildDir.Forward: return 0;
            case BuildDir.Left: return 90;
            case BuildDir.Back: return 180;
            case BuildDir.Right: return 270;
        }
    }
    
    
    public static Vector2Int GetRotationOffset(BuildDir dir, Vector2Int occupy)
    {
        switch (dir)
        {
            default:
            case BuildDir.Forward: return new Vector2Int(0, 0);
            case BuildDir.Left: return new Vector2Int(0, occupy.x);
            case BuildDir.Back: return new Vector2Int(occupy.x, occupy.y);
            case BuildDir.Right: return new Vector2Int(occupy.y, 0);
        }
    }
    

    public static List<Vector2Int> GetGridPositionList(Vector2Int pos, Vector2Int occupy, BuildDir dir)
    {
        List<Vector2Int> occupyList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case BuildDir.Forward:
            case BuildDir.Back:
                for (int x = 0; x < occupy.x; x++)
                {
                    for (int y = 0; y < occupy.y; y++)
                    {
                        occupyList.Add(pos + new Vector2Int(x, y));
                    }
                }

                break;
            case BuildDir.Left:
            case BuildDir.Right:
                for (int x = 0; x < occupy.y; x++)
                {
                    for (int y = 0; y < occupy.x; y++)
                    {
                        occupyList.Add(pos + new Vector2Int(x, y));
                    }
                }

                break;
        }


        return occupyList;
    }

    public static Dictionary<string, BuildCfg> cfgMap = new Dictionary<string, BuildCfg>()
    {
        ["Build1"] = new BuildCfg()
        {
            occupyXZ = new Vector2Int(1, 1),
        },
        ["Build2"] = new BuildCfg()
        {
            occupyXZ = new Vector2Int(2, 1),
        },
        ["Build3"] = new BuildCfg()
        {
            occupyXZ = new Vector2Int(2, 4),
        },
    };
    


}
