﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : Singleton<BuildManager>
{
    public List<TowerSO> _towers = new List<TowerSO>();

    public Text towerNameText;

    GameObject _turretToBuild;

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
            GameManager.Instance.buildModePanel.SetActive(true);
            buildMode = true;
            Debug.Log("Build mode activated");
            var tower = _towers.FirstOrDefault(x => x.TowerType == button.towerType);
            if(tower != null)
            {
                if (GameManager.Instance.playerTotalGold >= tower.TowerCost)
                {
                    Debug.Log("You choose tower: " + tower.TowerName);
                    _turretToBuild = tower.TowerPrefab;
                    towerCost = tower.TowerCost;
                    tempTowerType = tower.TowerType;
                }
                else
                {
                    ExitBuildMode();
                    Debug.Log("<color=red>Not enough gold! - TODO: Display on screen</color>");
                }
            }
        }
    }

    public void ExitBuildMode()
    {
        buildMode = false;
        GameManager.Instance.buildModePanel.SetActive(false);
        Debug.Log("Build mode deactivated");
    }
}