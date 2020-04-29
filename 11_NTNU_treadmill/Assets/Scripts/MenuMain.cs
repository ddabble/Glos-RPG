using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quest;

namespace Menu
{
    /// <summary>
    /// A class for the main menu
    /// </summary>
    public class MenuMain : MonoBehaviour
    {
        public QuestManager questManager;

        /// <summary>
        /// Changes the current quest in the game to the student quest.
        /// </summary>
        public void ChooseStudentQuest()
        {
            questManager.SetQuest(Quest.Quest.StudentQuest);
            LoadQuest();
        }

        /// <summary>
        /// Changes the current quest in the game to the VIP quest.
        /// </summary>
        public void ChooseVIPQuest()
        {
            questManager.SetQuest(Quest.Quest.VIPQuest);
            LoadQuest();
        }

        /// <summary>
        /// Starts the currently selected quest
        /// </summary>
        private void LoadQuest()
        {
            questManager.LoadQuest();
            Destroy(gameObject);
        }

        /// <summary>
        /// Toggles the language between english and norwegian
        /// </summary>
        public void ToggleLanguage()
        {
            questManager.ToggleLanguage();
        }

        /// <summary>
        /// Quits the game completely
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        /// For testing withour controller
        /// Press T to select the student quest
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChooseStudentQuest();
            }
        }
    }
}

