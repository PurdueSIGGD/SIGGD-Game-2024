using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [DoNotSerialize] public static VFXManager Instance;

    [SerializeField] private GameObject playerLightAttack1;
    [SerializeField] private GameObject playerLightAttack2;
    [SerializeField] private GameObject playerHeavyAttack;
    [SerializeField] private GameObject playerLightAttackUp;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayVFX(VFX vfxName, Vector3 position, Quaternion rotation)
    {
        Instantiate(getVFXPrefab(vfxName), position, rotation);
    }

    public void PlayVFX(VFX vfxName, Vector3 position, Quaternion rotation, Color color)
    {
        GameObject vfx = Instantiate(getVFXPrefab(vfxName), position, rotation);
        vfx.GetComponent<SpriteRenderer>().color = color;
    }



    public GameObject getVFXPrefab(VFX vfxName)
    {
        switch (vfxName)
        {
            case VFX.PLAYER_LIGHT_ATTACK_1:
                return playerLightAttack1;
            case VFX.PLAYER_LIGHT_ATTACK_2:
                return playerLightAttack2;
            case VFX.PLAYER_HEAVY_ATTACK:
                return playerHeavyAttack;
            case VFX.PLAYER_LIGHT_ATTACK_UP:
                return playerLightAttackUp;
            default: return null;
        }
    }
}

public enum VFX
{
    PLAYER_LIGHT_ATTACK_1,
    PLAYER_LIGHT_ATTACK_2,
    PLAYER_HEAVY_ATTACK,
    PLAYER_LIGHT_ATTACK_UP,
}