using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]
    public string treasureName;

    [SerializeField]
    public int trust;

    [SerializeField]
    public string ghostName;

    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && renderer.isVisible)
        {
            Interact(GameObject.Find("Player"));
            Destroy(gameObject);
        }

    }

    public void Interact(GameObject player)
    {
        PartyManager party = player.GetComponent<PartyManager>();
        foreach (GhostIdentity g in party.GetGhostPartyList())
        {
            if (string.Equals(g.GetCharacterInfo().name, ghostName))
            {
                //g.AddTrust(trust);
            }
        }
    }



}
