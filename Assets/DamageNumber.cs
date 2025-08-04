using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private bool isDamage;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Image messageIcon;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color damageDealtColor;
    [SerializeField] private Color damageTakenColor;
    [SerializeField] private Color healingReceivedColor;
    [SerializeField] private float thickTextOutlineWidth;

    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float damageTakenInitialSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float messageDuration;
    [SerializeField] private float damageTakenAddedDuration;
    [SerializeField] private float fadeOutDelay;
    [SerializeField] private float minimumStartingScale;
    [SerializeField] private float maximumStartingScale;
    [SerializeField] private float maximumStartingScaleThreshold;
    [SerializeField] private float fadeOutScaleGrowthRate;

    [SerializeField] private CharacterSO orionSO;
    [SerializeField] private CharacterSO northSO;
    [SerializeField] private CharacterSO evaSO;
    [SerializeField] private CharacterSO akihitoSO;
    [SerializeField] private CharacterSO yumeSO;
    [SerializeField] private CharacterSO silasSO;
    [SerializeField] private CharacterSO kingAegisSO;

    private DamageNumberManager manager;
    private GameObject owner;
    private float totalDamage = 0f;
    private float timer = 0f;
    private float baseTextOutlineWidth;
    private Color nextColor;





    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += DamageNumberEvent;
        GameplayEventHolder.OnHealingDealt += HealingNumberEvent;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= DamageNumberEvent;
        GameplayEventHolder.OnHealingDealt -= HealingNumberEvent;
    }



    // Start is called before the first frame update
    void Start()
    {
        baseTextOutlineWidth = messageText.outlineWidth;
        transform.localScale = new Vector3(minimumStartingScale, minimumStartingScale, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        rb.velocity -= (deceleration * Time.fixedDeltaTime * rb.velocity.normalized);

        if (timer > 0f)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f) DestroyMessage();
        }

        if (timer <= messageDuration - fadeOutDelay)
        {
            SetOpacity(timer, messageDuration - fadeOutDelay);
            transform.localScale += new Vector3(fadeOutScaleGrowthRate * Time.fixedDeltaTime, fadeOutScaleGrowthRate * Time.fixedDeltaTime, 0f);
        }
        else
        {
            float newScale = Mathf.Lerp(minimumStartingScale, maximumStartingScale, Mathf.Clamp((totalDamage / maximumStartingScaleThreshold), 0f, 1f));
            transform.localScale = new Vector3(newScale, newScale, 1f);
        }

    }





    public void SetManager(DamageNumberManager manager)
    {
        this.manager = manager;
    }

    public DamageNumberManager GetManager()
    {
        return this.manager;
    }

    public void SetOwner(GameObject owner)
    {
        if (this.owner != null && isDamage) manager.RemoveFromActiveDamageOwners(this.owner);
        if (this.owner != null && !isDamage) manager.RemoveFromActiveHealingOwners(this.owner);
        this.owner = owner;
        if (isDamage) manager.AddToActiveDamageOwners(this.owner);
        else manager.AddToActiveHealingOwners(this.owner);
        if (this.owner == PlayerID.instance.gameObject)
        {
            initialSpeed = damageTakenInitialSpeed;
            messageDuration += damageTakenAddedDuration;
            fadeOutDelay += damageTakenAddedDuration;
            if (isDamage) minimumStartingScale = maximumStartingScale;
        }
    }

    public GameObject GetOwner()
    {
        return this.owner;
    }

    public void SetColor(Color color)
    {
        messageIcon.color = color;
        messageText.color = color;
    }

    public void SetOpacity(float currentValue, float maxValue)
    {
        float percentOpacity = currentValue / maxValue;
        Color color = messageIcon.color;
        color.a = percentOpacity;
        messageIcon.color = color;
        messageText.color = color;
    }

    public void EnableThickTextOutline(bool enabled)
    {
        messageText.outlineWidth = enabled ? thickTextOutlineWidth : baseTextOutlineWidth;
        messageText.fontStyle = enabled ? FontStyles.Bold : FontStyles.Normal;
    }





    public void InitializeMessage(DamageNumberManager manager, GameObject owner, DamageContext context)
    {
        
        if (!isDamage) return;
        SetManager(manager);
        SetOwner(owner);
        HandleDamageEvent(context);
    }

    public void InitializeMessage(DamageNumberManager manager, GameObject owner, HealingContext context)
    {
        
        if (isDamage) return;
        SetManager(manager);
        SetOwner(owner);
        HandleHealingEvent(context);
    }

    public void InitializeMessage(DamageNumberManager manager, GameObject owner, float value, Sprite icon, string message)
    {
        
        SetManager(manager);
        SetOwner(owner);
        PlayMessage(value, icon, message);
    }





    public void DamageNumberEvent(DamageContext context)
    {
        if (!isDamage) return;
        HandleDamageEvent(context);
    }

    public void HandleDamageEvent(DamageContext context)
    {
        if (context.victim != owner) return;
        if (context.victim != PlayerID.instance.gameObject && context.attacker != PlayerID.instance.gameObject) return;
        if (context.victim == PlayerID.instance.gameObject)
        {
            nextColor = damageTakenColor;
            EnableThickTextOutline(true);
        }
        else
        {
            //SetColor(damageDealtColor);
            SetColorByGhostAction(context);
            EnableThickTextOutline(false);
        }
        PlayMessage(context.damage);
    }

    
    public void HealingNumberEvent(HealingContext context)
    {
        if (isDamage) return;
        HandleHealingEvent(context);
    }

    public void HandleHealingEvent(HealingContext context)
    {
        if (context.healee != owner) return;
        nextColor = healingReceivedColor;
        PlayMessage(context.healing, null, null, "+", null);
    }
    





    public void PlayMessage(float value)
    {
        PlayMessage(value, null, null, null, null);
    }
    
    public void PlayMessage(float value, Sprite icon)
    {
        PlayMessage(value, icon, null, null, null);
    }

    public void PlayMessage(float value, Sprite icon, string message)
    {
        PlayMessage(value, icon, message, null, null);
    }

    public void PlayMessage(float value, Sprite icon, string message, string prependMessage, string appendMessage)
    {
        if (value <= 0 && icon == null && message == null) return;

        if (value > 0f)
        {
            if (Mathf.FloorToInt(totalDamage) == Mathf.FloorToInt(totalDamage + value))
            {
                totalDamage += value;
                return;
            }
            totalDamage += value;
        }

        if (icon == null) messageIcon.enabled = false;
        else messageIcon.sprite = icon;
        messageText.text = (message == null) ? Mathf.FloorToInt(totalDamage).ToString() : message;
        if (prependMessage != null) messageText.text = prependMessage + messageText.text;
        if (appendMessage != null) messageText.text = messageText.text + appendMessage;

        SetOpacity(1f, 1f);
        SetColor(nextColor);
        transform.position = owner.transform.position + positionOffset;
        rb.velocity = initialSpeed * (new Vector2(Random.Range(-1f, 1f), 0.75f)).normalized;
        float newScale = Mathf.Lerp(minimumStartingScale, maximumStartingScale, Mathf.Clamp((totalDamage / maximumStartingScaleThreshold), 0f, 1f));
        transform.localScale = new Vector3(newScale, newScale, 1f);
        timer = messageDuration;
    }





    private void DestroyMessage()
    {
        if (isDamage) manager.RemoveFromActiveDamageOwners(owner);
        else manager.RemoveFromActiveHealingOwners(owner);
        Destroy(gameObject);
    }



    private void SetColorByGhostAction(DamageContext context)
    {
        Color color;
        switch (context.ghostID)
        {
            case GhostID.NORTH:
                color = northSO.highlightColor;
                break;
            case GhostID.EVA:
                color = evaSO.highlightColor;
                break;
            case GhostID.AKIHITO:
                color = akihitoSO.highlightColor;
                break;
            case GhostID.YUME:
                color = yumeSO.highlightColor;
                break;
            case GhostID.SILAS:
                color = silasSO.highlightColor;
                break;
            case GhostID.KING_AEGIS:
                color = kingAegisSO.highlightColor;
                break;
            default:
                color = orionSO.highlightColor;
                break;
        }
        nextColor = color;
    }
}
