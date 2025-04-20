
public interface IMusicTrack : ITrack {
    public void SetTrackVolume(float volume);
    public float GetTrackVolume();

    // Since these are long audio files, stopping them might be practical
    public void StopTrack();
}