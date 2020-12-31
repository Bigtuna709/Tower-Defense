using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField] Vector3 _towerOffSet;

    public Color hoverCanColor;
    public Color startColor;
    public Color hoverCantColor;

    public Renderer rend;
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
        if (CantBuild() && !BuildManager.Instance.buildMode)
        {
            BuildManager.Instance.SelectNode(this);
            return;
        }
        if(CantBuild())
        {
            return;
        }

        if (!BuildManager.Instance.buildMode)
        {
            BuildManager.Instance.towerUI.SetActive(false);
            return;
        }

        GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
        turret = (GameObject)Instantiate(turretToBuild, transform.position + _towerOffSet, transform.rotation);

        turretToBuild.GetComponent<TowerController>().towerType = BuildManager.Instance.tempTowerType;

        GameManager.Instance.playerTotalGold -= BuildManager.Instance.towerCost;
        GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();

        if(GameManager.Instance.playerTotalGold < BuildManager.Instance.towerCost)
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
        if(BuildManager.Instance.buildMode && !CantBuild())
        {
            rend.material.color = hoverCanColor;
        }
        if (!BuildManager.Instance.buildMode)
        {
            rend.material.color = startColor;
        }
    }
    void OnMouseExit()
    {
        if (BuildManager.Instance.buildMode || !BuildManager.Instance.buildMode)
        {
            rend.material.color = startColor;
        }
    }
}
