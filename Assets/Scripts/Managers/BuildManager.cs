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
    public Text towerRadiusText;
    public Text towerDamageText;
    public Image towerImage;

    [Header("TowerUI")]
    public GameObject towerUI;
    public Text towerSellText;
    public Text towerUpgradeNameText;
    public Text towerUpgradeDamageText;
    public Text towerUpgradeFireRadiusText;
    public Text towerUpgradeCostText;
    public GameObject upgradeBTN;

    [HideInInspector]
    public int tempSellCost;
    public int tempTowerRadius;

    GameObject _turretToBuild;
    public TowerController selectedTower;

    public bool buildMode;

    public int towerCost;
    public TowerType tempTowerType;
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.Instance.isPaused && buildMode)
            {
                ExitBuildMode();
            }
            else if(!GameManager.Instance.isPaused || GameManager.Instance.isPaused)
            {
                GameManager.Instance.PauseAndUnPauseGame();
            }
        }

    }
    public GameObject GetTurretToBuild()
    {
            return _turretToBuild;   
    }
    public void BuildTower(ButtonENums button)
    {
        if (!buildMode && selectedTower == null)
        {
            var tower = _towers.FirstOrDefault(x => x.TowerType == button.towerType);
            if (tower != null)
            {
                if (GameManager.Instance.playerTotalGold >= tower.TowerCost)
                {
                    EnterBuildMode();
                    LoadTowerStats(tower);
                    LoadTowerToBuild(tower);
                }
                else
                {
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
        towerRadiusText.text = "Tower Shoot Radius: " + tower.TowerShootRadius.ToString();
        towerCostText.text = "Tower Cost: " + tower.TowerCost.ToString() + "gp";
    }
    void LoadTowerToBuild(TowerSO tower)
    {
        _turretToBuild = tower.TowerPrefab;
        towerCost = tower.TowerCost;
        tempTowerType = tower.TowerType;
        tempSellCost = tower.TowerSellAmount;
        tempTowerRadius = tower.TowerShootRadius;
    }
    void EnterBuildMode()
    {
        if (!GameManager.Instance.isPaused)
        {
            buildMode = true;
            animator.SetTrigger("OpenMenu");
        }
    }
    public void ExitBuildMode()
    {
        buildMode = false;
        animator.SetTrigger("CloseMenu");
    }
    public void SelectTower(TowerController tower)
    {
        var instance = _towers.FirstOrDefault(x => x.TowerType == tower.towerType);
        if (instance != null)
        {
            if (selectedTower == null)
            {
                selectedTower = tower;
                _turretToBuild = null;
                towerUI.transform.position = tower.transform.position;
                SelectedTowerUI(instance);
                towerUI.SetActive(true);
                tower.towerFireRadiusIMG.SetActive(true);
            }
            else
            {
                selectedTower.towerFireRadiusIMG.SetActive(false);
                towerUI.SetActive(false);
                selectedTower = null;
            }
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
    public void UpgradeTower(ButtonENums button)
    {
        button = upgradeBTN.GetComponent<ButtonENums>();

        var newTower = _towers.FirstOrDefault(x => x.TowerType == button.towerType);
        if (newTower != null)
        {
            if (GameManager.Instance.playerTotalGold >= newTower.TowerCost)
            {
                DestroyOldTower(newTower);

                LoadTowerToBuild(newTower);

                CreateUpgradedTower(newTower);
            }
            else
            {
                Debug.Log("<color=red>Not enough gold! - TODO: Display on screen</color>");
            }
        }
        else
        {
            Debug.Log("<color=red>No such tower! Add the new tower to the list!</color>");
        }

    }

    private void CreateUpgradedTower(TowerSO newTower)
    {
        GameObject tower = Instantiate(newTower.TowerPrefab, selectedTower.transform.position, transform.rotation);
        GameManager.Instance.builtTowers.Add(tower);

        tower.GetComponent<SphereCollider>().radius = tempTowerRadius;
        var towerStats = tower.GetComponent<TowerController>();
        towerStats.towerType = tempTowerType;
        towerStats.towerSellCost = tempSellCost;
    }
    public void SellTower()
    {
        if (selectedTower != null)
        {
            GameManager.Instance.ShowGoldChange(selectedTower.towerSellCost);
            GameManager.Instance.builtTowers.Remove(selectedTower.gameObject);
            towerUI.SetActive(false);
            Destroy(selectedTower.gameObject);
        }
    }
    private void DestroyOldTower(TowerSO newTower)
    {
        Destroy(selectedTower.gameObject);
        GameManager.Instance.builtTowers.Remove(selectedTower.gameObject);
        towerUI.SetActive(false);
        GameManager.Instance.playerTotalGold -= newTower.TowerCost;
        GameManager.Instance.playerGoldText.text = GameManager.Instance.playerTotalGold.ToString();
    }
}
