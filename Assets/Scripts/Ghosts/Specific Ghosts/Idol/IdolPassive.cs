using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Passive for the Idol Ghost, boosts player speed when enemy dies.
/// </summary>
public class IdolPassive : MonoBehaviour
{
    [SerializeField] public GameObject tempoParticlesVFX;
    [SerializeField] public int tempoStacks = 0; // number of stacks
    [SerializeField] private bool kill; // has the player just scored a kill?
    [SerializeField] private bool uptempo; // have the stored tempo stacks been activated?
    [SerializeField] public float duration;
    [SerializeField] private float tick;
    [SerializeField] public bool active = false;
    List<string> statNames = new()
        {
            "Max Running Speed",
            "Running Accel.",
            //"Running Deaccel.",
            "Max Glide Speed",
            "Glide Accel.",
            //"Glide Deaccel."
        };
    private string dodgeStatName = "Dodge Chance";

    // Reference to player stats
    private StatManager playerStats;
    [HideInInspector] public IdolManager manager;

    //private IdolTempoParticles particlesVFX;

    // list of avaliable audio banks to play on max tempo
    public List<string> avaliableHoloJumpVA = new List<string>() { "Eva-Idol Max Tempo" };
    // list of avaliable audio banks to play on loosing clone
    public List<string> avaliableCloneLostVA = new() { "Eva-Idol Holo Jump Lost Clone" };

    void OnEnable()
    {
        GameplayEventHolder.OnDeath += IdolOnKill;
        GameplayEventHolder.OnDamageDealt += IdolOnDamageDealt;
    }
    void OnDisable()
    {
        GameplayEventHolder.OnDeath -= IdolOnKill;
        GameplayEventHolder.OnDamageDealt -= IdolOnDamageDealt;
    }

    void Start()
    {
        playerStats = PlayerID.instance.gameObject.GetComponent<StatManager>(); // yoink
        //particlesVFX = Instantiate(tempoParticlesVFX, PlayerID.instance.gameObject.transform).GetComponent<IdolTempoParticles>();
        //particlesVFX.gameObject.SetActive(false);
        tempoStacks = 0;

        PlayerID.instance.gameObject.GetComponent<PlayerHealth>().evaTempo = this;
        
        /*
        tempoStacks = SaveManager.data.eva.tempoCount;
        if (tempoStacks > 0)
        {
            uptempo = true;
            duration = SaveManager.data.eva.remainingTempoDuration;
        }
        */
    }

    void Update()
    {
        // tempo duration timer
        if (uptempo && duration > 0)
        {
            float inactiveModifier = manager.GetStats().ComputeValue("TEMPO_INACTIVE_DURATION_MODIFIER");
            float maxDuration = manager.GetStats().ComputeValue("TEMPO_BASE_DURATION");

            // decrement duration at a modified rate if Idol is not active
            tick = active ? Time.deltaTime : Time.deltaTime * inactiveModifier;
            SaveManager.data.eva.remainingTempoDuration = duration -= tick;

            // reset duration if player scored a kill
            if (kill)
            {
                duration = maxDuration;
                kill = false;
            }
        }
        else if (uptempo)
        {
            // end tempo if duration ended
            KillTempo();
        }
    }

    /// <summary>
    /// Called by IdolManager on swap to Idol, AFTER manager.active is set true
    /// </summary>
    public void ApplyBuffOnSwap()
    {
        // just being cautious and double checking if idol is active
        // swapping is so sketch but we ball
        if (active)
        {
            return;
        }
        active = true;
        UpdateSpeed(tempoStacks);
        Debug.Log("TEMPO AWAH UP");

        // initialize tempo timer if stacks exist and tempo isn't up already
        if (!uptempo && (tempoStacks > 0))
        {
            InitializeTempoTimer();
        }

        // VFX
        if (tempoStacks <= 0) return;
        GameObject teleportPulseVfX = Instantiate(manager.tempoPulseVFX, PlayerID.instance.transform.position, Quaternion.identity);
        teleportPulseVfX.GetComponent<RingExplosionHandler>().playRingExplosion(1.5f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostEmpowered(manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, tempoStacks, manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"));
    }
    /// <summary>
    /// Called by IdolManager on swap away from Idol, BEFORE manager.active is set false
    /// </summary>
    public void RemoveBuffOnSwap()
    {
        if (!active)
        {
            return;
        }
        UpdateSpeed(-tempoStacks);
        active = false;
        Debug.Log("TEMPO AWAH DOWN");

        // VFX
        PlayerID.instance.GetComponent<PlayerParticles>().StopGhostEmpowered();
    }

    /// <summary>
    /// Increases the Idol buff count by one
    /// </summary>
    public void IdolOnKill(DamageContext context)
    {
        // not me? DON'T CARE!!!

        if (context.attacker != PlayerID.instance.gameObject)
        {
            return;
        }

        IncrementTempo(Mathf.CeilToInt(manager.GetStats().ComputeValue("TEMPO_STACKS_PER_KILL")));
    }

    public void IdolOnDamageDealt(DamageContext context)
    {
        if (context.attacker != PlayerID.instance.gameObject || context.damageTypes.Contains(DamageType.STATUS)) return;
        if (uptempo && duration > 0)
        {
            duration += manager.GetStats().ComputeValue("TEMPO_DAMAGE_DURATION_GAIN");
        }
    }

    /// <summary>
    /// Increases the Idol buff count by one
    /// </summary>
    public void IncrementTempo(int stacks)
    {
        Debug.Log("Increasing tempo");

        // Update flagged to reset duration
        kill = true;

        // ensure tempo stacks don't exceed maximum
        int remainingStacks = (int) manager.GetStats().ComputeValue("TEMPO_MAX_STACKS") - tempoStacks;
        stacks = stacks < remainingStacks ? stacks : remainingStacks;

        // SFX
        if (tempoStacks + stacks < manager.GetStats().ComputeValue("TEMPO_MAX_STACKS") || tempoStacks >= manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"))
        {
            AudioManager.Instance.SFXBranch.GetSFXTrack("Eva-Tempo Gained").SetPitch(tempoStacks, manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"));
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Eva-Tempo Gained");
        }
        else
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Eva-Tempo Max");
        }

        // Voice Lines
        if (tempoStacks + stacks < manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"))
        {
            if (active) AudioManager.Instance.VABranch.PlayVATrack("Eva-Idol Activate Tempo");
        }
        else if (tempoStacks < manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"))
        {
            // play audio, if has upgrade, choose from 1 random voice bank to play
            string chosenBank = avaliableHoloJumpVA[Random.Range(0, avaliableHoloJumpVA.Count)];
            if (active) AudioManager.Instance.VABranch.PlayVATrack(chosenBank);
        }

        // increment tempo stacks by stacks
        tempoStacks += stacks;
        SaveManager.data.eva.tempoCount = tempoStacks;

        // Feedback Loop reduce Special cooldown
        GetComponent<FeedbackLoop>().reduceCooldown(false);

        // immediately apply tempo changes if Idol is active
        if (active)
        {
            UpdateSpeed(stacks);
        }

        // initialize tempo effect if idol is active, has stacks, and isn't speed boosted yet 
        if (tempoStacks > 0 && !uptempo)
        {
            InitializeTempoTimer();
        }

        // VFX
        if (active && tempoStacks > 0)
        {
            GameObject teleportPulseVfX = Instantiate(manager.tempoPulseVFX, PlayerID.instance.transform.position, Quaternion.identity);
            teleportPulseVfX.GetComponent<RingExplosionHandler>().playRingExplosion(1.5f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
            PlayerID.instance.GetComponent<PlayerParticles>().PlayGhostEmpowered(manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, tempoStacks, manager.GetStats().ComputeValue("TEMPO_MAX_STACKS"));
        }

        // Ability UI Ping
        if (tempoStacks > 0) GetComponent<IdolUIDriver>().basicAbilityUIManager.pingAbility();
    }

    /// <summary>
    /// On your marks... get set... GOOOO!!!
    /// Initializes the tempo timer to accurately track tempo duration until it ends.
    /// </summary>
    /// <returns></returns>
    private string InitializeTempoTimer()
    {
        Debug.Log("GO GO GO GO GO");
        duration = manager.GetStats().ComputeValue("TEMPO_BASE_DURATION");
        uptempo = true;
        return "GRAHHHHHHHHHHHHH I'M FAST AF";
    }

    /// <summary>
    /// Remove all effects of tempo stacks and resets tempo stack counter.
    /// </summary>
    /// <returns></returns>
    private string KillTempo()
    {
        if (active)
        {
            UpdateSpeed(-tempoStacks);
        }
        tempoStacks = SaveManager.data.eva.tempoCount = 0;
        uptempo = false;

        // VFX
        if (active) PlayerID.instance.GetComponent<PlayerParticles>().StopGhostEmpowered();

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Eva-Tempo Lost");

        return "AAAOAOAOAO SH I HIT A BRICK WALL OH GOD IT HURTS";
    }

    /// <summary>
    /// Modify player speed stats by changes in player stack count
    /// </summary>
    /// <param name="deltaStack"></param>
    private void UpdateSpeed(int delta)
    {
        // apply changes to each speed stat

        int dodgeMod = Mathf.FloorToInt(manager.GetStats().ComputeValue("TEMPO_DODGE_PERCENT_INT") * 10f);
        playerStats.ModifyStat(dodgeStatName, dodgeMod * delta);
        int mod = (int) manager.GetStats().ComputeValue("TEMPO_BUFF_PERCENT_INT");
        foreach (string statName in statNames)
        {
            // flip scaling direction for deacceleration stats

            delta = statName.Contains("Deaccel") ? delta * -1 : delta;
            playerStats.ModifyStat(statName, mod * delta);
        }
        Debug.Log("RUN STAT: " + playerStats.ComputeValue("Max Running Speed"));
        Debug.Log("DODGE STAT: " + playerStats.ComputeValue("Dodge Chance"));
    }
}