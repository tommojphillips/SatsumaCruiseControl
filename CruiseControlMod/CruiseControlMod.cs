using MSCLoader;
using UnityEngine;
using ModApi.Attachable;

namespace CruiseControlMod
{
    //CruiseControl Mod Class
    //Remember to check the DLL references:
    // cInput.dll
    // modapi_v0114-alpha.dll
    // MSCLoader.dll
    // PlayMaker.dll
    // UnityEngine.dll
    public class CruiseControlMod : Mod
    {
        public override string ID { get { return "CruiseControl"; } }
        public override string Name { get { return "Satsuma Cruise Control"; } }
        public override string Author { get { return "Nitro Pascal, tommojphillips"; } }
        public override string Version { get { return "0.14"; } }
        public override bool UseAssetsFolder => true;

        /// <summary>
        /// Represents the file name for the save file.
        /// </summary>
        private const string fileName = "SatsumaCCSaveData.txt";
        /// <summary>
        /// Represents the Cruise Control 3d model as a gameobject.
        /// </summary>
        private GameObject ccPanel;
        /// <summary>
        /// Represents the cruise control panel game object.
        /// </summary>
        private CruiseControlPanel cruiseControlPanel;
        /// <summary>
        /// Represents whether the use button is being held down.
        /// </summary>
        internal static bool useButtonDown
        {
            get
            {
                return cInput.GetKeyDown("Use");
            }
        }
        /// <summary>
        /// Loads save data.
        /// </summary>
        /// <returns></returns>
        private PartSaveInfo loadSaveData()
        {
            // Written, 12.10.2018

            try
            {
                return SaveLoad.DeserializeSaveFile<PartSaveInfo>(this, fileName);
            }
            catch (System.NullReferenceException)
            {
                // no save file exists.. //loading default save data.

                return null;
            }
        }
        //Called when mod is loading
        public override void OnLoad()
        {
            try
            {                
                // Creating "ccpanel"
                GameObject temp_ccPanel;
                AssetBundle bundle = LoadAssets.LoadBundle(this, "cruiseassets.unity3d");
                temp_ccPanel = bundle.LoadAsset("ccobject.prefab") as GameObject;
                // Making panel owner of buttons and wires.
                this.ccPanel = temp_ccPanel.transform.FindChild("Panel").gameObject;
                temp_ccPanel.transform.FindChild("BtnClr").gameObject.transform.SetParent(this.ccPanel.transform);
                temp_ccPanel.transform.FindChild("BtnOnOff").gameObject.transform.SetParent(this.ccPanel.transform);
                temp_ccPanel.transform.FindChild("BtnRes").gameObject.transform.SetParent(this.ccPanel.transform);
                temp_ccPanel.transform.FindChild("BtnSet").gameObject.transform.SetParent(this.ccPanel.transform);
                temp_ccPanel.transform.FindChild("Wires").gameObject.transform.SetParent(this.ccPanel.transform);
                // Setting scale of panel.
                this.ccPanel.transform.localScale = CruiseControlPanel.scale;
                // Adding rigidbody and mesh collider.
                this.ccPanel.AddComponent<Rigidbody>();
                this.ccPanel.AddComponent<MeshCollider>().convex = true;
                // loading part data
                PartSaveInfo ccPanel_partSaveInfo = this.loadSaveData();
                // Creating ccpanel part.
                this.ccPanel.name = "Cruise Control Panel";
                GameObject _parent = GameObject.Find("stock steering wheel(Clone)");
                Trigger trigger = new Trigger("ccPanelTrigger", _parent, new Vector3(0.112f, 0.042f, 0.016f), Quaternion.Euler(0, 0, 80.00001f), new Vector3(0.1f, 0.1f, 0.1f), false);
                this.cruiseControlPanel = new CruiseControlPanel(ccPanel_partSaveInfo, this.ccPanel, _parent, trigger, new Vector3(0.112f, 0.042f, 0.016f), Quaternion.Euler(0, 0, 80.00001f));
                // Destorying ccpanel copy.
                Object.Destroy(this.ccPanel);
                // Unloading bundle.
                bundle.Unload(false);
                ModConsole.Print(string.Format("{0}, v{1}: Loaded", this.Name, this.Version));

            }
            catch (System.Exception ex)
            {
                ModConsole.Error(string.Format("Error: {0}\r\nStacktrace: {1}", ex.Source, ex.StackTrace));
            }
        }
        //Called when player goes to the toilet
        public override void OnSave()
        {
            SaveLoad.SerializeSaveFile(this, this.cruiseControlPanel.getPartSaveInfo, fileName);
        }
    }
}
