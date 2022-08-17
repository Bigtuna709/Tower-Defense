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
            RemoveGhostTower();
            return;
        }
        if (CantBuild())
        {
            return;
        }

        if (!BuildManager.Instance.buildMode)
        {
            //move to own function
            if (BuildManager.Instance.selectedTower != null)
            {
                BuildManager.Instance.TurnOffTowerUI();
            }
            else
                return; 
        }

        RemoveGhostTower();
        BuildTowerOnNode();
    }

    void BuildTowerOnNode()
    {
        if (BuildManager.Instance.buildMode)
        {
            GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
            turret = (GameObject)Instantiate(turretToBuild, transform.position + _towerOffSet, transform.rotation);
            GameManager.Instance.builtTowers.Add(turret);

            var newTurret = turretToBuild.GetComponent<TowerController>();
            newTurret.towerFireRadiusIMG.SetActive(false);
            newTurret.towerType = BuildManager.Instance.tempTowerType;
            newTurret.towerSellCost = BuildManager.Instance.tempSellCost;
            turretToBuild.GetComponent<SphereCollider>().radius = BuildManager.Instance.tempTowerRadius;

            GameManager.Instance.playerTotalGold -= BuildManager.Instance.towerCost;
            GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();

            if (GameManager.Instance.playerTotalGold < BuildManager.Instance.towerCost)
            {
                BuildManager.Instance.ExitBuildMode();
                Debug.Log("<color=red>Not enough gold! - TODO: Display on screen</color>");
            }
        }
    }

    private bool CantBuild() => turret != null;

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
                ghostTurret.GetComponent<TowerController>().towerFireRadiusIMG.SetActive(true);
                Destroy(ghostTurret.GetComponent<TowerController>());
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
        if(BuildManager.Instance.buildMode)
        {
            rend.material.color = startColor;
            Destroy(ghostTurret);
        }
    }
}
