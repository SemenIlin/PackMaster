using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private GameObject rewardImage;

    [SerializeField] private Text rewardText;

    private int currentScore = 0;

    private void Start()
    {
        score.text = currentScore.ToString();
        rewardImage.SetActive(false);
    }

    public int CurrentScore { 
        get { return currentScore; }
        private set { currentScore = value; } 
    }
    public void UpdateScore(int reward)
    {
        CurrentScore += reward;
        score.text = CurrentScore.ToString();
    }

    public void ShowReward(int reward)
    {
        rewardText.text = reward.ToString();
        rewardImage.SetActive(true);
    }

    public void HideReward()
    {
        rewardImage.SetActive(false);
    }
}
