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
    float duration;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        SetValues();
    }

    void SetValues()
    {
        sRenderer.sprite = pwrUp.sprite;
        duration = pwrUp.duration;
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
