using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// Special action: player sprite color changes to 0.3 alpha for 8 seconds
/// </summary>
public class InvisibleAction : MonoBehaviour, IParty, IPossession
{
    [SerializeField]
    private float invisDuration;

    GameObject player;
    SpriteRenderer playerSpriteRenderer;

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
    }
    public void Select(GameObject player)
    {
        possessing = true;
    }
    public void DeSelect(GameObject player)
    {
        possessing = false;
        if (_resetInvisibleCoroutine != null)
        {
            playerSpriteRenderer.color = new Color(1, 1, 1, 1);
            _isInvisible = true;
        }
    }

    public void OnSpecial()
    {
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
    }

    private IEnumerator ResetInvisible()
    {
        yield return new WaitForSeconds(invisDuration);
        playerSpriteRenderer.color = new Color(1, 1, 1, 1);
        _isInvisible = false;
        _resetInvisibleCoroutine = null;
    }
}