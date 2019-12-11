using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Power-Up", menuName = "PowerUp")]
public class PowerUp : ScriptableObject
{
#if UNITY_EDITOR
    //public UnityAction onValidate = delegate { };
    //[HideInInspector]
    public List<PowerUpBehaviour> pwrUps = new List<PowerUpBehaviour>();
    //maybe move list to gameManager to make sure its correct (sometimes fucks up, as in theres more pwrups in the list than the inspector)

    private void OnValidate()
    {
        foreach(PowerUpBehaviour pwrUp in pwrUps)
        {
            pwrUp.valuesChanged = true;
//            Debug.Log("Changed from pwrUp '" + name + "'.", this);
        }
        Debug.Log("Dependencies: " + pwrUps.Count);
        //onValidate();
    }
#endif

    

    //select all *tag*

    public Sprite sprite;
    [Tooltip("How long will this power-up last (in seconds)? Zero = infinity (must break somehow)")]    //Use assert to make sure the pwr-up can break if duration = 0;
    public float duration;

    //temp
    public float extraJumpHeight;

}
