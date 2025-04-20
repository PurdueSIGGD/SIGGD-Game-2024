using System.Collections;
using UnityEngine;

public interface ILoopable : MonoBehaviour {

    // The coroutine which runs in the background to automatically loop the sound at the loop points
    protected abstract IEnumerator AutoLoop();
}