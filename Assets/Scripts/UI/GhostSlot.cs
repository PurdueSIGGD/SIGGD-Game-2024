using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image border;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private ReplaceGhostBehaviour replaceGhostBehaviour;
    [SerializeField] private int index;

    private GhostIdentity ghostIdentity;

    public void Visualize(GhostIdentity ghost, int ind)
    {
        ghostIdentity = ghost;
        CharacterSO info = ghost.GetCharacterInfo();
        image.sprite = info.characterIcon;
        border.color = info.primaryColor;
        name.text = info.displayName;
        index = ind;
    }

    public void Selected()
    {
        if (ghostIdentity != null)
        {
            //PartyManager.instance.RemoveAllGhost();
            //PartyManager.instance.TryAddGhostToParty(otherGhost);
            PartyManager.instance.SwitchGhostToIndex(index);
            PartyManager.instance.RemoveGhostFromParty(ghostIdentity);
            replaceGhostBehaviour.Chosen();
        }
    }
}
