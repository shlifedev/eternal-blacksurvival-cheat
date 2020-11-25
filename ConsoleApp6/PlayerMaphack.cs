using Blis.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using BIFog;
using Blis.Client.UI;
using Blis.Common;
using Blis.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Blis.Client.Cheat
{
    public class PlayerMaphack : CheatSingleton<PlayerMaphack>
    {
        public List<Renderer> playerRenderers = new List<Renderer>();
        public Coroutine maphackRoutine;
        public Coroutine meshRenderRoutine;
        public void Awake()
        {
            Log("PlayerMaphack Initalized!");
            maphackRoutine = StartCoroutine(CoMaphackRoutine());
            meshRenderRoutine = StartCoroutine(CoMeshRenderer());
        }

        private float Distance(LocalPlayerCharacter target)
        {
            return Vector3.Distance(CheatMain.instance.mine.GetPosition(), target.GetPosition());
        }

        public IEnumerator CoMeshRenderer()
        {
            //wait until..
            while (CheatMain.instance.IsInit() == false) 
                yield return null;

            Log("CoMeshRenderer Intialized!");
            /* Initalize */
            playerRenderers.Clear();
            foreach (var target in CheatMain.instance.players)
            {
                if (target == CheatMain.instance.mine) continue;
                playerRenderers.AddRange(target.gameObject.GetComponents<Renderer>());
                playerRenderers.AddRange(target.gameObject.GetComponentsInParent<Renderer>());
                playerRenderers.AddRange(target.gameObject.GetComponentsInChildren<Renderer>());
            }
            while (true)
            {
                if (enable)
                {
                    foreach (var renderer in playerRenderers)
                    {
                        renderer.enabled = true;
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }

        }
        public IEnumerator CoMaphackRoutine()
        {
            //wait until..
            while (CheatMain.instance.IsInit() == false)
                yield return null;

            Log("CoMaphackRoutine Intialized!");
            while (true)
            {
                if (enable)
                {
                    foreach (var v in CheatMain.instance.players)
                    {
                        if (v != null)
                        {
                            v.ShowMiniMapIcon(MiniMapIconType.None);
                            v.ShowMapIcon(MiniMapIconType.None);
                            if (CheatMain.instance.mine != v)
                            {
                                //거리가 100미터안에 드는경우만 표시
                                if (Distance(v) <= 50)
                                {
                                    v.FloatingUi.Show();
                                }
                            }
                        }
                        yield return null;
                    }
                }
                yield return new WaitForSeconds(0.015f);
            }
        }

        void OnGUI()
        {
            if (gui)
            {
                GUILayout.Label("Player Mesh Render Count : " + playerRenderers.Count);
                foreach (var target in CheatMain.instance.players)
                {
                    if (GUILayout.Button($"{target.Nickname} dist : " + Distance(target).ToString("0.0") + "m"))
                    {

                    }
                }
            }
        }

    }
}
