using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Target
{
    /// <summary>
    /// A class for instantiating fun targets in with a random variation from the spawn points in the scene.
    /// </summary>
    public class FunTargetManager : MonoBehaviour
    {
        /// <summary>
        /// The fun target prefab that the <c>FunTargetManager</c> spawnes.
        /// </summary>
        public GameObject funTarget;

        /// <summary>
        /// The total number of fun targets that the <c>FunTargetManager</c> spawnes in the game.
        /// </summary>
        public int numberOfTargets = 3;
        private Transform spawnPointsContainer;
        private Transform funTargetContainer;
        private List<Transform> spawnPoints;

        /// <summary>
        /// Creates fun targets in a random distance from a random selection of <c>numberOfTargets</c> spawn points.
        /// </summary>
        void Start()
        {
            System.Random rnd = new System.Random();
            spawnPointsContainer = this.transform.GetChild(0);
            funTargetContainer = this.transform.GetChild(1);
            spawnPoints = new List<Transform>();
            for (int i = 0; i < spawnPointsContainer.childCount; i++)
            {
                spawnPoints.Add(spawnPointsContainer.GetChild(i));
            }


            foreach (Transform child in spawnPoints.OrderBy(x => rnd.Next()).Take(numberOfTargets))
            {
                GameObject funTargetInstance = Instantiate(funTarget, child.position, child.rotation) as GameObject;
                funTargetInstance.transform.SetParent(funTargetContainer);
            }

        }
    }
}
