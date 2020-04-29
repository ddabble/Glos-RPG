using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

namespace Menu
{
    /// <summary>
    /// A class for the end menu of the game
    /// </summary>
    public class MenuEnd : MonoBehaviour
    {
        private TextMeshProUGUI info;
        private TextMeshProUGUI pageCount;
        private TextMeshProUGUI button;

        private string[] infoPages;
        private int lastPage;
        private int currentPage;

        private string exitButtonText;

        /// <summary>
        /// Event invoked when the menu is destroyed
        /// </summary>
        public UnityEvent exit = new UnityEvent();

        /// <summary>
        /// Writes the game result statistics on the end menu 
        /// </summary>
        void Start()
        {
            // get the text components we want to edit
            info = GameObject.Find("Text - Info").GetComponent<TextMeshProUGUI>();
            pageCount = GameObject.Find("Text - PageCount").GetComponent<TextMeshProUGUI>();
            button = GameObject.Find("Text - Button").GetComponent<TextMeshProUGUI>();

            // set page contents
            infoPages = new string[2];

            // get game stats
            Quest.PointsController pointsController = GameObject.Find("QuestController").GetComponent<Quest.PointsController>();
            int funTargets = pointsController.GetFunTargets();
            int infoTargets = pointsController.GetInfoTargets();
            float time = pointsController.GetTime();
            float credits = pointsController.GetCredits();
            string degree = pointsController.GetDegree();
            string normalTime = pointsController.GetNormalTime();

            // display stats on first page
            if(time == -1)
            {
                infoPages[0] =
                "Fun tragets: " + funTargets + "\n" +
                "Info targets: " + infoTargets + "\n" +
                "ECTS credits: " + credits + "\n" +
                degree + "\n";
            }
            else
            {
                infoPages[0] =
                "Fun tragets: " + funTargets + "\n" +
                "Info targets: " + infoTargets + "\n" +
                "Time: " + time + "\n" +
                "ECTS credits: " + credits + "\n" +
                degree + "\n" +
                normalTime;
            }

            // display sensible message on second page
            infoPages[1] = "In reality it is much harder to complete a degree.\n" +
                "If you want to perform well at NTNU you will have to work hard!";

            // initialize menu page variables
            lastPage = infoPages.Length - 1;
            currentPage = -1;

            // set last page button text
            exitButtonText = "Exit to main menu";

            // call next page to display first page
            Next();
        }

        /// <summary>
        /// For testing without controller
        /// Press T to close the finish menu
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// This method is called when the menu button is clicked and changes the menu to next page.
        /// </summary>
        public void Next()
        {
            if (currentPage < lastPage)
            {
                currentPage++;

                // display next page
                info.text = infoPages[currentPage];

                // update page count
                pageCount.text = "(" + (currentPage + 1) + "/" + infoPages.Length + ")";

                // change button text if moving to last page
                if (currentPage == lastPage)
                {
                    button.text = exitButtonText;
                }
            }
            else // we are on last page and exit this menu
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Called when the player finishes the menu
        /// </summary>
        void OnDestroy()
        {
            exit.Invoke();
        }
    }
}
