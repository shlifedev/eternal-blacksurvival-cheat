using Blis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blis.Client.Cheat
{
    public class GameDataDumper : CheatSingleton<GameDataDumper>
    {
        void OnGUI()
        {
            if (gui)
            {
                GUILayout.Label(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                if (GUILayout.Button("Dump"))
                {
                    System.IO.File.WriteAllText("GetAllItems.txt", Newtonsoft.Json.JsonConvert.SerializeObject(GameDB.item.GetAllItems()));
                    System.IO.File.WriteAllText("GetAllCharacterData.txt", Newtonsoft.Json.JsonConvert.SerializeObject(GameDB.character.GetAllCharacterData()));
                    System.IO.File.WriteAllText("GetAllSummonData.txt", Newtonsoft.Json.JsonConvert.SerializeObject(GameDB.character.GetAllSummonData()));
                    System.IO.File.WriteAllText("GetAllMonsterData.txt", Newtonsoft.Json.JsonConvert.SerializeObject(GameDB.monster.GetAllMonsterData()));
                }
            }
        } 
    }
}
