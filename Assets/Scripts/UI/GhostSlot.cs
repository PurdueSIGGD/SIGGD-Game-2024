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
    [SerializeField] private bool secondGhost;

    private GhostIdentity ghostIdentity;
    private GhostIdentity otherGhost;

    public void Visualize(GhostIdentity ghost)
    {
        ghostIdentity = ghost;
        CharacterSO info = ghost.GetCharacterInfo();
        image.sprite = info.characterIcon;
        border.color = info.primaryColor;
        name.text = info.displayName;
    }

    public void OtherGhost(GhostIdentity ghost)
    {
        otherGhost = ghost;
    }

    public void Selected()
    {
        if (ghostIdentity != null && otherGhost != null)
        {
            PartyManager.instance.RemoveAllGhost();
            PartyManager.instance.TryAddGhostToParty(otherGhost);
            replaceGhostBehaviour.Chosen();
        }
    }
}
