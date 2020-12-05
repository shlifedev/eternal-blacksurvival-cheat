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
            yield return new WaitForEndOfFrame();
            this.monsters.AddRange(findMonsters);
            yield return new WaitForEndOfFrame();
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
        public void WolfNotify()
        {

            if (enable)
            {
                foreach (var monster in monsters)
                {
                    if (monster.MonsterType == MonsterType.Wolf && monster.IsAlive)
                    {
                        GameObject minimapObj = MonoBehaviourInstance<GameUI>.inst.Minimap.UIMap.AddPing(PingType.Help, monster.GetPosition());
                        GameObject mapwindowObj = MonoBehaviourInstance<GameUI>.inst.MapWindow.UIMap.AddPing(PingType.Help,  monster.GetPosition());
                        Destroy(minimapObj, 5);
                        Destroy(mapwindowObj, 5);

                    }
                }
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
                            Destroy(minimapObj, 5);
                            Destroy(mapwindowObj, 5);

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
                yield return new WaitForSeconds(0.5f);
            } 
        }

        public int i = 0;
        void OnGUI()
        { 
            if (GUILayout.Button("Bear"))
            {
                if (Input.GetKeyUp(KeyCode.CapsLock))
                {
                    BearNotify();
                }
            }
            if (GUILayout.Button("Wolf"))
            {
                if (Input.GetKeyUp(KeyCode.CapsLock))
                {
                    BearNotify();
                }
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
                            if(v.IsAlive)
                            {
                                v.ShowMiniMapIcon(MiniMapIconType.None);
                            }
                            else
                            {
                                v.HideMiniMapIcon(MiniMapIconType.None);
                            } 
                        }
                        yield return null;
                    }
                    i++;
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
                WolfNotify(); 
            }

            DrawCube();
        } 

    }
}
