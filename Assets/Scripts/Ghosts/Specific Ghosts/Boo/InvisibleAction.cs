using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// Special action: player sprite color changes to 0.3 alpha for 8 seconds
/// </summary>
public class InvisibleAction : MonoBehaviour, IParty, ISelectable
{
    PlayerStateMachine psm;

    [SerializeField]
    private float invisDuration;
    private float invisClickCD = 0f; // the timer before the invis special action can be performed again
    [SerializeField]
    private bool canClickInvis = true; // bool for invis click cd
    GameObject player;
    SpriteRenderer playerSpriteRenderer;

    [SerializeField]
    private bool _isInvisible = false;
    private Coroutine _resetInvisibleCoroutine;
    private bool inParty = false;
    private bool possessing = false;
    public InvisibleAction()
    {
        /*player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
        }
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();*/
    }
    public void EnterParty(GameObject player)
    {
        inParty = true;
        this.player = player;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public void ExitParty(GameObject player)
    {
        inParty = false;
        this.player = null;
        playerSpriteRenderer = null;
        _resetInvisibleCoroutine = null;
    }
    public void Select(GameObject player)
    {
        possessing = true;
        player.GetComponent<Dash>().specialAction = GoInvisible;
        psm = player.GetComponent<PlayerStateMachine>();

    }
    public void DeSelect(GameObject player)
    {
        possessing = false;
        if (_resetInvisibleCoroutine != null)
        {
            playerSpriteRenderer.color = new Color(1, 1, 1, 1);
            _isInvisible = true;
        }
        player.GetComponent<Dash>().specialAction = null;
    }
    public bool GetBool()
    {
        return _isInvisible;
    }
    public void StartSpecial()
    {
        GoInvisible();
    }
    public void EndSpecial()
    {
        return;
    }
    public void GoInvisible()
    {
        psm.EnableTrigger("OPT");

        if (!canClickInvis)
        {
            return;
        }
        else
        {
            StartCoroutine(InvisClickCDTimer());
        }

        if (inParty && possessing)
        {
            if (!_isInvisible)
            {
                playerSpriteRenderer.color = new Color(1, 1, 1, 0.3f);
                _isInvisible = true;
                _resetInvisibleCoroutine = StartCoroutine(ResetInvisible());
            }
            else
            {
                playerSpriteRenderer.color = new Color(1, 1, 1, 1);
                _isInvisible = false;

                if (_resetInvisibleCoroutine != null)
                {
                    StopCoroutine(_resetInvisibleCoroutine);
                    _resetInvisibleCoroutine = null;
                }
            }
        }
        print("Invisible Status: " + _isInvisible);
    }

    private IEnumerator ResetInvisible()
    {
        yield return new WaitForSeconds(invisDuration);
        playerSpriteRenderer.color = new Color(1, 1, 1, 1);
        _isInvisible = false;
        _resetInvisibleCoroutine = null;
    }

    private IEnumerator InvisClickCDTimer()
    {
        canClickInvis = false;
        yield return new WaitForSeconds(invisClickCD);
        canClickInvis = true;
    }
}
