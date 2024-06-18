using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerClickHandler
{
    public float percentage;
    public GameManager.UpChoice choice;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SetUpgrade(choice, percentage);
    }
}
