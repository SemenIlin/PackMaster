public interface IBag 
{
    float RotationX { get; }
    float RotationY { get; }
    bool IsFail();
    void Closing();
    int Reward { get; }
}
