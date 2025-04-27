
public interface ISFXTrack : ITrack {

    // Sets the pitch to the given value along it's range from (0 to maxRange) to the sound's internal pitch range
    // Note: if the programmer's range does not start from 0, the range will not scale properly
    public void SetPitch(float currentValue, float maxValue);
}