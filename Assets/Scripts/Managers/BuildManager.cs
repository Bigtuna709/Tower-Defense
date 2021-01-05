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

    [Header("TowerUI")]
    public GameObject towerUI;
    public Text towerSellText;
    public Text towerUpgradeNameText;
    public Text towerUpgradeDamageText;
    public Text towerUpgradeFireRateText;
    public Text towerUpgradeCostText;
    public GameObject upgradeBTN;

    [HideInInspector]
    public int tempSellCost;

    GameObject _turretToBuild;
    TowerController selectedTower;

    public bool buildMode;

    public int towerCost;
    public TowerType tempTowerType;
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ExitBuildMode();
            towerUI.SetActive(false);
        }
    }
    public GameObject GetTurretToBuild()
    {
            return _turretToBuild;   
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
        tempSellCost = tower.TowerSellAmount;
    }
    void EnterBuildMode()
    {
        GameManager.Instance.buildModePanel.SetActive(true);
        buildMode = true;
        towerUI.SetActive(false);
    }
    public void ExitBuildMode()
    {
        GameManager.Instance.buildModePanel.SetActive(false);
        buildMode = false;
    }
    public void SelectTower(TowerController tower)
    {
        var instance = _towers.FirstOrDefault(x => x.TowerType == tower.towerType);
        if (instance != null)
        {
            selectedTower = tower;
            _turretToBuild = null;
            towerUI.transform.position = tower.transform.position;
            SelectedTowerUI(instance);
            towerUI.SetActive(true);
        }
    }
    public void SelectedTowerUI(TowerSO tower)
    {
        upgradeBTN.GetComponent<Button>().interactable = true;
        if (tower.TowerUpgrade != null)
        {
            towerUpgradeNameText.text = tower.TowerUpgrade.TowerName;
            towerUpgradeDamageText.text = "Tower Damage: " + tower.TowerUpgrade.TowerDamage.ToString();
            towerUpgradeCostText.text = "Cost to upgrade: " + tower.TowerUpgrade.TowerCost.ToString();
            towerSellText.text = "Sell tower for: " + tower.TowerSellAmount.ToString();
            upgradeBTN.GetComponent<ButtonENums>().towerType = tower.TowerUpgrade.TowerType;
        }
        else
        {
            towerUpgradeNameText.text = "";
            towerUpgradeDamageText.text = "";
            towerUpgradeCostText.text = "";
            towerSellText.text = "Sell tower for: " + tower.TowerSellAmount.ToString();
            upgradeBTN.GetComponent<Button>().interactable = false;
        }
    }
    public void SellTower()
    {
        if (selectedTower != null)
        {
            GameManager.Instance.ShowGoldChange(selectedTower.towerSellCost);
            towerUI.SetActive(false);
            Destroy(selectedTower.gameObject);
        }
    }
    public void UpgradeTower(ButtonENums button)
    {
        button = upgradeBTN.GetComponent<ButtonENums>();

        var newTower = _towers.FirstOrDefault(x => x.TowerType == button.towerType);
        if (newTower != null)
        {
            if (GameManager.Instance.playerTotalGold >= newTower.TowerCost) 
            {
                LoadTowerToBuild(newTower);
                GameObject turret = Instantiate(newTower.TowerPrefab, selectedTower.transform.position, transform.rotation);
                turret.GetComponent<TowerController>().towerType = tempTowerType;
                turret.GetComponent<TowerController>().towerSellCost = tempSellCost;

                Destroy(selectedTower.gameObject);
                towerUI.SetActive(false);
                GameManager.Instance.playerTotalGold -= newTower.TowerCost;
                GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();
            }
            else
            {
                Debug.Log("<color=red>Not enough gold! - TODO: Display on screen</color>");
            }
        }

    }
}
