using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CharacterSO orionCharacterInfo;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Image ghost1Icon;
    [SerializeField] private Image ghost1IconFrame;

    [SerializeField] private Image ghost2Icon;
    [SerializeField] private Image ghost2IconFrame;

    [SerializeField] private Image selectedGhostIcon;
    [SerializeField] private List<Image> selectedGhostFrames;

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
            ghost1IconFrame.enabled = false;
            return;
        }
        ghost1Icon.enabled = true;
        ghost1IconFrame.enabled = true;
        ghost1Icon.sprite = (ghosts[0].IsSelected()) ? orionCharacterInfo.icon : ghosts[0].GetCharacterInfo().icon;
        ghost1IconFrame.color = (ghosts[0].IsSelected()) ? orionCharacterInfo.primaryColor : ghosts[0].GetCharacterInfo().primaryColor;
    }



    private void updateGhost2Widget()
    {
        List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();
        if (ghosts.Count < 2 || ghosts[1] == null)
        {
            ghost2Icon.enabled = false;
            ghost2IconFrame.enabled = false;
            return;
        }
        ghost2Icon.enabled = true;
        ghost2IconFrame.enabled = true;
        ghost2Icon.sprite = (ghosts[1].IsSelected()) ? orionCharacterInfo.icon : ghosts[1].GetCharacterInfo().icon;
        ghost2IconFrame.color = (ghosts[1].IsSelected()) ? orionCharacterInfo.primaryColor : ghosts[1].GetCharacterInfo().primaryColor;
    }



    private void updateSelectedGhostWidget()
    {
        List<GhostIdentity> ghosts = partyManager.GetGhostMajorList();
        selectedGhostIcon.sprite = orionCharacterInfo.icon;
        foreach (Image frame in selectedGhostFrames)
        {
            Color color = orionCharacterInfo.primaryColor;
            color.a = frame.color.a;
            frame.color = color;
        }
        if (partyManager.GetSelectedGhost() != null)
        {
            selectedGhostIcon.sprite = partyManager.GetSelectedGhost().GetCharacterInfo().icon;
            foreach (Image frame in selectedGhostFrames)
            {
                Color color = partyManager.GetSelectedGhost().GetCharacterInfo().primaryColor;
                color.a = frame.color.a;
                frame.color = color;
            }
        }
        //healthText.text = (ghosts.Count >= 1 && ghosts[0] != null) ? Mathf.CeilToInt(ghosts[0].GetComponent<GhostManager>().getSpecialCooldown()).ToString() : "";
    }
}
