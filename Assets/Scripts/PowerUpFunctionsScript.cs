using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public static class PowerUpFunctionsScript //: MonoBehaviour
{
    //public static PowerUpFunctionsScript Instance;

    static PlayerScript player;

    //public delegate void PowerUpFunctions();
    //public static List<PowerUpFunctions> powerUpFunctions = new List<PowerUpFunctions>();

    private static void Start()
    {
        player = PlayerScript.Instance;
    }
    public static void Invulnerability()
    {
        Debug.Log("This is a test function for powerups.");
    }
    public static void Shield(int radius, Vector2 offset)
    {
        Debug.Log("Make a shield");
    }
    public static void ExtraJumpHeight(float extraHeight)
    {
         player.AddJumpHeight(extraHeight);
        //StartCoroutine(ExecuteMethodInSeconds(ResetJumpHeight, 3/*temp*/));
    }
    public static void AddTwoInts(int firstInt, int secondInd)
    {
        int res = firstInt + secondInd;
        //test method.
    }
    public static void ResetJumpHeight()
    {
        player.ResetJumppHeight();
    }

    public static IEnumerator ExecuteMethodInSeconds(System.Action method, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        method.Invoke();
    }

    /// <summary>
    /// Only invoke this once. It implements reflection and should only need to be called when the list of functions has changed anyway, which needs to be done by code.
    /// </summary>
    /// <returns></returns>
    public static List<MethodInfo> GetPowerUpFunctions(out ParameterInfo[][] parameters, out List<MethodInfo> actionList)    //maybe also send/recieve pwr-up duration (life-time) in this
    {
        MethodInfo[] methods = typeof(PowerUpFunctionsScript).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
        List<MethodInfo> pwrUpFunctions = new List<MethodInfo>(methods);
        MethodInfo currentMethod = (MethodInfo)MethodBase.GetCurrentMethod();

        if (pwrUpFunctions.Contains(currentMethod))
        {
            pwrUpFunctions.Remove(currentMethod);
        }

        actionList = new List<MethodInfo>();
        parameters = new ParameterInfo[(pwrUpFunctions.Count)][];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = pwrUpFunctions[i].GetParameters();

            if(parameters[i].Length == 0)
            {
                actionList.Add(pwrUpFunctions[i]);
            }
        }

        return pwrUpFunctions;
    }

}
