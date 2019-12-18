using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(PowerUp))]
public class PowerUpEditor : Editor
{
    int selected = 0;
    int actionSelected = 0;
    List<List<int>> indexLists = new List<List<int>>();
    List<System.Type> types;

    void AddToList<T>(List<T> list, int listCount)
    {
        while (list.Count < listCount)
        {
            var value = (list[0]);      //copy, dont use the same value...
            list.Add(value);
            Debug.Log("Value added to int list. Count: " + list.Count);
        }
    }

    void FunctionsMenu(PowerUp pwrUp)
    {
        List<string> optionList = new List<string>();
        var functionList = pwrUp.functions;
        foreach (var function in functionList)
        {
            optionList.Add(function.Name);
        }

        selected = EditorGUILayout.Popup("Power-Up Function", selected, optionList.ToArray());
        pwrUp.selected = selected;

        EditorGUILayout.LabelField(optionList[selected], EditorStyles.boldLabel);   //use the same kind of indexing for getting correct properties?
        //EditorGUILayout.HelpBox("This is a square message info in inspector", MessageType.Info, false);

    }

    void ActionsMenu(PowerUp pwrUp)
    {
        List<string> optionsList = new List<string>();
        var actionsList = pwrUp.actions;
        foreach (var action in actionsList)
        {
            optionsList.Add(action.Name);
        }
        actionSelected = EditorGUILayout.Popup("Action", actionSelected, optionsList.ToArray());
        pwrUp.actionSelected = actionSelected;
    }
    private static int CompareTypes(System.Type a, System.Type b)
    {
        return a.Name.CompareTo(b.Name);
    }

    HashSet<T> GetUniqueElements<T>(List<T> list)
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

    int GetTypeIndex(System.Type type, HashSet<System.Type> uniqueTypes)
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

    int GetPropertyIndex(System.Type type, HashSet<System.Type> uniqueTypes)
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

    void AddProperties(PowerUp pwrUp)
    {
        List<ParameterInfo[]> parameters = new List<ParameterInfo[]>(pwrUp.parameters);
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

    private void OnEnable()
    {
        //AddProperties((PowerUp)target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PowerUp pwrUp = (PowerUp)target;
        FunctionsMenu(pwrUp);

        AddProperties((PowerUp)target);

        System.Type type;
        string name = "";

        if (pwrUp.parameters[selected].Length != 0)     //the function has atleast one parameter
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            type = pwrUp.parameters[selected][0].ParameterType;
            name = pwrUp.parameters[selected][0].Name;

            foreach (System.Reflection.ParameterInfo parameter in pwrUp.parameters[selected])
            {
                SerializedProperty property = null;
                type = parameter.ParameterType;
                name = parameter.Name;
                int propertyIndex = GetPropertyIndex(type, GetUniqueElements(types));

                if (type == typeof(int))
                {
                    
                    property = serializedObject.FindProperty("intList");
                    property = property.GetArrayElementAtIndex(propertyIndex);      //need to figure out a way to index the parameters.
                    EditorGUILayout.PropertyField(property, new GUIContent(name));

                }
                else if (type == typeof(float))
                {
                    property = serializedObject.FindProperty("value_f");
                    EditorGUILayout.PropertyField(property, new GUIContent(name));

                }
                else if (type == typeof(Vector2))
                {
                    property = serializedObject.FindProperty("value_v3");
                    EditorGUILayout.PropertyField(property, new GUIContent(name));

                }
                else if (type == typeof(System.Action))
                {
                    //property = serializedObject.FindProperty("actionList");

                    //property = property.GetArrayElementAtIndex(0);
                    ActionsMenu(pwrUp);
                }
            }

            EditorGUI.indentLevel = 0;  //Reset indent for next function

            //GUILayout.EndArea();
            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            pwrUp.OnChange();
        }
        serializedObject.ApplyModifiedProperties();
    }
}