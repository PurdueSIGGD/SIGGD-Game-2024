using UnityEngine;
using System.Collections;

/// <summary>
/// Special action: player sprite color changes to 0.3 alpha for 8 seconds
/// </summary>
public class InvisibleAction : GhostAction
{
    GameObject player;
    SpriteRenderer playerSpriteRenderer;

    private bool _isInvisible = false;
    private Coroutine _resetInvisibleCoroutine;
    public InvisibleAction()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
        }
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }
    public override void EnterSpecial()
    {
    }

    public override void ExitSpecial()
    {
        if (_resetInvisibleCoroutine != null)
        {
            
            playerSpriteRenderer.color = new Color(1, 1, 1, 1);
            _isInvisible = true;
        }
    }

    public override void OnSpecial(MonoBehaviour context)
    {
        if (!_isInvisible) {
            playerSpriteRenderer.color = new Color(1, 1, 1, 0.3f);
            _isInvisible = true;
            _resetInvisibleCoroutine = context.StartCoroutine(ResetInvisible());
        } else {
            playerSpriteRenderer.color = new Color(1, 1, 1, 1);
            _isInvisible = false;

            if (_resetInvisibleCoroutine != null)
            {
                context.StopCoroutine(_resetInvisibleCoroutine);
                _resetInvisibleCoroutine = null;
            }
        }
    }

    public override void UpdateSpecial()
    {
    }

    private IEnumerator ResetInvisible()
    {
        yield return new WaitForSeconds(8);
        playerSpriteRenderer.color = new Color(1, 1, 1, 1);
        _isInvisible = false;
        _resetInvisibleCoroutine = null;
    }
}