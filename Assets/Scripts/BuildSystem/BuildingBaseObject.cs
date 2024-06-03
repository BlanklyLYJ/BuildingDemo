using System.Collections.Generic;
using UnityEngine;

public class BuildingBaseObject : MonoBehaviour
{
    public static BuildingBaseObject Create(Vector3 worldPosition, Vector2Int orgin, BuildingConfig.BuildDir dir,
        BuildingConfig.BuildCfg cfg, Transform prefab)
    {
        Transform buildTransform = Instantiate(prefab, worldPosition,
            Quaternion.Euler(0, BuildingConfig.GetRotationAngle(dir), 0));
        BuildingBaseObject buildingBaseObject = buildTransform.GetComponent<BuildingBaseObject>();
        buildingBaseObject.cfg = cfg;
        buildingBaseObject.orign = orgin;
        buildingBaseObject.dir = dir;
        return buildingBaseObject;
    }

    public BuildingConfig.BuildCfg cfg;
    public Vector2Int orign;
    public BuildingConfig.BuildDir dir;


    public List<Vector2Int> GetGridPositionList()
    {
        return BuildingConfig.GetGridPositionList(orign, cfg.occupyXZ, dir);
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}