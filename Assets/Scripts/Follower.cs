using UnityEngine;

public class Follower : MonoBehaviour
{
    public float Speed = 1;
    public float StartRotation = 270f;
    public int StartPosition = 0;

    public int CurrentPosition { get; set; }
    public int PreviousPosition { get; set; }
    public Animator Animator { get; private set; }

    private void Start()
    {
        CurrentPosition = StartPosition;
        PreviousPosition = StartPosition;
        Animator = transform.GetComponent<Animator>();
    }
}
