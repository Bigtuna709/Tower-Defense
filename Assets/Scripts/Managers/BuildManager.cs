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
    public Text towerSellText;
    public Text towerUpgradeNameText;
    public Text towerUpgradeDamageText;
    public Text towerUpgradeFireRateText;
    public Text towerUpgradeFireRangeText;
    public Text towerUpgradeCostText;
    public GameObject upgradeBTN;

    GameObject _turretToBuild;
    TowerController selectedTower;

    public bool buildMode;

    public int towerLevel;
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
        towerLevel = tower.TowerLevel;
        
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
        var instance = _towers.FirstOrDefault(x => x.TowerType == tower.towerType);

        selectedTower = tower;
        _turretToBuild = null;
        towerUI.transform.position = tower.transform.position;
        SelectedTowerUI(instance, towerLevel);
        towerUI.SetActive(true);
    }
    public void SelectedTowerUI(TowerSO tower, int towerLevel)
    {
        upgradeBTN.GetComponent<Button>().interactable = true;
        if (tower.TowerUpgrade != null)
        {
            towerUpgradeNameText.text = tower.TowerUpgrade.TowerName;
            towerUpgradeDamageText.text = tower.TowerUpgrade.TowerDamage.ToString();
            //towerUpgradeFireRangeText.text = tower.TowerUpgrade.TowerRateOfFire.ToString();
            towerUpgradeCostText.text = tower.TowerUpgradeCost.ToString();
            towerSellText.text = tower.TowerSellAmount.ToString();
        }
        else
        {
            towerUpgradeNameText.text = "";
            towerUpgradeDamageText.text = "";
            towerUpgradeCostText.text = "";
            towerSellText.text = tower.TowerSellAmount.ToString();
            upgradeBTN.GetComponent<Button>().interactable = false;
        }
    }
    public void SellTower()
    {

    }
    public void UpgradeTower()
    {

    }
}
