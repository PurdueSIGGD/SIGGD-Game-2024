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
        //PartyUI partyUI = FindFirstObjectByType<PartyUI>();
        //partyUI.OpenUI();
    }

    // ==============================
    //        Other Functions
    // ==============================
}
