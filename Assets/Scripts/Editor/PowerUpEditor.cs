using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PowerUp))]
public class PowerUpEditor : Editor
{
    int selected = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PowerUp pwrUp = (PowerUp)target;
        List<string> optionList = new List<string>();
        var functionList = pwrUp.puFunctinos;
        foreach (var function in functionList)
        {
            optionList.Add(function.Name);
        }

        selected = EditorGUILayout.Popup("Power-Up Function", selected, optionList.ToArray());
        pwrUp.selected = selected;

        //SerializedProperty property = serializedObject.FindProperty("Parameters");
        //EditorGUILayout.PropertyField(property);

        string name = "";
        System.Type type;
        //Serialize parameter types based on function instead?
        if (pwrUp.properties[selected].Length != 0)     //the function has atleast one parameter
        {
            //get parameter type
            type = pwrUp.properties[selected][0].ParameterType;
            name = pwrUp.properties[selected][0].Name;

            foreach (var p in pwrUp.properties[selected])
            {
                SerializedProperty property = null;
                type = p.ParameterType;
                name = p.Name;


                if (type == typeof(int))
                {
                    //Check if there is an available int element, if not add one.
                    //pwrUp.intList.Add(0);

                    property = serializedObject.FindProperty("intList");
                    //property = serializedObject.FindProperty("intHolder");
                    //var booleanProperty = property.FindPropertyRelative("isUsed").GetArrayElementAtIndex(selected);
                    //if (booleanProperty.boolValue == false)
                    //{
                    //    booleanProperty.boolValue = true;
                    //    property = property.FindPropertyRelative("value").GetArrayElementAtIndex(selected);
                    //}

                    //Debug.Log("isUsed: " + booleanProperty.boolValue);
                    property = property.GetArrayElementAtIndex(0);
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


        //var parameters = functionList[selected].GetParameters();
        //foreach (var para in parameters)
        //{
        //    Debug.Log(para);
        //    SerializedProperty property = serializedObject.FindProperty(para.Name);
        //    Debug.Log(property.ToString());
        //    EditorGUILayout.PropertyField(property);

        //}
        //SerializedProperty property = ("test");
        //EditorGUILayout.PropertyField(property);

        if (GUI.changed)
        {
            pwrUp.OnChange();
        }
        serializedObject.ApplyModifiedProperties();
    }



}


