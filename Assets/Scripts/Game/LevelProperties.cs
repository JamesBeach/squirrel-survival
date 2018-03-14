using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

public class LevelProperties : MonoBehaviour {
    [SerializeField]
    private CampaignLevelParameters _campaignParams;
    [SerializeField]
    private SurvivalLevelParameters _survivalParams;
    [SerializeField]
    private GameMode testGameMode;
    private GameManager manager;

    public CampaignLevelParameters campaignParams
    {
        get { return _campaignParams; }
    }

    public SurvivalLevelParameters survivalParams
    {
        get { return _survivalParams; }
    }

    private void Awake()
    {
        manager = GameManager.instance;

        // determine game mode and load appropriate controller
        GameMode mode = (manager.mode == GameMode.NONE) ? testGameMode : manager.mode;
        switch (mode)
        {
            case GameMode.CAMPAIGN:
                gameObject.AddComponent<CampaignLevelController>();
                break;
            case GameMode.SURVIVAL:
                gameObject.AddComponent<SurvivalLevelController>();
                // get rid of all acorns
                List<GameObject> nuts = new List<GameObject>();
                nuts.AddRange(GameObject.FindGameObjectsWithTag("Nut"));
                foreach (GameObject nut in nuts)
                {
                    Destroy(nut);
                }
                break;
            default:
                Debug.LogError("LevelProperties: Invalid game mode.");
                manager.OnLevelComplete(0);
                break;
        }
    }
}

[System.Serializable]
public struct CampaignLevelParameters
{
    [SerializeField]
    public int minimumNuts;
    [SerializeField]
    public int timeLimit;
}

[System.Serializable]
public struct SurvivalLevelParameters
{
    [SerializeField]
    public int timeOnTimer;
    [SerializeField]
    public int xMin, xMax, yMin, yMax;
    [SerializeField]
    public GameObject acornPrefab;
}