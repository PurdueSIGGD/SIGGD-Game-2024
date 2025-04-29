using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicDriver : MonoBehaviour
{
    [SerializeField] private MusicTrackName roomMusicTrack;

    private EnemySpawning enemySpawning;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawning = FindObjectOfType<EnemySpawning>();
        if (AudioManager.Instance.MusicBranch.GetCurrentMusicTrackName() == roomMusicTrack) return;
        if (AudioManager.Instance.MusicBranch.GetCurrentMusicTrackName() == MusicTrackName.NULL)
        {
            AudioManager.Instance.MusicBranch.PlayMusicTrack(roomMusicTrack);
            return;
        }
        AudioManager.Instance.MusicBranch.CrossfadeTo(roomMusicTrack, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawning == null) return;
        if (enemySpawning.GetCurrentEnemies().Count <= 0)
        {
            AudioManager.Instance.SetEnergyLevel(0f);
            return;
        }
        float combatEnergyLevel = Mathf.Min(0.5f + (enemySpawning.GetCurrentEnemies().Count * 0.06f), 1f);
        AudioManager.Instance.SetEnergyLevel(combatEnergyLevel);
    }
}
