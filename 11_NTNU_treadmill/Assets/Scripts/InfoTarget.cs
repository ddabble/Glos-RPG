using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Target
{
    /// <summary>
    /// A class for info targets that is inheriting from <c>Target</c>. This class has information about the target to be shown at the display when hit.
    /// </summary>
    public class InfoTarget : Target
    {
        /// <summary>
        /// The title that is shown on the hologram when this info target is hit.
        /// </summary>
        public string title = "Sample Title";

        /// <summary>
        /// The info that is shown on the hologram when this info target is hit.
        /// </summary>
        public string info = "Sample info text";

        /// <summary>
        /// A method that is called when the target is hit, which updates the information on the hand display and updates the hit counter.
        /// </summary>
        protected override void Hit()
        {
            GameObject.Find("HandDisplay - Title").GetComponent<TextMeshProUGUI>().text = title;
            GameObject.Find("HandDisplay - Info").GetComponent<TextMeshProUGUI>().text = info;
            GameObject.Find("Display").GetComponent<HandDisplay.HandDisplay>().ShowDisplay();

            // give points
            pointsController.IncrementInfoTargetCounter();
        }
    }
}
