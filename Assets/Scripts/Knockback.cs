using System.Data;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Generic Knockback effect intended to be used when a collider applies a
/// knockback to a body to produce a knockback effect.
/// </summary>
public class Knockback
{
    /// <summary>
    /// Adds a knockback effect to the other body. Defaults object drag to
    /// 5 if drag isn't set.
    /// </summary>
    /// <param name="source">The source of the knockback in a collision</param>
    /// <param name="other">The object to be knocked away</param>
    /// <param name="force">The scale of the knockback effect</param>
    public void AddKnockback(Transform source, Transform other, float force)
    {
        if (other.GetComponent<Rigidbody>() == null)
        {
            return;
        }
        Rigidbody rb = other.GetComponent<Rigidbody>();
        Vector3 dir = (other.position - source.position).normalized;

        if (rb.drag == 0)
        {
            rb.drag = 5;
        }
        
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
