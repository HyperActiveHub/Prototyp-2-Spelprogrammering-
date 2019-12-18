using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(PowerUp))]
public class PowerUpEditor : Editor
{
    int selected = 0;
    int actionSelected = 0;

    void FunctionsMenu(PowerUp pwrUp)
    {
        List<string> optionList = new List<string>();
        var functionList = pwrUp.functions;
        foreach (var function in functionList)
        {
            optionList.Add(function.Name);
        }

        selected = EditorGUILayout.Popup("Power-Up Function", selected, optionList.ToArray());
        if (!Application.isPlaying)
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

    private void OnEnable()
    {
        //AddProperties((PowerUp)target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        PowerUp pwrUp = (PowerUp)target;
        //Undo.RecordObject(target, "PowerUp change");
        selected = pwrUp.selected;
        FunctionsMenu(pwrUp);
        pwrUp.AddProperties(pwrUp);


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
                int propertyIndex = pwrUp.GetPropertyIndex(type, pwrUp.GetUniqueElements(pwrUp.types), true);

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
        serializedObject.ApplyModifiedProperties(); ///asdasdasd
    }
}