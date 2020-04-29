using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Quest
{
    /// <summary>
    /// A class for that ensures that the points are counted correctly.
    /// </summary>
    public class PointsController : MonoBehaviour
    {
        private const float funTargetWeight = 7.5f;
        private float infoTargetWeight = 12.0f;
        private int funTargetsShot = 0;
        private int infoTargetsShot = 0;

        private float raceTime;

        private float credits;
        private string degree;
        private string normalTime;

        /// <summary>
        /// Is called when a fun target is shot and updates the counter.
        /// </summary>
        public void IncrementFunTargetCounter()
        {
            funTargetsShot++;
        }

        /// <summary>
        /// Is incremented when an info target is hit and updates the counter.
        /// </summary>
        public void IncrementInfoTargetCounter()
        {
            infoTargetsShot++;
        }

        /// <summary>
        /// A method that is called when you are finished with the quest and that calculates your score based on your completion time
        /// and the number of targets you hit.
        /// </summary>
        public void CalculateDegree()
        {
            infoTargetWeight = 480 / GameObject.Find("InfoTargetManagerPrefab").GetComponent<Target.InfoTargetManager>().GetNrInfoTargets();

            // calculate degree
            credits = funTargetsShot * funTargetWeight + infoTargetsShot * infoTargetWeight;
            if (credits >= 480)
            {
                degree = "You finished a PhD";
            }
            else if (credits >= 300)
            {
                degree = "You finished a masters degree";
            }
            else if (credits >= 180)
            {
                degree = "You finished a bachelors degree";
            }
            else if (credits >= 60)
            {
                degree = "You finished a one year programme";
            }
            else
            {
                degree = "You didn't finish a degree";
            }

            raceTime = -1;

            // Check if degree was completed in normal time
            // 60 seconds being 1 year
            Race.RaceGoalHandler race;
            GameObject finishline = GameObject.Find("finishline_hitbox");
            if (finishline)
            {
                race = finishline.GetComponent<Race.RaceGoalHandler>();
                raceTime = race.GetRaceTime();
            }

            float creditsPerTime = credits / raceTime;

            if (creditsPerTime >= 1.1f)
            {
                normalTime = "You finished your study faster than most";
            }
            else if (creditsPerTime <= 0.9f)
            {
                normalTime = "You used more time than normal on your study";
            }
            else
            {
                normalTime = "You completed your study in normal time";
            }
        }

        public int GetFunTargets() { return funTargetsShot; }
        public int GetInfoTargets() { return infoTargetsShot; }
        public float GetCredits() { return credits; }
        public string GetDegree() { return degree; }
        public float GetTime() { return raceTime; }
        public string GetNormalTime() { return normalTime; }
    }
}
