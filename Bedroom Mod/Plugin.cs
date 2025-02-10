using System;
using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using static OVRPlugin;

namespace Bedroom_Mod
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        Transform Stump;
        GameObject bedroom;
        Shader fxshd;
        private void Start()
        {
            GorillaTagger.OnPlayerSpawned(() => StartCoroutine(LoadAssetBundleCoroutine()));
        }
        private IEnumerator LoadAssetBundleCoroutine()
        {
            using (Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bedroom_Mod.Assets.bedroom"))
            {
                if (str == null)
                {
                    yield break;
                }

                AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromStreamAsync(str);
                yield return bundleRequest;
                AssetBundle bundle = bundleRequest.assetBundle;
                if (bundle == null)
                {
                    yield break;
                }
                InitializeBedroom(bundle);
            }
        }

        private void InitializeBedroom(AssetBundle bundle)
        {
            bedroom = Instantiate(bundle.LoadAsset<GameObject>("bedroom"));
            if (bedroom == null)
            {
                return;
            }
            GameObject.Find("screen")?.SetActive(false);
            GameObject.Find("COC Text")?.SetActive(false);
            GameObject.Find("CodeOfConduct")?.SetActive(false);
            fxshd = bedroom.transform.FindChildRecursive("custom screen (1)").GetComponent<Renderer>().material.shader;
            bedroom.transform.FindChildRecursive("custom screen (1)").GetComponent<Renderer>().material.mainTexture = LoadEmbeddedTexture("Bedroom_Mod.Assets.pic.png");
            FixShader(bedroom.transform);

            Stump = bedroom.transform.GetChild(5);
            if (Stump != null)
            {
                AdjustStumpTransforms();
                RemoveUnnecessaryChildren();
            }
            bedroom.transform.SetParent(GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom")?.transform);
            bundle.UnloadAsync(false);
            Destroy(this);
        }
        void AdjustStumpTransforms()
        {
            Stump.GetChild(0).localPosition = new Vector3(2.6818f, -9.2773f, 0.3f);
            Stump.GetChild(0).localRotation = Quaternion.Euler(0, 0, 27.5511f);

            Stump.GetChild(8).localRotation = Quaternion.Euler(Vector3.zero);
            Stump.GetChild(8).localPosition = new Vector3(0.7589f, -0.4334f, 1.1661f);

            Stump.GetChild(13).localPosition = new Vector3(5.1252f, -2.8832f, 1.424f);
            Stump.GetChild(13).localRotation = Quaternion.Euler(2.6766f, 186.1166f, 243.9278f);
        }
        void RemoveUnnecessaryChildren()
        {
            Stump.GetChild(14)?.gameObject.SetActive(false);
            Stump.GetChild(5)?.gameObject.SetActive(false);
            Stump.GetChild(6)?.gameObject.SetActive(false);
            Stump.GetChild(11)?.gameObject.SetActive(false);

            foreach (Transform t in bedroom.transform)
            {
                if (t != Stump)
                {
                    Destroy(t.gameObject);
                }
            }
        }

        void FixShader(Transform t)
        {
            foreach (Transform tr in t)
            {
                FixShader(tr);
            }
            Renderer renderer = t.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.shader = fxshd;
            }
        }

        private Texture2D LoadEmbeddedTexture(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);

                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(imageData))
                {
                    texture.filterMode = FilterMode.Point;
                    return texture;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
