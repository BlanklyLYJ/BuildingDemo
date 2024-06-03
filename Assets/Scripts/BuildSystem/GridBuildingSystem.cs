using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }

    private GridManager<GridObject> _gridManager;

    [SerializeField] public List<Transform> testTransformList;

    // 这个可以换成配置 
    private Transform testTransform;

    public BuildingConfig.BuildDir dir = BuildingConfig.BuildDir.Forward;

    public BuildingGhostObject ghostGO;

    private void Awake()
    {
        Instance = this;
        int gridRow = 40;
        int gridCol = 40;
        float gridSize = 5f;
        _gridManager = new GridManager<GridObject>(gridRow, gridCol, gridSize, new Vector3(0, 0, 0),
            (GridManager<GridObject> g, int x, int y) => new GridObject(g, x, y));
        testTransform = testTransformList[0];
    }

    public class GridObject
    {
        private GridManager<GridObject> grid;
        private int x;
        private int z;
        private BuildingBaseObject buildingBaseObject;

        public GridObject(GridManager<GridObject> gird, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public BuildingBaseObject GetBuildingBaseObject()
        {
            return buildingBaseObject;
        }

        public void SetBuildingBaseObject(BuildingBaseObject buildingBaseObject)
        {
            this.buildingBaseObject = buildingBaseObject;
        }

        public void ClearBuildingBaseObject()
        {
            buildingBaseObject = null;
        }

        public bool CanBuild()
        {
            return buildingBaseObject == null;
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + buildingBaseObject;
        }
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // grid.SetValue(GetMouseWorldPosition(), 10);
            _gridManager.GetXZ(GetMouseWorldPosition(), out int x, out int z);
            GridObject gridObj;

            // 拿到配置，todo 方向
            BuildingConfig.BuildCfg cfg = BuildingConfig.cfgMap[testTransform.name];
            List<Vector2Int> occupyList = BuildingConfig.GetGridPositionList(new Vector2Int(x, z), cfg.occupyXZ, dir);
            bool canBuild = true;
            List<GridObject> buildGridObjects = new List<GridObject>();

            foreach (Vector2Int vector2Int in occupyList)
            {
                gridObj = _gridManager.GetTGridObject(vector2Int.x, vector2Int.y);
                buildGridObjects.Add(gridObj);
                // 判断是否都可以建造
                if (!gridObj.CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }


            if (canBuild)
            {
                Vector2Int ocOffset = BuildingConfig.GetRotationOffset(dir, cfg.occupyXZ);
                Vector3 placeObjectWorldPosition = _gridManager.GetXZSetPosition(GetMouseWorldPosition()) +
                                                   new Vector3(ocOffset.x, 0, ocOffset.y) * _gridManager.cellSize;
                // 创建实物
                BuildingBaseObject buildingBaseObject = BuildingBaseObject.Create(placeObjectWorldPosition,
                    new Vector2Int(x, z), dir, cfg, testTransform);

                foreach (GridObject gridObject in buildGridObjects)
                {
                    //挨个设置
                    gridObject.SetBuildingBaseObject(buildingBaseObject);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = BuildingConfig.GetNextDir(dir);
        }

        if (Input.GetMouseButton(1))
        {
            GridObject gridObject = _gridManager.GetTGridObject(GetMouseWorldPosition());
            BuildingBaseObject buildingBaseObject = gridObject.GetBuildingBaseObject();
            if (buildingBaseObject != null)
            {
                List<Vector2Int> occupyList = buildingBaseObject.GetGridPositionList();
                buildingBaseObject.DestroySelf();
                foreach (Vector2Int vector2Int in occupyList)
                {
                    _gridManager.GetTGridObject(vector2Int.x, vector2Int.y).ClearBuildingBaseObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            testTransform = testTransformList[0];
            if (ghostGO)
            {
                ghostGO.RefreshVisual();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            testTransform = testTransformList[1];
            if (ghostGO)
            {
                ghostGO.RefreshVisual();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            testTransform = testTransformList[2];
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            testTransform = testTransformList[3];
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            testTransform = testTransformList[4];
        }
    }

    public Transform GetCurrentBuildingPrefab()
    {
        return testTransform;
    }

    public Quaternion GetBuildingObjectRotation() {
        if (testTransform != null) {
            return Quaternion.Euler(0, BuildingConfig.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }
    
    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = GetMouseWorldPosition();
        _gridManager.GetXZ(mousePosition, out int x, out int z);

        if (testTransform != null) {
            
            Vector2Int rotationOffset = BuildingConfig.GetRotationOffset(dir, BuildingConfig.cfgMap[testTransform.name].occupyXZ);
            Vector3 placedObjectWorldPosition = _gridManager.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _gridManager.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    
    public static Vector3 GetMouseWorldPosition(float planeY = 0f)
    {
        // Vector3 vec = GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector3 dir = ray.direction;
        float num = (planeY - ray.origin.y) / dir.y;
        Vector3 pos = ray.origin + ray.direction * num;
        // Debug.LogError($"看看坐标{pos}");
        return pos;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithY(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithY(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}