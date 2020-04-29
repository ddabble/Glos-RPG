using System.Collections;
using System.Collections.Generic;
using Target;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;

namespace Menu
{
    /// <summary>
    /// A class for the tutorial menu
    /// </summary>
    public class MenuTutorial : MonoBehaviour
    {
        /// <summary>
        /// Event invoked when the menu is destroyed
        /// </summary>
        public UnityEvent exit = new UnityEvent();

        /// <summary>
        /// The title element in the menu. Changed based on selected language.
        /// </summary>
        public TextMeshProUGUI titleMesh;

        /// <summary>
        /// The text element in the menu. Is used to change the text.
        /// </summary>
        public TextMeshProUGUI textMesh;

        /// <summary>
        /// Button used on the first tutorial page, to move to next page.
        /// </summary>
        public GameObject nextButton;

        /// <summary>
        /// Image of the controller with an arrow pointing to the button you are supposed to press.
        /// </summary>
        public GameObject controllerImage;

        /// <summary>
        /// Image of an info target to show its appearance.
        /// </summary>
        public GameObject infoTargetImage;

        /// <summary>
        /// Image of a fun target to show its appearance.
        /// </summary>
        public GameObject funTargetImage;

        /// <summary>
        /// The hand display that is toggled.
        /// </summary>
        public HandDisplay.HandDisplay handDisplay;

        /// <summary>
        /// The script for shooting with the wand.
        /// </summary>
        public Wand.WandShooting wandShooting;

        /// <summary>
        /// The info targets you are going to shoot in the tutorial.
        /// </summary>
        public GameObject[] infoTargets;

        /// <summary>
        /// The fun targets you are going to shoot in the tutorial.
        /// </summary>
        public GameObject[] funTargets;

        /// <summary>
        /// The text on the different pages of the tutorial
        /// </summary>
        private readonly TutorialPage[] tutorialPages = { new TutorialPage.QuestInfoPage(), new TutorialPage.HandDisplayToggle(), new TutorialPage.ShootProjectile(), new TutorialPage.ShootInfoTarget(), new TutorialPage.ShootFunTarget(), new TutorialPage.StartQuest() };

        /// <summary>
        /// The index of the current page in the <c>tutorialPages</c> array.
        /// </summary>
        private int pageNumber = 0;

        /// <summary>
        /// Initializes the tutorial menu to the right information.
        /// </summary>
        private void Start()
        {
            handDisplay = FindObjectOfType(typeof(HandDisplay.HandDisplay)) as HandDisplay.HandDisplay;
            wandShooting = GameObject.Find("Controller_Right_Wand").GetComponent<Wand.WandShooting>();

            if (GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().IsEnglishSelected())
            {
                titleMesh.text = "Tutorial";
            } else
            {
                titleMesh.text = "Opplæring";
            }

            tutorialPages[0].GoTo(this);
        }

        /// <summary>
        /// For testing without controller
        /// Press T to close the tutorial menu
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Changes the tutorial to the next page
        /// </summary>
        private void NextPage()
        {
            pageNumber++;
            if (pageNumber < tutorialPages.Length)
            {
                tutorialPages[pageNumber - 1].Leave();
                tutorialPages[pageNumber].GoTo(this);
            }
            else
            {
                //SceneManager.LoadScene("Gloshaugen");
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

        /// <summary>
        /// A class for the different pages for the tutorial
        /// </summary>
        private abstract class TutorialPage
        {
            /// <summary>
            /// A method that is called when this tutorial page is opened.
            /// </summary>
            /// <param name="tutorialMenu">The tutorial menu corresponding to this <c>TutorialPage</c></param>
            public abstract void GoTo(MenuTutorial tutorialMenu);

            /// <summary>
            /// A method that is called when this tutorial page is closed.
            /// </summary>
            public abstract void Leave();

            /// <summary>
            /// The tutorial menu this page belongs to.
            /// </summary>
            private MenuTutorial menuTutorial;

            /// <summary>
            /// The text that is displayed on the tutorial canvas on this TutorialPage.
            /// </summary>
            public abstract string info {
                get;
            }

            /// <summary>
            /// A class for when the user gets an introduction to the selected quest
            /// </summary>
            public class QuestInfoPage : TutorialPage
            {
                public override string info => GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().GetQuestInfo();

                public override void GoTo(MenuTutorial tutorialMenu)
                {
                    this.menuTutorial = tutorialMenu;
                    menuTutorial.textMesh.text = info;
                    menuTutorial.nextButton.SetActive(true);
                    menuTutorial.nextButton.GetComponent<Button>().onClick.AddListener(menuTutorial.NextPage);
                }

                public override void Leave()
                {
                    menuTutorial.nextButton.SetActive(false);
                    menuTutorial.nextButton.GetComponent<Button>().onClick.AddListener(menuTutorial.NextPage);
                }
            }

            /// <summary>
            /// A class for when the user learns to toggle the hand display.
            /// </summary>
            public class HandDisplayToggle : TutorialPage
            {

                /// <summary>
                /// Number of times the display is toggled.
                /// </summary>
                public int displayToggles = 0;

                public override string info => GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().IsEnglishSelected() ?
                    "You hold a hologram in your left hand.\n\nTo toggle its visibility, try pressing the button on the back of the left controller.\n\n\n\n\n\n\nTo continue, turn the hologram visibility off and on." :
                    "I din venstre hånd holder du et hologram.\n\nTrykk knappen bakpå venstre kontroll for å skru det av og på.\n\n\n\n\n\n\nFor å fortsette, skru hologrammet av og på.";

                public override void GoTo(MenuTutorial tutorialMenu)
                {
                    this.menuTutorial = tutorialMenu;
                    menuTutorial.textMesh.text = info;
                    menuTutorial.controllerImage.SetActive(true);
                    menuTutorial.handDisplay.OnToggleEvent += DisplayToggled;
                }

                /// <summary>
                /// Triggered when the hand display visibility is toggled.
                /// </summary>
                private void DisplayToggled()
                {
                    displayToggles++;
                    if (displayToggles == 2)
                    {
                        menuTutorial.NextPage();
                    }
                }

                public override void Leave()
                {
                    menuTutorial.controllerImage.SetActive(false);
                    menuTutorial.handDisplay.OnToggleEvent -= DisplayToggled;
                }
            }

            /// <summary>
            /// A class for learning to shoot a projectile.
            /// </summary>
            public class ShootProjectile : TutorialPage
            {

                /// <summary>
                /// Number of times the display is toggled.
                /// </summary>
                public int wandShootings = 0;

                public override string info => GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().IsEnglishSelected() ?
                    "You hold a wand in your right hand.\n\nTo shoot a projectile, try pressing the button on the back of the right controller.\n\n\n\n\n\n\nTo continue, shoot 3 projectiles." :
                    "I din høyre hånd holder du en tryllestav.\n\nFor å skyte prosjektiler, trykk knappen bakpå høyre kontroll.\n\n\n\n\n\n\nFor å fortsette, skyt 3 prosjektiler.";

                public override void GoTo(MenuTutorial tutorialMenu)
                {
                    this.menuTutorial = tutorialMenu;
                    menuTutorial.textMesh.text = info;
                    menuTutorial.controllerImage.SetActive(true);
                    menuTutorial.wandShooting.OnShootEvent += WandShot;
                }

                /// <summary>
                /// Triggered when the hand display visibility is toggled.
                /// </summary>
                private void WandShot()
                {
                    wandShootings++;
                    if (wandShootings == 3)
                    {
                        menuTutorial.NextPage();
                    }
                }

                public override void Leave()
                {
                    menuTutorial.controllerImage.SetActive(false);
                    menuTutorial.wandShooting.OnShootEvent -= WandShot;
                }
            }

            /// <summary>
            /// A class for learning about info targets.
            /// </summary>
            public class ShootInfoTarget : TutorialPage
            {
                /// <summary>
                /// The number of info targets that are currently hit.
                /// </summary>
                private int infoTargetHits = 0;
                public override string info => GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().IsEnglishSelected() ?
                    "This is an info target.\n\n\n\n\n\n\nWhen you shoot it, information will appear on the hologram in your left hand. Info targets also give you ECTS credits.\nShoot the two info targets to the left of you to continue." :
                    "Dette er en infoblink.\n\n\n\n\n\n\nNår du skyter den, får du informasjon på hologrammet i din venstre hånd. Infoblinker gir deg også studiepoeng!\nSkyt de to infoblinkene til venstre for å fortsette.";

                public override void GoTo(MenuTutorial tutorialMenu)
                {
                    this.menuTutorial = tutorialMenu;
                    foreach (GameObject infoTarget in menuTutorial.infoTargets)
                    {
                        infoTarget.SetActive(true);
                        infoTarget.GetComponent<InfoTarget>().OnHitEvent += OnHit;
                    }

                    menuTutorial.textMesh.text = info;
                    menuTutorial.infoTargetImage.SetActive(true);
                }

                /// <summary>
                /// A method that is run whenever an info target is hit.
                /// </summary>
                private void OnHit()
                {
                    infoTargetHits++;
                    if (infoTargetHits == menuTutorial.infoTargets.Length)
                    {
                        menuTutorial.NextPage();
                    }
                }

                public override void Leave()
                {
                    menuTutorial.infoTargetImage.SetActive(false);
                }
            }

            /// <summary>
            /// A class for learning to shoot fun targets.
            /// </summary>
            public class ShootFunTarget : TutorialPage
            {
                /// <summary>
                /// The number of fun targets that are currently hit.
                /// </summary>
                private int funTargetHits = 0;
                public override string info => GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().IsEnglishSelected() ?
                    "This is a fun target.\n\n\n\n\n\n\nThese are just for fun. You get ECTS credits for shooting them though.\n\nShoot the two fun targets to the right of you to continue." :
                    "Dette er en moroblink.\n\n\n\n\n\n\nDisse er her bare for moroskyld, men du får studiepoeng om du skyter dem.\n\nSkyt de to moroblinkene til høyre for å fortsette.";

                public override void GoTo(MenuTutorial tutorialMenu)
                {
                    this.menuTutorial = tutorialMenu;
                    foreach (GameObject funTarget in menuTutorial.funTargets)
                    {
                        funTarget.SetActive(true);
                        funTarget.GetComponent<FunTarget>().OnHitEvent += OnHit;
                    }
                    menuTutorial.textMesh.text = info;
                    menuTutorial.funTargetImage.SetActive(true);
                }

                /// <summary>
                /// A method that is run whenever a fun target is hit.
                /// </summary>
                private void OnHit()
                {
                    funTargetHits++;
                    if (funTargetHits == menuTutorial.funTargets.Length)
                    {
                        menuTutorial.NextPage();
                    }
                }

                public override void Leave()
                {
                    menuTutorial.funTargetImage.SetActive(false);
                }
            }

            /// <summary>
            /// A class for learning telling the user that the quest has started.
            /// </summary>
            public class StartQuest : TutorialPage
            {
                public override string info => GameObject.Find("QuestController").GetComponent<Quest.QuestManager>().IsEnglishSelected() ?
                    "Nice work!\n\nYou may now explore campus." :
                    "Bra jobbet!\n\nDu kan nå utforske campus.";

                public override void GoTo(MenuTutorial tutorialMenu)
                {
                    this.menuTutorial = tutorialMenu;
                    menuTutorial.textMesh.text = info;
                    menuTutorial.nextButton.SetActive(true);
                    menuTutorial.nextButton.GetComponent<Button>().onClick.AddListener(menuTutorial.NextPage);
                }

                public override void Leave()
                {
                    menuTutorial.nextButton.SetActive(false);
                }
            }
        }
    }
}

