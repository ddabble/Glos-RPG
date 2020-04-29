using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.SceneManagement;
using Target;

namespace Quest
{
    /// <summary>
    /// A class for selecting quests.
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        /// <summary>
        /// The quest for the current game.
        /// </summary>
        private Quest CurrentQuest = Quest.StudentQuest;

        /// <summary>
        /// The currently selected lanugage
        /// true being english, and false being norwegian
        /// </summary>
        private bool english = false;

        /// <summary>
        /// Sets the current quest of the game to <c>newQuest</c>
        /// </summary>
        /// <param name="newQuest">The quest to set for the current game.</param>
        public void SetQuest(Quest newQuest)
        {
            CurrentQuest = newQuest;
        }

        /// <summary>
        /// Get the current quest in the game
        /// </summary>
        /// <returns>The current quest</returns>
        public Quest GetQuest()
        {
            return CurrentQuest;
        }

        /// <summary>
        /// Toggles the language between english and norwegian
        /// false being english and true being norwegian
        /// </summary>
        public void ToggleLanguage()
        {
            english = !english;
        }

        /// <summary>
        /// For tutorial menu to get quest introduction text
        /// </summary>
        /// <returns>Quest introduction text</returns>
        public string GetQuestInfo()
        {
            if (english)
            {
                return CurrentQuest.tutorialInfo[0];
            }
            else
            {
                return CurrentQuest.tutorialInfo[1];
            }
        }

        /// <summary>
        /// For InfoTargetManager to get filenames for files containing infotargets
        /// </summary>
        /// <returns>List of filenames containing infotargets</returns>
        public string[] GetInfoTargetFiles()
        {
            if (english) // load english infotargets
            {
                return CurrentQuest.InfoTargetFiles[0];
            }
            else // load norwegian infotargets
            {
                return CurrentQuest.InfoTargetFiles[1];
            }
        }

        /// <summary>
        /// Returns wether or not english is selected as the game language
        /// </summary>
        /// <returns></returns>
        public bool IsEnglishSelected()
        {
            return english;
        }

        /// <summary>
        /// Omni character component
        /// </summary>
        public OmniMovementComponent player;
        
        /// <summary>
        /// The menu spawn point.
        /// </summary>
        public GameObject questMenuSpawn;

        /// <summary>
        /// The menu spawn point.
        /// </summary>
        public GameObject endMenuSpawn;

        /// <summary>
        /// A prefab with the main menu. Displayed when finishing the quest.
        /// </summary>
        public GameObject mainMenuPrefab;

        /// <summary>
        /// A prefab with the quest tutorial menu. This prefab is spawned then the scene starts.
        /// </summary>
        public GameObject tutorialMenuPrefab;

        /// <summary>
        /// A prefab with the finish menu when the quest ends. This menu gives you your score in the game.
        /// </summary>
        public GameObject finishMenuPrefab;

        /// <summary>
        /// PointsController for tracking shot targets and calculating degree
        /// </summary>
        private PointsController pointsController;

        /// <summary>
        /// The manager of the info targets in the scene.
        /// </summary>
        private InfoTargetManager infoTargetManager;

        /// <summary>
        /// The startline for the race
        /// </summary>
        public GameObject startLine;

        /// <summary>
        /// Is called when quest is selected from start menu.
        /// Shows player a tutorial menu for the quest and gives them a laser pointer to navigate it.
        /// </summary>
        public void LoadQuest()
        {
            // find points controller
            pointsController = GameObject.Find("QuestController").GetComponent<PointsController>();

            // Display tutorial menu
            questMenuSpawn = GameObject.Find("MenuSpawnPoint");
            GameObject tutorialMenu = Instantiate(tutorialMenuPrefab, questMenuSpawn.transform, false) as GameObject;

            // Start the quest when the tutorial is finished
            tutorialMenu.GetComponent<Menu.MenuTutorial>().exit.AddListener(StartQuest);
        }

        /// <summary>
        /// Is called when the tutorial menu is finished and the quest starts.
        /// This gives you the wand.
        /// </summary>
        private void StartQuest()
        {
            //Restart start and finish-line
            startLine.GetComponentInChildren<Race.RaceStartHandler>().RestartRace();

            // Listen for player to cross race start line
            UnityEvent raceStart = GameObject.Find("startline_hitbox").GetComponent<Race.RaceStartHandler>().startRace;
            UnityAction action = () => { StartRace(); };
            raceStart.AddListener(action);
            startLine.SetActive(false);
            
            // Add listener for player pressing the end button
            // Only for teleporting players, not treadmill users
            if (GameObject.Find("Player"))
            {
                UnityAction actionButtonPress = () => { FinishQuest(); };
                UnityEvent buttonHit = GameObject.Find("Button").GetComponent<ButtonHit>().buttonPress;
                buttonHit.AddListener(actionButtonPress);
            }

            // Load info targets
            infoTargetManager = GameObject.Find("InfoTargetManagerPrefab").GetComponent<InfoTargetManager>();
            infoTargetManager.LoadInfoTargets();

            // Activate treadmill
            player.maxSpeed = 20;

        }

        private void StartRace()
        {
            // Listen for player to cross race finish line
            UnityEvent raceFinish= GameObject.Find("finishline_hitbox").GetComponent<Race.RaceGoalHandler>().finishRace;
            UnityAction action = () => { FinishQuest(); };
            raceFinish.AddListener(action);
        }

        /// <summary>
        /// Called when player finishes race
        /// Shows a menu with score and gives player a laser pointer to navigate menu.
        /// </summary>
        private void FinishQuest()
        {
            // calculate degree
            pointsController.CalculateDegree();

            // Display end quest menu
            endMenuSpawn = GameObject.Find("EndMenuSpawn");
            if (endMenuSpawn.transform.childCount == 0)
            {
                GameObject finishMenu = Instantiate(finishMenuPrefab, endMenuSpawn.transform, false) as GameObject;

                // Add listener for when player finishes end menu
                QuestManager dummy = new GameObject().AddComponent<QuestManager>();
                UnityAction action = () => { dummy.StartCoroutine("ReturnToMainMenu"); };
                finishMenu.GetComponent<Menu.MenuEnd>().exit.AddListener(action);
            }
        }


        /// <summary>
        /// Called when the player finishes the finish menu.
        /// Returns player to main menu.
        /// </summary>
        private IEnumerator ReturnToMainMenu()
        {
            Debug.Log("FINISHED QUEST!");

            // Turn the camera black
            Camera camera = Camera.allCameras[0];
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.cullingMask = 0;
            yield return new WaitForSeconds(2);

            // Reload the scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

