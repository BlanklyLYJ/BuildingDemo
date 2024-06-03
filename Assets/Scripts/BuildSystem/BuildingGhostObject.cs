using UnityEngine;

public class BuildingGhostObject:MonoBehaviour
{
    public Transform ghost;
    private BuildingConfig.BuildCfg cfg;

    private void Start()
    {
        RefreshVisual();
    }

    private void LateUpdate() {
        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetBuildingObjectRotation(), Time.deltaTime * 15f);
    }
    
    public void RefreshVisual() {
        if (ghost != null) {
            Destroy(ghost.gameObject);
            ghost = null;
        }

        Transform prefab = GridBuildingSystem.Instance.GetCurrentBuildingPrefab();

        if (prefab != null) {
            ghost = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            ghost.parent = transform;
            ghost.localPosition = Vector3.zero;
            ghost.localEulerAngles = Vector3.zero;
        }
    }


}