using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CharacterSO orionCharacterInfo;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    [Header("Ghost 1")]
    [SerializeField] private Image ghost1Icon;
    [SerializeField] private List<Image> ghost1Frames;
    [SerializeField] private TextMeshProUGUI ghost1SpecialTimer;
    [SerializeField] private Slider ghost1SpecialSlider;
    [SerializeField] private Image ghost1SpecialBackground;
    [SerializeField] private Image ghost1SpecialFrame;
    [SerializeField] private Image ghost1SpecialIcon;

    [Header("Ghost 2")]
    [SerializeField] private Image ghost2Icon;
    [SerializeField] private List<Image> ghost2Frames;

    [Header("Selected Ghost")]
    [SerializeField] private Image selectedGhostIcon;
    [SerializeField] private List<Image> selectedGhostFrames;
    [SerializeField] private TextMeshProUGUI selectedGhostSpecialTimer;
    [SerializeField] private Slider selectedGhostSpecialSlider;
    [SerializeField] private Image selectedGhostSpecialBackground;
    [SerializeField] private Image selectedGhostSpecialFrame;
    [SerializeField] private Image selectedGhostSpecialIcon;

    private Health health;
    private StatManager stats;
    private PartyManager partyManager;



    // Start is called before the first frame update
    void Start()
    {
        health = PlayerID.instance.GetComponent<Health>();
        stats = PlayerID.instance.GetComponent<StatManager>();
        partyManager = PlayerID.instance.GetComponent<PartyManager>();

        healthSlider.maxValue = stats.ComputeValue("Max Health");
        healthSlider.value = healthSlider.maxValue;
    }



    // Update is called once per frame
    void Update()
    {
        updateHealthWidget();
        updateGhost1Widget();
        updateGhost2Widget();
        updateSelectedGhostWidget();
        updateSelectedGhostSpecialWidget();
        updateGhost1SpecialWidget();
    }



    private void updateHealthWidget()
    {
        healthText.text = Mathf.CeilToInt(health.currentHealth) + " | " + Mathf.CeilToInt(stats.ComputeValue("Max Health"));
        healthSlider.value = health.currentHealth;
    }



    private void updateGhost1Widget()
    {
        List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();
        if (ghosts.Count < 1 || ghosts[0] == null)
        {
            ghost1Icon.enabled = false;
            foreach (Image frame in ghost1Frames)
            {
                frame.enabled = false;
            }
            return;
        }
        ghost1Icon.enabled = true;
        foreach (Image frame in ghost1Frames)
        {
            frame.enabled = true;
        }
        ghost1Icon.sprite = (ghosts[0].IsSelected()) ? orionCharacterInfo.characterIcon : ghosts[0].GetCharacterInfo().characterIcon;
        foreach (Image frame in ghost1Frames)
        {
            Color color = (ghosts[0].IsSelected()) ? orionCharacterInfo.primaryColor : ghosts[0].GetCharacterInfo().primaryColor;
            color.a = frame.color.a;
            frame.color = color;
        }
    }



    private void updateGhost2Widget()
    {
        List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();
        if (ghosts.Count < 2 || ghosts[1] == null)
        {
            ghost2Icon.enabled = false;
            foreach (Image frame in ghost2Frames)
            {
                frame.enabled = false;
            }
            return;
        }
        ghost2Icon.enabled = true;
        foreach (Image frame in ghost2Frames)
        {
            frame.enabled = true;
        }
        ghost2Icon.sprite = (ghosts[1].IsSelected()) ? orionCharacterInfo.characterIcon : ghosts[1].GetCharacterInfo().characterIcon;
        foreach (Image frame in ghost2Frames)
        {
            Color color = (ghosts[1].IsSelected()) ? orionCharacterInfo.primaryColor : ghosts[1].GetCharacterInfo().primaryColor;
            color.a = frame.color.a;
            frame.color = color;
        }
    }



    private void updateSelectedGhostWidget()
    {
        //List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();
        selectedGhostIcon.sprite = orionCharacterInfo.characterIcon;
        foreach (Image frame in selectedGhostFrames)
        {
            Color color = orionCharacterInfo.primaryColor;
            color.a = frame.color.a;
            frame.color = color;
        }
        if (partyManager.GetSelectedGhost() != null)
        {
            selectedGhostIcon.sprite = partyManager.GetSelectedGhost().GetCharacterInfo().characterIcon;
            foreach (Image frame in selectedGhostFrames)
            {
                Color color = partyManager.GetSelectedGhost().GetCharacterInfo().primaryColor;
                color.a = frame.color.a;
                frame.color = color;
            }
        }
    }

    private void updateSelectedGhostSpecialWidget()
    {
        //List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();
        selectedGhostSpecialFrame.color = orionCharacterInfo.primaryColor;
        selectedGhostSpecialIcon.sprite = orionCharacterInfo.specialAbilityIcon;
        if (PlayerID.instance.GetComponent<OrionManager>().getSpecialCooldown() <= 0f)
        {
            selectedGhostSpecialTimer.text = "";
            selectedGhostSpecialSlider.value = 0f;
            selectedGhostSpecialBackground.color = new Color(0.863f, 1f, 1f);
            Color color = selectedGhostSpecialIcon.color;
            color.a = 1f;
            selectedGhostSpecialIcon.color = color;
        }
        else
        {
            selectedGhostSpecialTimer.text = Mathf.CeilToInt(PlayerID.instance.GetComponent<OrionManager>().getSpecialCooldown()).ToString();
            selectedGhostSpecialSlider.maxValue = PlayerID.instance.GetComponent<StatManager>().ComputeValue("Dash Cooldown");
            selectedGhostSpecialSlider.value = selectedGhostSpecialSlider.maxValue - PlayerID.instance.GetComponent<OrionManager>().getSpecialCooldown();
            selectedGhostSpecialBackground.color = new Color(0.5f, 0.5f, 0.5f);
            Color color = selectedGhostSpecialIcon.color;
            color.a = 0.5f;
            selectedGhostSpecialIcon.color = color;
        }

        if (partyManager.GetSelectedGhost() != null)
        {
            selectedGhostSpecialFrame.color = partyManager.GetSelectedGhost().GetCharacterInfo().primaryColor;
            selectedGhostSpecialIcon.sprite = partyManager.GetSelectedGhost().GetCharacterInfo().specialAbilityIcon;
            if (partyManager.GetSelectedGhost().GetComponent<GhostManager>().getSpecialCooldown() <= 0f)
            {
                selectedGhostSpecialTimer.text = "";
                selectedGhostSpecialSlider.value = 0f;
                selectedGhostSpecialFrame.color = partyManager.GetSelectedGhost().GetCharacterInfo().primaryColor;
                selectedGhostSpecialBackground.color = new Color(0.863f, 1f, 1f);
                selectedGhostSpecialIcon.enabled = true;
                Color color = selectedGhostSpecialIcon.color;
                color.a = 1f;
                selectedGhostSpecialIcon.color = color;
            }
            else
            {
                selectedGhostSpecialTimer.text = Mathf.CeilToInt(partyManager.GetSelectedGhost().GetComponent<GhostManager>().getSpecialCooldown()).ToString();
                selectedGhostSpecialSlider.maxValue = partyManager.GetSelectedGhost().GetComponent<StatManager>().ComputeValue("Special Cooldown");
                selectedGhostSpecialSlider.value = selectedGhostSpecialSlider.maxValue - partyManager.GetSelectedGhost().GetComponent<GhostManager>().getSpecialCooldown();
                selectedGhostSpecialBackground.color = new Color(0.5f, 0.5f, 0.5f);
                Color color = selectedGhostSpecialIcon.color;
                color.a = 0.5f;
                selectedGhostSpecialIcon.color = color;
            }
        }
    }

    private void updateGhost1SpecialWidget()
    {
        List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();

        if (ghosts.Count < 1 || ghosts[0] == null)
        {
            ghost1SpecialBackground.enabled = false;
            ghost1SpecialIcon.enabled = false;
            ghost1SpecialTimer.enabled = false;
            ghost1SpecialSlider.value = 0f;
            ghost1SpecialSlider.enabled = false;
            ghost1SpecialFrame.enabled = false;
            return;
        }
        ghost1SpecialBackground.enabled = true;
        ghost1SpecialIcon.enabled = true;
        ghost1SpecialTimer.enabled = true;
        ghost1SpecialSlider.enabled = true;
        ghost1SpecialFrame.enabled = true;

        ghost1SpecialFrame.color = orionCharacterInfo.primaryColor;
        ghost1SpecialIcon.sprite = orionCharacterInfo.specialAbilityIcon;
        if (PlayerID.instance.GetComponent<OrionManager>().getSpecialCooldown() <= 0f)
        {
            ghost1SpecialTimer.text = "";
            ghost1SpecialSlider.value = 0f;
            ghost1SpecialBackground.color = new Color(0.863f, 1f, 1f);
            Color color = ghost1SpecialIcon.color;
            color.a = 1f;
            ghost1SpecialIcon.color = color;
        }
        else
        {
            ghost1SpecialTimer.text = Mathf.CeilToInt(PlayerID.instance.GetComponent<OrionManager>().getSpecialCooldown()).ToString();
            ghost1SpecialSlider.maxValue = PlayerID.instance.GetComponent<StatManager>().ComputeValue("Dash Cooldown");
            ghost1SpecialSlider.value = ghost1SpecialSlider.maxValue - PlayerID.instance.GetComponent<OrionManager>().getSpecialCooldown();
            ghost1SpecialBackground.color = new Color(0.5f, 0.5f, 0.5f);
            Color color = ghost1SpecialIcon.color;
            color.a = 0.5f;
            ghost1SpecialIcon.color = color;
        }

        if (!ghosts[0].IsSelected())
        {
            ghost1SpecialFrame.color = ghosts[0].GetCharacterInfo().primaryColor;
            ghost1SpecialIcon.sprite = ghosts[0].GetCharacterInfo().specialAbilityIcon;
            if (ghosts[0].GetComponent<GhostManager>().getSpecialCooldown() <= 0f)
            {
                ghost1SpecialTimer.text = "";
                ghost1SpecialSlider.value = 0f;
                ghost1SpecialFrame.color = ghosts[0].GetCharacterInfo().primaryColor;
                ghost1SpecialBackground.color = new Color(0.863f, 1f, 1f);
                Color color = ghost1SpecialIcon.color;
                color.a = 1f;
                ghost1SpecialIcon.color = color;
            }
            else
            {
                ghost1SpecialTimer.text = Mathf.CeilToInt(ghosts[0].GetComponent<GhostManager>().getSpecialCooldown()).ToString();
                ghost1SpecialSlider.maxValue = ghosts[0].GetComponent<StatManager>().ComputeValue("Special Cooldown");
                ghost1SpecialSlider.value = ghost1SpecialSlider.maxValue - ghosts[0].GetComponent<GhostManager>().getSpecialCooldown();
                ghost1SpecialBackground.color = new Color(0.5f, 0.5f, 0.5f);
                Color color = ghost1SpecialIcon.color;
                color.a = 0.5f;
                ghost1SpecialIcon.color = color;
            }
        }
    }
}
