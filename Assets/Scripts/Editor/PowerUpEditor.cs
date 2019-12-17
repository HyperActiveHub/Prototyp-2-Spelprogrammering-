using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PowerUp))]
public class PowerUpEditor : Editor
{
    int selected = 0;
    int actionSelected = 0;
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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PowerUp pwrUp = (PowerUp)target;
        FunctionsMenu(pwrUp);
        

        //temp, this should only be done once
        List<System.Reflection.ParameterInfo[]> parameters = new List<System.Reflection.ParameterInfo[]>(pwrUp.parameters);
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

        if (pwrUp.parameters[selected].Length != 0)     //the function has atleast one parameter
        {
            //GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(400, 400)));
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //get parameter type
            type = pwrUp.parameters[selected][0].ParameterType;
            name = pwrUp.parameters[selected][0].Name;

            //clear typeLists in PowerUp
            //if intList.Count != amount of int parameters, intList.add

            foreach (var p in pwrUp.parameters[selected])
            {
                SerializedProperty property = null;
                type = p.ParameterType;
                name = p.Name;


                if (type == typeof(int))
                {
                    property = serializedObject.FindProperty("intList");

                    property = property.GetArrayElementAtIndex(0);      //need to figure out a way to index the parameters, in each type(List).
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