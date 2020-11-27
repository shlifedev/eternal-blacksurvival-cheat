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
    public class TrapFinder : CheatSingleton<TrapFinder>
    {
        List<LocalSummonTrap> traps = new List<LocalSummonTrap>();

        public IEnumerator CoTrapFindRoutine()
        {
            while (true)
            {
                if (enable)
                {
                    traps.Clear();
                    traps.AddRange(FindObjectsOfType<LocalSummonTrap>()); 
                }
                yield return new WaitForSeconds(1);
                Log("Trap Count : " + traps.Count);
            }
        }

        void Awake()
        {
            StartCoroutine(CoTrapFindRoutine());
        }
        public void Update()
        {
            if(enable == false) return;
            if (CheatMain.instance.mine != null)
            {
                foreach (var trap in traps)
                {
                    var owner = trap.IsOwner(CheatMain.instance.mine); 
                    if (trap.IsAlive)
                    {
                        Popcron.Gizmos.Cube(trap.GetPosition(), trap.GetRotation(), new Vector3(0.5f, 0.5f, 0.5f), Color.red);
                    }
                }
            }
        }
    }
}
