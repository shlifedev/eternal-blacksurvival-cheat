using Blis.Client.Cheat.NLogHook;
using UnityEngine;

 
public class StaticLoader
{
	/// <summary>
	/// 함수 사이닝값
	/// 55 48 8B EC 48 83 EC 20 49 BA ??
	/// 
	/// 두패턴비교
	/// 55 48 8B EC 48 83 EC 20 49 BA 88 4B 33 C0 86 01 00 00 48 8D 6D 00 49 BB 8A C7 04 E7 86 01 00 00 41 FF D3 83 38 00 48 8D 65 00 5D C3
	/// 55 48 8B EC 48 83 EC 20 49 BA C0 08 A2 3C 9B 01 00 00 48 8D 6D 00 49 BB AA A6 C3 66 9B 01 00 00 41 FF D3 83 38 00 48 8D 65 00 5D C3
	/// </summary>
	[RuntimeInitializeOnLoadMethod]
	public static void StaticLoad()
	{
		CheatBehaviour.instance.CreateInstance(true);
	}
}