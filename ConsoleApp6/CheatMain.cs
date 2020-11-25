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
    public class CheatMain : CheatSingleton<CheatMain>
    {
        public LocalPlayerCharacter mine;
        public List<LocalPlayerCharacter> players = new List<LocalPlayerCharacter>();
        public List<LocalMonster> monsters = new List<LocalMonster>();
        public LocalWorld world;

        System.Action onUpdateMonsterCount;
        public void Awake()
        {
        }
        public bool init = false;

        public bool IsInit()
        {
            return init;
        }

        public bool IsMine(LocalPlayerCharacter target)
        {
            return mine == target;

        }


        public void RegMonsterCountUpdateEvent(System.Action callback)
        {
            this.onUpdateMonsterCount += callback;
        }

        public IEnumerator CoUpdateMonsterObjects()
        {
            while (init == false)
                yield return null;

            while (true)
            {
                
                var findMonsters = FindObjectsOfType<LocalMonster>();
                
                int oldCount = monsters.Count;
                monsters.Clear();
                monsters.AddRange(findMonsters);
                if (oldCount != findMonsters.Length)
                {
                    onUpdateMonsterCount?.Invoke(); 
                    Log("몬스터 데이터가 업데이트 되었습니다.");
                }

                yield return new WaitForSeconds(1);
            }
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Insert))
            {
                StartCoroutine(CoGameDataUpdator());
                StartCoroutine(CoUpdateMonsterObjects());
            }
        }
        public IEnumerator CoGameDataUpdator()
        {
            while (true)
            {
                if (init == false)
                {
                    //내 플레이어 id 찾기
                    var me = PlayerController.inst.myObjectId;

                    //월드 찾기
                    world = ClientService.inst.World;

                    //플레이어 정보 업데이트
                    var players = FindObjectsOfType<LocalPlayerCharacter>();

                    this.players.AddRange(players);

                    this.players.ForEach(x =>
                    {
                        if (x.ObjectId == me)
                        {
                            mine = x;
                        }
                    });


                    //스타트 치트
                    PlayerMaphack.instance.CreateInstance();
                    MonsterMaphack.instance.CreateInstance();

                    init = true; 
                }
                else
                { 
                }
                yield return null;
            }
        }



    }

}