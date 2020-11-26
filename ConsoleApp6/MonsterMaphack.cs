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
    public class MonsterMaphack : CheatSingleton<MonsterMaphack>
    {
        public List<Renderer> monsterRenderers = new List<Renderer>();
        public Coroutine maphackRoutine;
        public Coroutine meshRenderRoutine;
        public void Awake()
        {
            maphackRoutine = StartCoroutine(CoMaphackRoutine());
            meshRenderRoutine = StartCoroutine(CoMeshRenderer());
          
        }

        private float Distance(LocalPlayerCharacter target)
        {
            return Vector3.Distance(CheatMain.instance.mine.GetPosition(), target.GetPosition());
        }


        /// <summary>
        /// 몬스터 숫자 변경시 렌더러 업데이트
        /// </summary>
        public void OnMonsterCountChanged()
        { 
            monsterRenderers.Clear();
            foreach (var target in CheatMain.instance.monsters)
            { 
                monsterRenderers.AddRange(target.gameObject.GetComponents<Renderer>());
                monsterRenderers.AddRange(target.gameObject.GetComponentsInParent<Renderer>());
                monsterRenderers.AddRange(target.gameObject.GetComponentsInChildren<Renderer>());
            } 
        }
         
        public void BearNotify()
        {
            
                if (enable)
                {
                    foreach (var monster in CheatMain.instance.monsters)
                    {
                        if (monster.MonsterType == MonsterType.Bear && monster.IsAlive)
                        {
                            GameObject minimapObj = MonoBehaviourInstance<GameUI>.inst.Minimap.UIMap.AddPing(PingType.Warning, monster.GetPosition());
                            GameObject mapwindowObj = MonoBehaviourInstance<GameUI>.inst.MapWindow.UIMap.AddPing(PingType.Warning,  monster.GetPosition());
                        }
                    }
                } 
        }

        public IEnumerator CoMeshRenderer()
        {
            //wait until..
            while (CheatMain.instance.IsInit() == false)
                yield return null; 
            Log("CoMeshRenderer Initalized!");
            /* Initalize */ 
            while (true)
            {
                if (enable)
                {
                    foreach (var renderer in monsterRenderers)
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
            Log("CoMaphackRoutine Initalized!");
            while (true)
            {
                if (enable)
                {
                    foreach (var v in CheatMain.instance.monsters)
                    {
                        if (v != null)
                        {
                            v.ShowMiniMapIcon(MiniMapIconType.None);
                            v.ShowMapIcon(MiniMapIconType.None);
                        }
                        yield return null;
                    }
                }
                yield return new WaitForSeconds(0.015f);
            }
        }


        void Update()
        {
            if(Input.GetKeyUp(KeyCode.CapsLock))
            {
                BearNotify();
            }
        }
        void OnGUI()
        {
            if (gui)
            {
                GUILayout.Label("Monster Mesh Render Count : " + monsterRenderers.Count);
            }
        }

    }
}
