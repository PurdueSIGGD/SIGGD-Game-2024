using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General interface to handle Player selection of a specific object in a list.
/// Similar functionality to IParty
/// </summary>
public interface ISelectable
{
    public void Select(GameObject player);
    public void DeSelect(GameObject player);
}
