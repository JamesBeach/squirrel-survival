namespace GameManagement
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// The GameManager is responsible for coordinating scenes, menus, and interstitials as well as preserving relevant game data
    /// between them. It stores various game properties (such as the values of items) in a centralized way and any such properties that
    /// should apply globally in a consistent manner should be implemented here. It may also be responsible for the storage and
    /// dispensing of user settings and save data.
    /// </summary>
    public class GameManager
    {

        /********** FIELDS **********/

        private static GameManager _instance;
        private GameMode _mode;              // the current game mode being played
        private Campaign _campaign;          // the campaign being played or the most recent one played.
        private readonly string mainMenu = "Start";

        /********** CONSTRUCTOR ***********/

        /// <summary>
        /// Constructor is private as the GameManager is a singleton. Use the static property getter GameManager.instance instead.
        /// </summary>
        private GameManager()
        {

        }

        /********** DELEGATES **********/

        

        /********** PROPERTIES **********/

        /// <summary>
        /// The instance of the game manager for this session. One will be created if it does not already exist.
        /// </summary>
        public static GameManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// The current game mode being played.
        /// </summary>
        public GameMode mode
        {
            get { return _mode; }
        }

        /// <summary>
        /// The state of the campaign being played.
        /// </summary>
        public Campaign campaign
        {
            // Read-only exposure of campaign state
            // CAMPAIGN PROPERTIES SHOULD NOT BE EDITED FROM OUTSIDE THE GAME MANAGER.
            get { return _campaign; }
        }

        /********** METHODS **********/

        /// <summary>
        /// Begin a new campaign over the given list of levels, in order.
        /// </summary>
        /// <param name="levels">An ordered list of levels that the campaign will progress over.</param>
        public void StartCampaign(List<string> levels)
        {
            _campaign.inProgress = true;
            _campaign.scoreTotal = 0;
            _campaign.scoreAdded = 0;
            _campaign.currentLevel = 0;
            _campaign.levels = levels;

            _mode = GameMode.CAMPAIGN;

            OnCampaignLevelCompleted(0);
        }

        /// <summary>
        /// Start a level in survival mode.
        /// </summary>
        /// <param name="name">The level to load.</param>
        public void StartSurvivalLevel(string name)
        {
            _mode = GameMode.SURVIVAL;
            // SceneManager.sceneLoaded += OnSurvivalLevelLoaded;
            SceneManager.LoadScene(name);
        }

        /// <summary>
        /// This method should be called when a level is completed. The GameManager will take the appropriate action based on
        /// the current game mode and the state of the campaign, if it applies.
        /// </summary>
        /// <param name="score">The score achieved in the level.</param>
        public void OnLevelComplete(int score)
        {
            switch (mode)
            {
                /* WARNING, there be GOTOs ahead! */
                /* The default action is to reset the game state (no active campaign, game mode set to NONE)
                 * and return to the main menu. GOTO statements are used to achieve this in a parsimonious way. */
                case GameMode.CAMPAIGN:
                    if (_campaign.inProgress)
                    {
                        OnCampaignLevelCompleted(score);
                    }
                    else
                    {
                        Debug.LogError("WARNING: Inconsistent state in GameManager; game mode is set to CAMPAIGN but " +
                            "there is no campaign in progress. Returning to main menu.");
                        goto default;
                    }
                    break;
                case GameMode.SURVIVAL:
                    OnSurvivalLevelCompleted(score);
                    break;
                case GameMode.NONE:
                    Debug.Log("GameManager: Level completed with no game mode set. This shouldn't happen unless " +
                        "a level is started outside the main menu (e.g., from the Unity3D editor).");
                    goto default;
                default:
                    _campaign.inProgress = false;
                    _mode = GameMode.NONE;
                    SceneManager.LoadScene(mainMenu);
                    break;
            }
        }

        private void OnCampaignLevelCompleted(int score)
        {
            _campaign.scoreAdded = score;

            /* TODO: REPLACE WITH INTERSTITIAL SCENE WITH TALLYING */
            _campaign.scoreTotal += _campaign.scoreAdded;

            if (_campaign.currentLevel >= _campaign.levels.Count)
            {
                // TODO: Better endgame stuff
                _campaign.inProgress = false;
                SceneManager.LoadScene("Start");
            }
            else
            {
                SceneManager.LoadScene(_campaign.levels[_campaign.currentLevel++]);
            }
        }

        private void OnSurvivalLevelCompleted(int score)
        {
            SceneManager.LoadScene("Start");
        }

        /********** STRUCTS **********/

        
    }

    [System.Serializable]
    public enum GameMode
    {
        NONE,           // No game is being played
        CAMPAIGN,
        SURVIVAL
    }

    /// <summary>
    /// This struct holds data for a campaign game. Score is broken out into the total score and
    /// the score for the most recently completed level. Scores of individual levels are to be visually tallied
    /// into the total score during scene interstitials.
    /// </summary>
    public struct Campaign
    {
        public bool inProgress;       // this campaign is currently in progress if true
        public int currentLevel;      // the level currently being played
        public int scoreTotal;        // the total score of all levels completed so far
        public int scoreAdded;        // the score of the last completed level
        public List<string> levels;   // the levels in this campaign
    }
}