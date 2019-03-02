using System;
using UnityEngine;
using MSCLoader;
using ModApi;

namespace CruiseControlMod
{
    /// <summary>
    /// Represents Cruise Control Function for the Cruise Control Unit.
    /// </summary>
    internal class CruiseControl : MonoBehaviour
    {
        // Written, 27.09.2018

        #region Properties

        // Instance Properties
        /// <summary>
        /// Represents whether the stock steering wheel is installed.
        /// </summary>
        private bool isStockSteeringWheelInstalled => (this.gameObject // ccPanel that's attached to the stock steering wheel.
                                                        .transform?.parent? // stock steering wheel.
                                                        .transform?.parent? // stock steering wheel's parent (could be null so '?' null operator use).
                                                        .name == "pivot_stock_wheel"); // Either the name of the parent is equal to null or 'pivot_sto....' .           

        // Static Properties
        /// <summary>
        /// Represents the audio for the dash buttons.
        /// </summary>
        private static AudioSource dashButtonAudioSource
        {
            get
            {
                return GameObject.Find("dash_button").GetComponent<AudioSource>();
            }
        }
        /// <summary>
        /// Represents whether the satsuma has power.
        /// </summary>
        private static bool hasPower
        {
            get
            {
                GameObject elect = GameObject.Find("SATSUMA(557kg, 248)/Electricity");
                PlayMakerFSM power = PlayMakerFSM.FindFsmOnGameObject(elect, "Power");
                return power.FsmVariables.FindFsmBool("ElectricsOK").Value;
            }
        }
        /// <summary>
        /// Represents the clear button of the ccUnit.
        /// </summary>
        private static GameObject clearButton
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the power button of the ccUnit.
        /// </summary>
        private static GameObject powerButton
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the res button of the ccUnit.
        /// </summary>
        private static GameObject resButton
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the set button of the ccUnit.
        /// </summary>
        private static GameObject setButton
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the wires of the ccUnit.
        /// </summary>
        private static GameObject wires
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets up the cruise control unit.
        /// </summary>
        private void Start()
        {
            // Written, 27.09.2018

            // Setting Clear Button
            clearButton = this.gameObject.transform.FindChild("BtnClr").gameObject;
            clearButton.AddComponent<MeshCollider>().convex = true;
            clearButton.name = "Clear";
            // Setting power Button
            powerButton = this.gameObject.transform.FindChild("BtnOnOff").gameObject;
            powerButton.AddComponent<MeshCollider>().convex = true;
            powerButton.name = "Power";
            // Setting Res Button
            resButton = this.gameObject.transform.FindChild("BtnRes").gameObject;
            resButton.AddComponent<MeshCollider>().convex = true;
            resButton.name = "Res";
            // Setting Set Button
            setButton = this.gameObject.transform.FindChild("BtnSet").gameObject;
            setButton.AddComponent<MeshCollider>().convex = true;
            setButton.name = "Set";
            // Setting Wires
            wires = this.gameObject.transform.FindChild("Wires").gameObject;
        }
        /// <summary>
        /// Runs every frame.
        /// </summary>
        private void Update()
        {
            // Written, 27.09.2018

            this.ccFunction();
        }

        /// <summary>
        /// The function of the cruise control unit.
        /// </summary>
        private void ccFunction()
        {
            // Written, 27.09.2018

            if (this.isStockSteeringWheelInstalled)
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1f, 1 << this.gameObject.layer))
                {
                    bool foundObject = false;
                    bool foundParent = false;

                    if (hit.transform?.gameObject?.name == "SATSUMA(557kg, 248)")
                    {
                        foundParent = true;
                        GameObject gameObjectHit;

                        gameObjectHit = hit.collider?.gameObject;

                        if (gameObjectHit != null)
                        {
                            Action actionToPerform = null;

                            if (gameObjectHit == clearButton)
                            {
                                foundObject = true;
                                actionToPerform = this.onClear;
                            }
                            else
                            {
                                if (gameObjectHit == powerButton)
                                {
                                    foundObject = true;
                                    actionToPerform = this.onPower;
                                }
                                else
                                {
                                    if (gameObjectHit == resButton)
                                    {
                                        foundObject = true;
                                        actionToPerform = this.onRes;
                                    }
                                    else
                                    {
                                        if (gameObjectHit == setButton)
                                        {
                                            foundObject = true;
                                            actionToPerform = this.onSet;
                                        }
                                    }
                                }
                            }
                            if (foundObject)
                            {
                                ModClient.guiInteract(gameObjectHit.name);
                                if (CruiseControlMod.useButtonDown)
                                {
                                    if (hasPower)
                                        actionToPerform.Invoke();
                                    AudioSource audio = dashButtonAudioSource;
                                    audio.transform.position = gameObjectHit.transform.position;
                                    audio.Play();
                                }
                            }
                        }
                    }
                    if (foundParent && !foundObject) // Only reset if gui use was active. (my attempt at preventing flashing graphics gitch in text.)
                    {
                        ModClient.guiInteract();
                    }
                }
            }
        }
        /// <summary>
        /// Occurs when the clear button is pressed.
        /// </summary>
        private void onClear()
        {
            ModConsole.Print("Clear button clicked");
        }
        /// <summary>
        /// Occurs when the power button is pressed.
        /// </summary>
        private void onPower()
        {
            ModConsole.Print("Power button clicked");
        }
        /// <summary>
        /// Occurs when the res button is pressed.
        /// </summary>
        private void onRes()
        {
            ModConsole.Print("Res button clicked");
        }
        /// <summary>
        /// Occurs when the set button is pressed.
        /// </summary>
        private void onSet()
        {
            ModConsole.Print("Set button clicked");
        }        

        #endregion
    }
}
