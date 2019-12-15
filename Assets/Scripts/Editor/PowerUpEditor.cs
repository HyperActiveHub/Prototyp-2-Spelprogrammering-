using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PowerUp))]
public class PowerUpEditor : Editor
{
    int selected = 0;

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
        var functionList = pwrUp.puFunctinos;
        foreach (var function in functionList)
        {
            optionList.Add(function.Name);
        }

        selected = EditorGUILayout.Popup("Power-Up Function", selected, optionList.ToArray());
        pwrUp.selected = selected;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PowerUp pwrUp = (PowerUp)target;
        FunctionsMenu(pwrUp);

        //temp, this should only be done once
        List<System.Reflection.ParameterInfo[]> parameters = new List<System.Reflection.ParameterInfo[]>(pwrUp.properties);
        int intCount = 0, floatCount = 0, v3Count = 0;

        List<System.Type> types = new List<System.Type>();
        Dictionary<int, bool> isParameterIndexUsed = new Dictionary<int, bool>();


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

        foreach (var pType in types)
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

        //string typeString = "";
        //foreach(var t in types)
        //{
        //    typeString += t.ToString() + ", ";
        //}
        //Debug.Log("Parameter types: " + typeString);

        //temp, this should only be done once

        System.Type type;
        string name = "";

        if (pwrUp.properties[selected].Length != 0)     //the function has atleast one parameter
        {
            //get parameter type
            type = pwrUp.properties[selected][0].ParameterType;
            name = pwrUp.properties[selected][0].Name;

            //clear typeLists in PowerUp
            //if intList.Count != amount of int parameters, intList.add

            foreach (var p in pwrUp.properties[selected])
            {
                SerializedProperty property = null;
                type = p.ParameterType;
                name = p.Name;


                if (type == typeof(int))
                {
                    property = serializedObject.FindProperty("intList");
                    
                    property = property.GetArrayElementAtIndex(0);      //need to figure out a way to index the parameters, in each type(List).
                }
                else if (type == typeof(float))
                {
                    property = serializedObject.FindProperty("value_f");
                }
                else if (type == typeof(Vector2))
                {
                    property = serializedObject.FindProperty("value_v3");
                }

                EditorGUILayout.PropertyField(property, new GUIContent(name));
            }
        }

        if (GUI.changed)
        {
            pwrUp.OnChange();
        }
        serializedObject.ApplyModifiedProperties();
    }
}