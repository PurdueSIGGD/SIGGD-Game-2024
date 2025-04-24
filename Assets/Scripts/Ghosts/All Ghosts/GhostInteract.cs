using UnityEngine;

public class GhostInteract : InRangeInteract
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField]
    private ConvoSO hubConvo;

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

        InteractOption opt1 = new InteractOption("Talk", StartDialogue);
        InteractOption opt2 = new InteractOption("Add to Party", AddGhostToParty);
        InteractOption opt3 = new InteractOption("View Skill Tree", ViewSkillTree);

        InteractOption[] options = { opt1, opt2, opt3 };
        return options;
    }

    private void AddGhostToParty()
    {
        CloseMenu();
        PartyManager partyManger = PlayerID.instance.GetComponent<PartyManager>();
        partyManger.TryAddGhostToParty(this.GetComponent<GhostIdentity>());
        this.enabled = false;
    }

    private void StartDialogue()
    {
        CloseMenu();
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(hubConvo);
    }

    private void ViewSkillTree()
    {
        CloseMenu();
        SkillTreeUI skillTreeUI = FindFirstObjectByType<SkillTreeUI>(FindObjectsInactive.Include);
        skillTreeUI.OpenSkillTree(this.gameObject);
    }

    // ==============================
    //        Other Functions
    // ==============================

}