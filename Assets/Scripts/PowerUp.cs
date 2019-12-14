using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct IntHolder
{
    public List<bool> isUsed;
    public List<int> value;
}


[CreateAssetMenu(fileName = "New Power-Up", menuName = "PowerUp")]
public class PowerUp : ScriptableObject
{
#if UNITY_EDITOR
    //temp
    [HideInInspector] public float value_f;
    [HideInInspector] public int value_i;
    [HideInInspector] public Vector3 value_v3;
    [HideInInspector]
    public List<int> intList = new List<int>
    {
        0,0,0   //this works! just need to figure out how/when to add new elements..
    };
    //temp

    //public List<IntHolder> intHolder = new List<IntHolder>();
    //public IntHolder intHolder = new IntHolder();

    //public List<PowerUpFunctionsScript.PowerUpFunctions> puFunctinos = new List<PowerUpFunctionsScript.PowerUpFunctions>();
    public List<System.Reflection.MethodInfo> puFunctinos = new List<System.Reflection.MethodInfo>();

    [HideInInspector]
    public int selected;

    public enum types { integer, floating, boolean, Vector2D, Rectangle }


    [SerializeField]
    public List<System.Reflection.ParameterInfo> pwrUpProperties = new List<System.Reflection.ParameterInfo>();



    //public myParameters Parameters;
    public System.Reflection.ParameterInfo[][] properties;

    private void OnValidate()
    {
        OnChange();

    }

    public void OnChange()
    {
        GameManager.PwrUpChanged();

        //check if list changed first
        puFunctinos = PowerUpFunctionsScript.GetPowerUpFunctions(out properties);
        for (int i = 0; i < properties.Length; i++)
        {
            for (int j = 0; j < properties[i].Length; j++)
            {
                pwrUpProperties.Add(properties[i][j]);
            }
        }

        Debug.Log("Selected: " + puFunctinos[selected].Name);

    }
#endif



    //select all *tag*

    public Sprite sprite;
    [Tooltip("How long will this power-up last (in seconds)? Zero = infinity (must break somehow)")]    //Use assert to make sure the pwr-up can break if duration = 0;
    public float duration;

    //temp
    public float extraJumpHeight;

}
