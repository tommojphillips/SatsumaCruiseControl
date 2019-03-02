using ModApi.Attachable;
using UnityEngine;

namespace CruiseControlMod
{
    internal class CruiseControlPanel : Part
    {
        // Written, 18.10.2018

        #region Fields

        /// <summary>
        /// Represents the scale of the cruise control unit.
        /// </summary>
        internal static readonly Vector3 scale = new Vector3(100, 100, 100);

        #endregion

        #region Properties

        /// <summary>
        /// Represents the default part info.
        /// </summary>
        protected override PartSaveInfo defaultPartSaveInfo => new PartSaveInfo()
        {
            installed = false, // starts installed?
            position = new Vector3(-9.212333f, 0.08187382f, 9.214458f), // Next to phone
            rotation = Quaternion.Euler(270, 126.112f, 0)
        };
        /// <summary>
        /// Represents the rigid part; the installed part.
        /// </summary>
        protected override GameObject rigidPart
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the active part; the free/pickable part.
        /// </summary>
        protected override GameObject activePart
        {
            get;
            set;
        }
        /// <summary>
        /// Gets save info from encapsulated method.
        /// </summary>
        internal PartSaveInfo getPartSaveInfo
        {
            get
            {
                return this.getSaveInfo();
            }
        }

        #endregion

        public CruiseControlPanel(PartSaveInfo inPartSaveInfo, GameObject part, GameObject parent, Trigger inPartTrigger, Vector3 inPartPosition, Quaternion inPartRotation) : base(inPartSaveInfo, part, parent, inPartTrigger, inPartPosition, inPartRotation)
        {
            // Written, 18.10.2018

            // Adding cruise control function to the rigid part, (installed).
            this.rigidPart.AddComponent<CruiseControl>();
            // Adding fix scale function to parts.
            this.activePart.AddComponent<FixedScaleMono>();
        }
    }
}
