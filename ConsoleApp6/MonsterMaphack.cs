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
        public List<LocalMonster> monsters = new List<LocalMonster>();
        public List<Renderer> monsterRenderers = new List<Renderer>();
        public Coroutine maphackRoutine;
        public Coroutine meshRenderRoutine;
        public void Awake()
        {
            maphackRoutine = StartCoroutine(CoMaphackRoutine());
            meshRenderRoutine = StartCoroutine(CoMeshRenderer());
          
        }
        public IEnumerator CoUpdateMonsterObjects()
        {
            var findMonsters = FindObjectsOfType<LocalMonster>();
            this.monsters.Clear();
            this.monsters.AddRange(findMonsters);
            OnMonsterCountChanged();
            yield return new WaitForSeconds(1);
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
            foreach (var target in monsters)
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
                    foreach (var monster in monsters)
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
                    yield return CoUpdateMonsterObjects();
                    foreach (var v in monsters)
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


        void DrawCube()
        {
            if (enable == false) return;
            foreach (var monster in monsters)
            {
                if (monster.IsAlive)
                { 
                    var dist = GameUtil.DistanceOnPlane(CheatMain.instance.mine.GetPosition(), monster.GetPosition());
                    if(dist <= 75)
                    {
                        Popcron.Gizmos.Cube(monster.GetPosition(), monster.GetRotation(), monster.gameObject.transform.localScale, Color.yellow); 
                    } 
                }
            }
        }
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.CapsLock))
            {
                BearNotify();
            }
            DrawCube();
        }
        void OnGUI()
        {
            GUILayout.Label(gui.ToString());
            if (gui)
            {
                GUILayout.Label("----Monster----");
                GUILayout.Label("Monster Mesh Render Count : " + monsterRenderers.Count);
                GUILayout.Label("Monster Count : " + monsters.Count);
                
                if(GUILayout.Button("Update Force Monster"))
                {
                    monsters.ForEach(x => { x.OnSight(); });
                }
            }
        }

    }
}
