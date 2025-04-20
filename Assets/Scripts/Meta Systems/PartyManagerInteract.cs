using UnityEngine;

public class PartyManagerInteract : InRangeInteract
{
    // ==============================
    //       Serialized Fields
    // ==============================


    // ==============================
    //        Other Variables
    // ==============================


    // ==============================
    //        Unity Functions
    // ==============================


    // ==============================
    //       Private Functions
    // ==============================


    protected override InteractOption[] GetMenuOptions()
    {
        InteractOption opt1 = new InteractOption("Edit Party", OpenPartyUI);
        InteractOption[] options = { opt1 };

        return options;
    }

    private void OpenPartyUI()
    {
        PartyManagerUI partyUI = FindAnyObjectByType<PartyManagerUI>(FindObjectsInactive.Include);
        partyUI.OpenPartyMenu();
    }

    // ==============================
    //        Other Functions
    // ==============================
}
