using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerUpBehaviour : MonoBehaviour
{
    public PowerUp pwrUp;

    SpriteRenderer sRenderer;

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        SetValues();
    }

    void SetValues()
    {
        sRenderer.sprite = pwrUp.sprite;
    }

#if UNITY_EDITOR
    [HideInInspector]
    public bool valuesChanged;
    PowerUp oldPwrUp = null;

    private void OnValidate()
    {
        //Debug.Log("Changed pwr-up type from " + oldPwrUp.name + " to " + pwrUp.name);
        oldPwrUp.pwrUps.Remove(this);
        pwrUp.pwrUps.Add(this);
        oldPwrUp = pwrUp;
        valuesChanged = true;
    }

    private void OnEnable()
    {
        SetValues();
        oldPwrUp = pwrUp;
        pwrUp.pwrUps.Add(this);
    }

    void ChangedPwrUp()
    {
        valuesChanged = true;
    }

    private void OnDisable()
    {
        pwrUp.pwrUps.Remove(this);
    }

    void UpdateValues()
    {
        SetValues();
        valuesChanged = false;
    }

    private void Update()
    {
        if (valuesChanged)
        {
            SetValues();
            valuesChanged = false;
        }
    }
#endif

}
