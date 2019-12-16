using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance != null)
            {
                return _instance;
            }
            else
            {
                Debug.LogError("You need to add a GameManager.");
                return null;
            }
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("Too many GameManagers, there can only be one GameManager.", this);
        }
    }

#if UNITY_EDITOR
    static public List<PowerUpBehaviour> pwrUps = new List<PowerUpBehaviour>();
    public static void PwrUpChanged()
    {
        foreach (PowerUpBehaviour pwrUp in pwrUps)
        {
            pwrUp.valuesChanged = true;
        }
    }

    public static void ClearAndAddPwrUps()
    {
        pwrUps.Clear();

        foreach (PowerUpBehaviour puInstance in FindObjectsOfType<PowerUpBehaviour>())
        {
            pwrUps.Add(puInstance);
        }
    }
#endif

}
