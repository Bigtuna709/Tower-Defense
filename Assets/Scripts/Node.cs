using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField] Vector3 _towerOffSet;

    public Color hoverCanColor;
    public Color startColor;
    public Color hoverCantColor;

    public Renderer rend;
    GameObject ghostTurret;
    GameObject turret;
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (CantBuild())
        {
            return;
        }

        if (!BuildManager.Instance.buildMode)
        {
            BuildManager.Instance.towerUI.SetActive(false);
            return;
        }
        Destroy(ghostTurret);
        BuildTowerOnNode();
    }

    void BuildTowerOnNode()
    {
        GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
        turret = (GameObject)Instantiate(turretToBuild, transform.position + _towerOffSet, transform.rotation);

        turretToBuild.GetComponent<TowerController>().towerType = BuildManager.Instance.tempTowerType;
        turretToBuild.GetComponent<TowerController>().towerSellCost = BuildManager.Instance.tempSellCost;

        GameManager.Instance.playerTotalGold -= BuildManager.Instance.towerCost;
        GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();

        if (GameManager.Instance.playerTotalGold < BuildManager.Instance.towerCost)
        {
            BuildManager.Instance.ExitBuildMode();
            Debug.Log("<color=red>Not enough gold! - TODO: Display on screen</color>");
        }
    }

    private bool CantBuild()
    {
        return turret != null;
    }

    void OnMouseOver()
    {
        if (BuildManager.Instance.buildMode && CantBuild())
        {
            rend.material.color = hoverCantColor;
        }
        if (BuildManager.Instance.buildMode && !CantBuild())
        {
            rend.material.color = hoverCanColor;
        }
        if (!BuildManager.Instance.buildMode)
        {
            RemoveGhostTower();
        }
    }
    void OnMouseEnter()
    {
        if (BuildManager.Instance.buildMode)
        {
            rend.material.color = startColor;
            if (turret == null)
            {
                GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
                ghostTurret = (GameObject)Instantiate(turretToBuild, transform.position + _towerOffSet, transform.rotation);
                ghostTurret.GetComponent<CapsuleCollider>().isTrigger = true;
                ghostTurret.GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }
    void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            RemoveGhostTower();
            return;
        }
        if (BuildManager.Instance.buildMode || !BuildManager.Instance.buildMode)
        {
            RemoveGhostTower();
        }
    }

    private void RemoveGhostTower()
    {
        rend.material.color = startColor;
        Destroy(ghostTurret);
    }
}
