using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public static class PowerUpFunctionsScript
{
    //public delegate void PowerUpFunctions();
    //public static List<PowerUpFunctions> powerUpFunctions = new List<PowerUpFunctions>();

    public static void Invulnerability()
    {
        Debug.Log("This is a test function for powerups.");
    }

    public static void Shield(float radius, Vector2 offset)
    {
        Debug.Log("Make a shield");
    }


    public static void ExtraJumpHeight(int extraHeight)
    {
        Debug.Log("This is yet another test function for powerups.");
    }

    /// <summary>
    /// Only invoke this once. It implements reflection and should only need to be called when the list of functions has changed anyway, which needs to be done by code.
    /// </summary>
    /// <returns></returns>
    public static List<MethodInfo> GetPowerUpFunctions(out ParameterInfo[][] parameters)
    {
        MethodInfo[] methods = typeof(PowerUpFunctionsScript).GetMethods(BindingFlags.Public | BindingFlags.Static);
        List<MethodInfo> pwrUpFunctions = new List<MethodInfo>(methods);
        MethodInfo currentMethod = (MethodInfo)MethodBase.GetCurrentMethod();

        if (pwrUpFunctions.Contains(currentMethod))
        {
            pwrUpFunctions.Remove(currentMethod);
        }

        parameters = new ParameterInfo[(pwrUpFunctions.Count)][];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = pwrUpFunctions[i].GetParameters();
        }
        
        return pwrUpFunctions;
    }
}
