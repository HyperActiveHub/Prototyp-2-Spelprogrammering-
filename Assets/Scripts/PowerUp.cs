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

        AddProperties(this);


    }

    List<List<int>> indexLists = new List<List<int>>();
    public List<System.Type> types;

    public HashSet<T> GetUniqueElements<T>(List<T> list)
    {
        return new HashSet<T>(list);
    }

    List<List<System.Type>> GetAllTypeLists(List<System.Type> types, HashSet<System.Type> uniqueTypes)
    {
        List<List<System.Type>> typeLists = new List<List<System.Type>>();
        indexLists.Clear();
        foreach (var element in uniqueTypes)
        {
            List<System.Type> typeList = new List<System.Type>();
            int firstIndex = types.IndexOf(element);
            int lastIndex = types.LastIndexOf(element);
            int diff = lastIndex - firstIndex;

            if (diff == 0)      //if theres only one parameter of type element first and last index will be the same, GetRange(index, index) returns an empty list.
            {
                typeList = new List<System.Type>() { element };
            }
            else
            {
                typeList = types.GetRange(firstIndex, diff + 1);
            }
            typeLists.Add(typeList);

            List<int> indexList = new List<int>();
            for (int i = 0; i < typeList.Count; i++)
            {
                indexList.Add(i);
            }
            indexLists.Add(indexList);
        }
        return typeLists;
    }

    public int GetTypeIndex(System.Type type, HashSet<System.Type> uniqueTypes)
    {
        if (uniqueTypes.Contains(type))
        {
            List<System.Type> list = new List<System.Type>(uniqueTypes);
            return list.IndexOf(type);
        }
        else
        {
            Debug.LogError("Type not found", this);
            return -1;
        }
    }

    public int GetPropertyIndex(System.Type type, HashSet<System.Type> uniqueTypes)
    {
        //Get the typeIndex... 
        //Get the index of the first element in indexLists[typeIndex]
        //remove that index from that list (to ensure it is unable to be accidentally used for another parameter as well.)

        int typeIndex = GetTypeIndex(type, uniqueTypes);

        if (indexLists[typeIndex].Count != 0)
        {
            int propertyIndex = indexLists[typeIndex][0];       //maybe use a stack instead?
            indexLists[typeIndex].Remove(propertyIndex);
            return propertyIndex;
        }
        else
        {
            Debug.LogError("Property index not found (too few elements in typeList)", this);
            return -1;
        }
    }

    public int GetIndexOfProperty(System.Type type)
    {

    }

    public void AddProperties(PowerUp pwrUp)
    {
        List<System.Reflection.ParameterInfo[]> parameters = new List<System.Reflection.ParameterInfo[]>(pwrUp.parameters);
        int intCount = 0, floatCount = 0, v3Count = 0;

        types = new List<System.Type>();

        foreach (var p in parameters)
        {
            foreach (var e in p)
            {
                types.Add(e.ParameterType);
                //int index = 0;//parameters.IndexOf/;
                //isParameterIndexUsed.Add(index, false);
                //Debug.Log("param '" + e.ParameterType + "', index: " + index);
            }
        }

        //temp
        foreach (var pType in types)    //uhhh..
        {
            if (pType == typeof(int))
            {
                intCount++;
            }
            else if (pType == typeof(float))
            {
                floatCount++;
            }
            else if (pType == typeof(Vector3))
            {
                v3Count++;
            }
        }

        AddToList(pwrUp.intList, intCount);
        //temp

        types.Sort(CompareTypes);

        var unique = GetUniqueElements(types);

        var allTypeLists = GetAllTypeLists(types, unique);
    }

    private static int CompareTypes(System.Type a, System.Type b)
    {
        return a.Name.CompareTo(b.Name);
    }

    public void AddToList<T>(List<T> list, int listCount)
    {
        while (list.Count < listCount)
        {
            var value = (list[0]);      //copy, dont use the same value...
            list.Add(value);
            Debug.Log("Value added to int list. Count: " + list.Count);
        }
    }
}