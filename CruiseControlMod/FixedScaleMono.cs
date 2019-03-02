using UnityEngine;

namespace CruiseControlMod
{
    /// <summary>
    /// Represents the active cruise control panel part.
    /// </summary>
    internal class FixedScaleMono : MonoBehaviour
    {
        // Written, 18.10.2018

        /// <summary>
        /// Occurs every frame.
        /// </summary>
        private void Update()
        {
            // Written, 18.10.2018

            this.fixScale();
        }
        /// <summary>
        /// Assigns the gameobjects scale. Fixes issue when picked object up and put down resets scale to 1,1,1.
        /// </summary>
        private void fixScale()
        {
            // Written, 27.09.2018

            if (this.gameObject.transform.hasChanged)
            {
                this.gameObject.transform.localScale = CruiseControlPanel.scale; // For some reason when picked up ccpanel and then letting go resets the scale to 1,1,1. preventing that..
                this.gameObject.transform.hasChanged = false;
            }
        }
    }
}
