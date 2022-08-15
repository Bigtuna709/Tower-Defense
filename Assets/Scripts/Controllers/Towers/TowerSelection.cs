using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelection : MonoBehaviour
{
    private TowerController towerController;
    private void Awake()
    {
        towerController = GetComponent<TowerController>();
    }
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!BuildManager.Instance.buildMode)
        {
            BuildManager.Instance.SelectTower(towerController);
            return;
        }
    }


}
