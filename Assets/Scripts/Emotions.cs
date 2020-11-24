using UnityEngine;
using UnityEngine.UI;

public class Emotions : MonoBehaviour
{
    [SerializeField] private Sprite angry;
    [SerializeField] private Sprite smile;
    [SerializeField] private Sprite empty;
    private Image emotion;
    private void Start()
    {
        emotion = GetComponent<Image>();
    }

    public void SetEmotionAngry()
    {
        emotion.sprite = angry;
    }

    public void SetEmotionSmile()
    {
        emotion.sprite = smile;
    }

    public void ResetEmotion()
    {
        emotion.sprite = empty;
    }
}
