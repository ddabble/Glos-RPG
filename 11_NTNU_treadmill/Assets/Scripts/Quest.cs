using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace Quest
{
    /// <summary>
    /// A class where each instansiated object represents a quest in the game
    /// </summary>
    public class Quest : MonoBehaviour
    {
         /// <summary>
        /// The quest for potential students where you go around on campus and get information about studies at NTNU
        /// and the student life at NTNU.
        /// </summary>
        public static readonly Quest StudentQuest = new Quest(
            new string[] {
                "In this quest you will embark on a five year long journey to complete your masters degree.\nYour goal is to travel across campus and gather knowledge by shooting info targets.\nIf your days of studying becomes too monotone, try shooting some fun targets as well.",
                "I dette oppdraget vil du reise på et fem år langt eventyr for å fullføre en mastergrad.\nMålet ditt er å løpe gjennom campus, samle kunnskap ved å skyte flyvende mål."
            },
            new string[][] {
                new string[] { "studies.json", "student_life.json", "buildings.json", "history.json", "science.json" },
                new string[] { "studier.json", "studentliv.json", "bygninger.json", "historie.json", "vitenskap.json" }
            }
        );
        
        /// <summary>
        /// A quest for external people that are interested in the science and architecture on Gløshaugen
        /// </summary>
        public static readonly Quest VIPQuest = new Quest(
            new string[] {
                "In this quest you will go on a tour through Gløshaugen, shoot some targets and learn a bit about the history of NTNU and the research that takes place here.",
                "I dette oppdraget skal du gå en tur gjennom Gløshaugen, skyte noen ballonger, og lære litt om historien til NTNU og forskningen som gjøres her."
            },
            new string[][] {
                new string[] { "buildings.json", "science.json", "history.json" },
                new string[] { "bygninger.json", "vitenskap.json", "historie.json" }
            }
        );

        /// <summary>
        /// Introduction to the selected quest that is shown on the first tutorial page
        /// </summary>
        public readonly string[] tutorialInfo;

        /// <summary>
        /// File names of the JSON-files with info targets in this quest.
        /// </summary>
        public readonly string[][] InfoTargetFiles;

        private Quest(string[] tutorialInfo, string[][] infoTargetFiles)
        {
            this.tutorialInfo = tutorialInfo;
            this.InfoTargetFiles = infoTargetFiles;
        }
    }
}

