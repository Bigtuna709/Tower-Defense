using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : Singleton<BuildManager>
{
    public List<TowerSO> _towers = new List<TowerSO>();

    [Header("Build Menu")]
    public Text towerNameText;
    public Text towerCostText;
    public Text towerTypeText;
    public Text towerDamageText;
    public Image towerImage;

    public GameObject towerUI;

    [Header("TowerUI")]
    public int towerSellAmount;

    GameObject _turretToBuild;
    TowerController selectedTower;

    public bool buildMode;

    public int towerCost;
    public TowerType tempTowerType;

    public GameObject GetTurretToBuild()
    {
            return _turretToBuild;   
    }
    private void Update()
    {
        if (buildMode)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ExitBuildMode();
            }
        }
    }
    public void BuildTower(ButtonENums button)
    {
        if (!buildMode)
        {
            EnterBuildMode();
            var tower = _towers.FirstOrDefault(x => x.TowerType == button.towerType);
            if (tower != null)
            {
                if (GameManager.Instance.playerTotalGold >= tower.TowerCost)
                {
                    LoadTowerStats(tower);
                    LoadTowerToBuild(tower);
                }
                else
                {
                    ExitBuildMode();
                    Debug.Log("<color=red>Not enough gold! - TODO: Display on screen</color>");
                }
            }
        }
    }


    void LoadTowerStats(TowerSO tower)
    {
        towerImage.sprite = tower.TowerSprite;
        towerNameText.text = "Tower Name: " + tower.TowerName;
        towerDamageText.text = "Tower Damage: " + tower.TowerDamage.ToString();
        towerTypeText.text = "Tower Type: " + tower.TowerType.ToString();
        towerCostText.text = "Tower Cost: " + tower.TowerCost.ToString() + "gp";
    }
    void LoadTowerToBuild(TowerSO tower)
    {
        _turretToBuild = tower.TowerPrefab;
        towerCost = tower.TowerCost;
        tempTowerType = tower.TowerType;
    }
    void EnterBuildMode()
    {
        GameManager.Instance.buildModePanel.SetActive(true);
        buildMode = true;
        towerUI.SetActive(false);
        Debug.Log("Build mode activated");
    }
    public void ExitBuildMode()
    {
        buildMode = false;
        GameManager.Instance.buildModePanel.SetActive(false);
        Debug.Log("Build mode deactivated");
    }

    public void SelectTower(TowerController tower)
    {
        selectedTower = tower;
        _turretToBuild = null;
        towerUI.transform.position = tower.transform.position;
        towerUI.SetActive(true);
    }

    public void SellTower(TowerSO tower)
    {
        towerSellAmount = tower.TowerSellAmount;
        GameManager.Instance.playerTotalGold += towerSellAmount;
        GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();
    }
    public void UpgradeTower()
    {

    }
}
