using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // private System.Collections.Generic.SynchronizedCollection<Vector3>() offsets;
    private ArrayList offsetss;  // stores Vector2s for each shake coroutine
    private ArrayList synchOff;  // the above, but synchronised for thread safety
    private Transform camTrans;  // the camera's local transform
    private float globalScalar;  // applied to all shakes as a final scalar, range [0, 1]
    private float minAmp = 0.01f;  // the amplitude at which a shake is canceled (note that shake amplitudes never reach zero on their own, thus minAmp > 0)
    private bool killMode;  // in the process of stopping all shakes

    /// <summary>
    /// Nested class used to store a Vector2 by reference more easily
    /// </summary>
    private class RefVec2 {
        public Vector2 vec;
        public RefVec2(Vector2 v) {
            vec = v;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        offsetss = new ArrayList();
        synchOff = ArrayList.Synchronized(offsetss);
        camTrans = this.gameObject.transform;
        globalScalar = 1f;
        killMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveSum = Vector2.zero;
        foreach (RefVec2 shk in synchOff) {
            moveSum += shk.vec;
        }
        camTrans.localPosition = moveSum * globalScalar;
    }

    /// <summary>
    /// Starts a 
    /// taper bound formula: f(x) = ae^(-b * (x-d))
    /// bounded oscillation formula: g(x) = f(x)sin(2*pi*p(x-d))
    /// constant oscillation formula: h(x) = a*sin(2*pi*p(x-d))
    /// function stays h(x) until x=d, then changes to g(x)
    /// </summary>
    /// <param name="amp">a: Maximum oscillation amplitude</param>
    /// <param name="taper">b: Taper decrease rate</param>
    /// <param name="delay">d: Time until taper starts</param>
    /// <param name="freq">p: Frequency of oscillation</param>
    /// <param name="directio">Vector2: Direction of oscillation</param>
    public void Shake(float amp, float taper, float delay, float freq, Vector2 directio) {
        StartCoroutine(ShakeCoroutine(amp, taper, delay, freq, directio));
    }

    /// <summary>
    /// Decreases and resets ALL currently applied camera shake coroutines
    /// </summary>
    /// <param name="dur">Amount of time to decrease to 0 oscillation</param>
    public void StopShake(float dur) {
        if (killMode == false) {
            killMode = true;
            StartCoroutine(KillShake(dur));
        }
    }

    /// <summary>
    /// Coroutine for a single shake called by Shake()
    /// Adds a Vector2 to synchOff and modifies it according to fuction parameters over time
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShakeCoroutine(float amp, float taper, float delay, float freq, Vector2 directio) {
        float startTime = Time.time;
        float switchTime = startTime + delay;
        float x = 0f;
        float wav = 0f;
        RefVec2 thisOffset = new RefVec2(Vector2.zero);
        Vector2 dir = directio.normalized;
        synchOff.Add(thisOffset);
        while (Time.time < switchTime) {
            yield return new WaitForEndOfFrame();
            x = Time.time - startTime;
            wav = Mathf.Sin(2 * Mathf.PI * freq * (x - delay));
            thisOffset.vec = dir * amp * wav;
        }
        float fx = amp * Mathf.Exp(-taper * (x - delay));
        while (fx > minAmp) {
            yield return new WaitForEndOfFrame();
            x = Time.time - startTime;
            fx = amp * Mathf.Exp(-taper * (x - delay));
            wav = Mathf.Sin(2 * Mathf.PI * freq * (x - delay));
            thisOffset.vec = dir * fx * wav;
        }
        synchOff.Remove(thisOffset);
        yield return null;
    }

    /// <summary>
    /// Called by StopShake()
    /// Reduces all shakes to 0 amplitude over time and then stops all coroutines prematurely
    /// Clears synchOff
    /// </summary>
    /// <returns></returns>
    private IEnumerator KillShake(float dur) {
        float startTime = Time.time;
        float endTime = startTime + dur;
        float decFactor = 1 / dur;
        while (Time.time < endTime) {
            float timeProp = ((Time.time - startTime) * decFactor) - 1;
            globalScalar = Mathf.Pow(timeProp, 2);
            yield return new WaitForEndOfFrame();
        }
        globalScalar = 0f;
        StopAllCoroutines();
        globalScalar = 1f;
        synchOff.Clear();
        killMode = false;
        yield return null;
    }
}
