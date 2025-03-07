using UnityEngine;

public class PedestalTest : MonoBehaviour
{
    [SerializeField] PartyPedestal pedestal;

    void OnInteract()
    {
        pedestal.Interact();
    }
}
