using System;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using Utilla;

namespace Bedroom_Mod
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
		public GameObject bedroom;
		void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
			bedroom.SetActive(true);
			GameObject.Find("screen").SetActive(false);
			GameObject.Find("COC Text").SetActive(false);
			GameObject.Find("CodeOfConduct").SetActive(false);
		}

        void OnDisable()
        {
			/* Undo mod setup here */
			/* This provides support for toggling mods with ComputerInterface, please implement it :) */
			/* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

			HarmonyPatches.RemoveHarmonyPatches();
			Utilla.Events.GameInitialized -= OnGameInitialized;
			bedroom.SetActive(false);
			GameObject.Find("screen").SetActive(true);
			GameObject.Find("COC Text").SetActive(true);
			GameObject.Find("CodeOfConduct").SetActive(true);
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			/* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
			Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bedroom_Mod.Assets.bedroom");
			AssetBundle bundle = AssetBundle.LoadFromStream(str);
			GameObject sluber = bundle.LoadAsset<GameObject>("bedroom");
			bedroom = Instantiate(sluber);
			bedroom.SetActive(true);
			GameObject.Find("screen").SetActive(false);
			GameObject.Find("COC Text").SetActive(false);
			GameObject.Find("CodeOfConduct").SetActive(false);
			GameObject.Find("custom Monitors Screens").SetActive(false);
		}

		void Update()
		{
            /*if (!GameObject.Find("_Teleporter(Clone)").activeSelf)
            {
                GameObject.Find("Bed stump").SetActive(false);
                GameObject.Find("pillow stump").SetActive(false);

            }
            else
            {
                GameObject.Find("Bed stump").SetActive(true);
                GameObject.Find("pillow stump").SetActive(true); */
            }
        }

		/* This attribute tells Utilla to call this method when a modded room is joined */
		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			/* Activate your mod here */
			/* This code will run regardless of if the mod is enabled*/

			inRoom = true;
		}

		/* This attribute tells Utilla to call this method when a modded room is left */
		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			/* Deactivate your mod here */
			/* This code will run regardless of if the mod is enabled*/

			inRoom = false;
		}
	}
}