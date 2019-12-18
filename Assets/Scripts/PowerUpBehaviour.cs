using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerUpBehaviour : MonoBehaviour
{
    public PowerUp pwrUp;

    SpriteRenderer sRenderer;
    //float duration;
    //List<System.Reflection.MethodInfo> functions;
    //List<System.Reflection.MethodInfo> chosenFunctions;
    System.Reflection.MethodInfo chosenFunction;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        SetValues();
    }

    void SetValues()
    {
        sRenderer.sprite = pwrUp.sprite;
        //set a list of all functions and actions and invoke them accordingly when powerUp is picked up.

        chosenFunction = pwrUp.functions[pwrUp.selected];

        //duration = pwrUp.duration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerScript>() != null)
        {
            //invoke all functions with correct parameters
            //
            if(pwrUp.parameters[pwrUp.selected].Length == 0)
            {
                chosenFunction.Invoke(null, null);

            }
            else
            {
                List<object> parameterTypes = new List<object>();
                foreach(var parameter in pwrUp.parameters[pwrUp.selected])
                {
                    var unique = pwrUp.GetUniqueElements(pwrUp.types);
                    int typeIndex = pwrUp.GetTypeIndex(parameter.ParameterType, unique);
                    int propertyIndex = pwrUp.GetPropertyIndex(parameter.ParameterType, unique);

                    parameterTypes.Add(pwrUp.intList[propertyIndex]);
                }


                chosenFunction.Invoke(null, parameterTypes.ToArray());
            }

            //pwrUp.GetPropertyIndex()
            //methodInfoOfFunction.Invoke(null, null); if function lacks parameters (i call them actions).
            //methodInfoOfFunction.Invoke(null, allCorrectParameters (the serialized ones...)); if function has one or more parameters.
        }
    }

#if UNITY_EDITOR
    [HideInInspector]
    public bool valuesChanged;

    private void OnEnable()
    {
        GameManager.ClearAndAddPwrUps();
    }

    private void OnDisable()
    {
        GameManager.ClearAndAddPwrUps();
    }

    private void OnValidate()
    {
        valuesChanged = true;
    }

    void UpdateValues()
    {
        SetValues();
        valuesChanged = false;
    }

    private void LateUpdate()
    {
        //This is done to avoid sending message during awake (warning)
        if (valuesChanged)
        {
            UpdateValues();
        }
    }

#endif

}
