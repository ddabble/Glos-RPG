using Quest;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Target
{
    /// <summary>
    /// A class for instantiating info targets from a file
    /// </summary>
    public class InfoTargetManager : MonoBehaviour
    {

        /// <summary>
        /// The prefab representing a fun target that is instansiated by the <c>InfoTargetManager</c>.
        /// </summary>
        public GameObject infoTargetPrefab;

        private double latitude1 = 63.415169;
        private double longitude1 = 10.410940;

        private double x1 = -246.8;
        private double z1 = 178.6;

        private double latitude2 = 63.419094;
        private double longitude2 = 10.399276;

        private double x2 = 344.4;
        private double z2 = -247.4;

        private double y = 47.901;

        /// <summary>
        /// The game object in the scene hierarcy that is set as the parent to all the info targets. It makes the scene hierarcy more "clean".
        /// </summary>
        public Transform infoTargetParent;

        /// <summary>
        /// An int to hold the number of infotargets, used by PointsController to set infoTargetWeight
        /// </summary>
        private int nrInfoTargets = 0;

        /// <summary>
        /// Gets information about the info targets in the current quest through their JSON-files
        /// with all the info targets, including their latitude and longitude and
        /// information about them. It creates these info targets at start time.
        /// </summary>
        public void LoadInfoTargets()
        {
            QuestManager questManager = GameObject.Find("QuestController").GetComponent<Quest.QuestManager>();
            string[] infoTargetFiles = questManager.GetInfoTargetFiles();
            foreach (string filename in infoTargetFiles)
            {
                InstantiateInfoTargetsFromFile(filename);
            }
        }

        /// <summary>
        /// Instantiates all the info targets in the JSON-file given by the filename
        /// </summary>
        /// <param name="filename">The name of the JSON-file</param>
        private void InstantiateInfoTargetsFromFile(string filename)
        {
            var sr = new StreamReader(Application.streamingAssetsPath + "/infotargets/" + filename);
            var fileContents = sr.ReadToEnd();
            sr.Close();


            InfoTargetArray infoTargetArray = JsonUtility.FromJson<InfoTargetArray>(fileContents);
            Debug.Log(JsonUtility.ToJson(infoTargetArray));
            foreach (InfoTargetMetadata infoTarget in infoTargetArray.infoTargets)
            {
                GenerateInfoTarget(infoTarget);
                nrInfoTargets++;
            }
        }

        /// <summary>
        /// Translates geographic coordinates (latitude and longitude) into Unity coordinates (x and z).
        /// This is done through linear interpolation, where we have two points where we know both the Unity coordinates
        /// and their geographic coordinates.
        /// </summary>
        /// <param name="lat">The latitude in geographic coordinates</param>
        /// <param name="lon">The latitude in geographic coordinates</param>
        /// <returns>The corresponding x and z coordinates in the Unity coordinate system</returns>
        private double[] CoordinatesFromLatLon(double lat, double lon)
        {
            double deltaX = (x2 - x1) / (longitude2 - longitude1);
            double deltaZ = (z2 - z1) / (latitude2 - latitude1);
            double x = x1 + (lon - longitude1) * deltaX;
            double z = z1 + (lat - latitude1) * deltaZ;
            return new double[] { x, z };
        }

        /// <summary>
        /// Instansiates an info target based on the information in the infoTargetMetadata
        /// </summary>
        /// <param name="infoTargetMetadata">Information about the info target to be instansiated, including name, information and geographic coordinates</param>
        private void GenerateInfoTarget(InfoTargetMetadata infoTargetMetadata)
        {
            double[] coords = CoordinatesFromLatLon(infoTargetMetadata.lat, infoTargetMetadata.lon);
            GameObject infoTarget = Instantiate(infoTargetPrefab, new Vector3((float)coords[0], (float)y, (float)coords[1]), Quaternion.identity);
            infoTarget.transform.SetParent(infoTargetParent);
            InfoTarget infoTargetScript = infoTarget.GetComponent<InfoTarget>();
            infoTargetScript.info = infoTargetMetadata.information;
            infoTargetScript.title = infoTargetMetadata.name;
        }

        /// <summary>
        /// A class for holding information about an info target
        /// </summary>
        [System.Serializable]
        public class InfoTargetMetadata
        {
            public string information;
            public string name;
            public double lat;
            public double lon;
        }

        /// <summary>
        /// A helper class for deserializing an array of InfoTargetMetadata
        /// </summary>
        [System.Serializable]
        public class InfoTargetArray
        {
            /// <summary>
            /// An arrray with information about info targets.
            /// </summary>
            public InfoTargetMetadata[] infoTargets;
        }

        /// <summary>
        /// Called by PointsManager, used to set infoTargetWeight
        /// </summary>
        /// <returns>Number of info targets loaded</returns>
        public int GetNrInfoTargets()
        {
            return nrInfoTargets;
        }
    }

}