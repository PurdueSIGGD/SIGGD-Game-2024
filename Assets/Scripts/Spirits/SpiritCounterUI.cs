using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class SpiritCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private Image iconImage;

    [SerializeField] private Spirit.SpiritType spiritType;
    [SerializeField] private Color spiritColor;

    [SerializeField] private bool fromSaveManager = true; // from save manager or spirit tracker?

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        iconImage.color = spiritColor;
    }

    public void UpdateText()
    {
        if (fromSaveManager)
        {

            switch (spiritType) {
                case Spirit.SpiritType.Red:
                    counterText.text = SaveManager.data.spiritCounts[0].ToString();
                    break;
                case Spirit.SpiritType.Blue:
                    counterText.text = SaveManager.data.spiritCounts[1].ToString();
                    break;
                case Spirit.SpiritType.Yellow:
                    counterText.text = SaveManager.data.spiritCounts[2].ToString();
                    break;

            }

        } else
        {
            SpiritTracker sp = PersistentData.Instance.GetComponent<SpiritTracker>();
            switch (spiritType)
            {
                case Spirit.SpiritType.Red:
                    counterText.text = sp.redSpiritsCollected.ToString();
                    break;
                case Spirit.SpiritType.Blue:
                    counterText.text = sp.blueSpiritsCollected.ToString();
                    break;
                case Spirit.SpiritType.Yellow:
                    counterText.text = sp.yellowSpiritsCollected.ToString();
                    break;

            }
        }
    }


}
