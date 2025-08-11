using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
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
    [SerializeField] private float deceleration;
    [SerializeField] private float messageDuration;
    [SerializeField] private float extendedDuration;
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
    private bool skipToMaxStartingScale = false;
    private bool stickToMinStartingScale = false;
    private float durationTime;





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

        if (timer <= durationTime - fadeOutDelay)
        {
            SetOpacity(timer, durationTime - fadeOutDelay);
            transform.localScale += new Vector3(fadeOutScaleGrowthRate * Time.fixedDeltaTime, fadeOutScaleGrowthRate * Time.fixedDeltaTime, 0f);
        }
        else
        {
            float newScale = Mathf.Lerp((skipToMaxStartingScale ? maximumStartingScale : minimumStartingScale), (stickToMinStartingScale ? minimumStartingScale : maximumStartingScale), Mathf.Clamp((totalDamage / maximumStartingScaleThreshold), 0f, 1f));
            transform.localScale = new Vector3(newScale, newScale, 1f);
        }

    }





    public void SetManager(DamageNumberManager manager)
    {
        this.manager = manager;
        if (this.owner != null && isDamage) manager.RemoveFromActiveDamageNumbers(this);
        if (this.owner != null && !isDamage) manager.RemoveFromActiveHealingNumbers(this);
        if (isDamage) manager.AddToActiveDamageNumbers(this);
        else manager.AddToActiveHealingNumbers(this);
    }

    public DamageNumberManager GetManager()
    {
        return this.manager;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
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

    private void UseDamageDealtSettings()
    {
        durationTime = messageDuration;
        skipToMaxStartingScale = false;
        stickToMinStartingScale = false;
        EnableThickTextOutline(false);
    }

    private void UseDamageTakenSettings()
    {
        durationTime = extendedDuration;
        skipToMaxStartingScale = true;
        stickToMinStartingScale = false;
        EnableThickTextOutline(true);
    }

    private void UseHealingReceivedSettings()
    {
        durationTime = extendedDuration;
        skipToMaxStartingScale = false;
        stickToMinStartingScale = false;
        EnableThickTextOutline(false);
    }

    private void UseMessageSetttings()
    {
        durationTime = messageDuration;
        skipToMaxStartingScale = false;
        stickToMinStartingScale = true;
        EnableThickTextOutline(false);
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

    public void InitializeMessage(DamageNumberManager manager, GameObject owner, float value, Sprite icon, string message, Color color)
    {
        SetManager(manager);
        SetOwner(owner);
        nextColor = color;
        PlayMessage(value, icon, message, color);
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
            UseDamageTakenSettings();
        }
        else
        {
            SetColorByGhostAction(context);
            UseDamageDealtSettings();
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
        UseHealingReceivedSettings();
        PlayMessage(context.healing, null, null, "+", null);
    }
    





    private void PlayMessage(float value)
    {
        PlayMessage(value, null, null, null, null);
    }
    
    private void PlayMessage(float value, Sprite icon)
    {
        PlayMessage(value, icon, null, null, null);
    }

    private void PlayMessage(float value, Sprite icon, string message)
    {
        PlayMessage(value, icon, message, null, null);
    }

    public void PlayMessage(float value, Sprite icon, string message, Color color)
    {
        nextColor = color;
        UseMessageSetttings();
        PlayMessage(value, icon, message);
    }

    private void PlayMessage(float value, Sprite icon, string message, string prependMessage, string appendMessage)
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
        Vector2 direction = (new Vector2(Random.Range(-0.25f, 0.25f), 0.75f)).normalized;
        transform.position = owner.transform.position + ((Vector3) (direction) * Random.Range(0.75f, 1.25f));
        rb.velocity = initialSpeed * direction;
        float newScale = Mathf.Lerp((skipToMaxStartingScale ? maximumStartingScale : minimumStartingScale), (stickToMinStartingScale ? minimumStartingScale : maximumStartingScale), Mathf.Clamp((totalDamage / maximumStartingScaleThreshold), 0f, 1f));
        transform.localScale = new Vector3(newScale, newScale, 1f);
        timer = durationTime;
    }

    public void PlayMessage(float value, Sprite icon, string message, string prependMessage, string appendMessage, Color color)
    {
        nextColor = color;
        UseMessageSetttings();
        PlayMessage(value, icon, message, prependMessage, appendMessage);
    }





    private void DestroyMessage()
    {
        if (isDamage) manager.RemoveFromActiveDamageNumbers(this);
        else manager.RemoveFromActiveHealingNumbers(this);
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
