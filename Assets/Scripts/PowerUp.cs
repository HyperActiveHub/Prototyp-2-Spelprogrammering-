using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Power-Up", menuName = "PowerUp")]
public class PowerUp : ScriptableObject
{
    public Sprite sprite;
    //[Tooltip("How long will this power-up last (in seconds)? Zero = infinity (must break somehow)")]    //Use assert to make sure the pwr-up can break if duration = 0;
    //public float duration;

    //temp
    [HideInInspector] public float value_f;
    [HideInInspector] public int value_i;
    [HideInInspector] public Vector3 value_v3;
    /*[HideInInspector]*/ public List<int> intList = new List<int>();
    //temp

    //public List<PowerUpFunctionsScript.PowerUpFunctions> puFunctinos = new List<PowerUpFunctionsScript.PowerUpFunctions>();
    public List<System.Reflection.MethodInfo> functions = new List<System.Reflection.MethodInfo>();
    public List<System.Reflection.MethodInfo> actions;

    [HideInInspector]
    public int selected, actionSelected;
    [SerializeField]
    public List<System.Reflection.ParameterInfo> pwrUpParameters = new List<System.Reflection.ParameterInfo>();
    public System.Reflection.ParameterInfo[][] parameters;

    private void OnValidate()
    {
        OnChange();
        if (intList.Count == 0)
        {
            intList.Add(0);
        }
    }

    public void OnChange()
    {
        GameManager.PwrUpChanged();

        //check if list changed first
        functions = PowerUpFunctionsScript.GetPowerUpFunctions(out parameters, out actions);
        for (int i = 0; i < parameters.Length; i++)
        {
            for (int j = 0; j < parameters[i].Length; j++)
            {
                pwrUpParameters.Add(parameters[i][j]);
            }
        }
    }

}