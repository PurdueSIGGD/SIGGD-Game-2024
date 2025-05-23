using UnityEngine.Events;

public interface IScreenUI
{
    /// <summary>
    /// Calls given action next time screen UI related game object is closed
    /// </summary>
    public void OnNextCloseCall(UnityAction action);
}
