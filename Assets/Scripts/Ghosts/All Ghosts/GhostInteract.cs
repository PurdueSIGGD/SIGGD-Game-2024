using System.Linq.Expressions;
using UnityEngine;

public class GhostInteract : InRangeInteract, IParty
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField]
    private ConvoSO hubConvo;
    [SerializeField] private bool isFirstInteraction;
    [SerializeField] private bool isNPC;
    private GameObject interactionIndicator;
    private Vector2 origPos;

    void Awake()
    {
        origPos = transform.position;
    }


    public void SetConvo(ConvoSO convo, GameObject indicator = null)
    {
        if (indicator)
        {
            interactionIndicator = Instantiate(indicator, transform.position, transform.rotation);
        }

        hubConvo = convo;
    }


    // ==============================
    //       Private Functions
    // ==============================

    protected override InteractOption[] GetMenuOptions()
    {
        if (isFirstInteraction)
        {
            InteractOption opt = new InteractOption("Talk", FirstInteraction);
            return new InteractOption[] { opt };
        }

        InteractOption[] options;

        InteractOption opt1 = new InteractOption("Talk", StartDialogue);
        if (!isNPC)
        {
            InteractOption opt2 = new InteractOption("Add to Party", AddGhostToParty);
            InteractOption opt3 = new InteractOption("View Skill Tree", ViewSkillTree);
            options = new InteractOption[]{ opt1, opt2, opt3 };
        }
        else{
            options = new InteractOption[] { opt1 };
        }
        
        return options;
    }

    private void AddGhostToParty()
    {
        CloseMenu();
        PartyManager partyManger = PlayerID.instance.GetComponent<PartyManager>();
        partyManger.TryAddGhostToParty(this.GetComponent<GhostIdentity>());
    }

    private void FirstInteraction()
    {
        CloseMenu();
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(hubConvo);
        dialogueManager.OnNextCloseCall(AddGhostToParty);
    }

    private void StartDialogue()
    {
        CloseMenu();
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(hubConvo);

        if (interactionIndicator) Destroy(interactionIndicator);
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

    public void EnterParty(GameObject player)
    {
        enabled = false;
    }

    public void ExitParty(GameObject player)
    {
        enabled = true;
    }

    public void EnableIndiactor()
    {
        if (interactionIndicator) interactionIndicator.SetActive(true);
    }

    public void DisableIndicator()
    {
        if (interactionIndicator) interactionIndicator.SetActive(false);
    }

    public void ReturnGhostToOrigPos()
    {
        transform.position = origPos;
    }
}