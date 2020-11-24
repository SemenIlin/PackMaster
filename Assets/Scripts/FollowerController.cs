using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    private const int POSITION_AT_COUNTER = 4;
    [SerializeField] private Path path;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private GameObject emotion;
    [SerializeField] private BagsController bagsController;
    [SerializeField] private ScoreController scoreController;

    [SerializeField] private GameObject bag;
    [SerializeField] private Path pathForBag;
    [SerializeField] private int speed = 10;

    private Emotions emotions;

    private bool IsMove = false;
    private bool IsClick = false;
    private bool IsSmile = false;

    private List<Follower> currentCharacters = new List<Follower>();

    public static event Action<bool> ShowButton;
    public static Action MoveLeft;
    public static Action MoveRight;

    private void Start()
    {
        MoveLeft += MoveToLeft;
        MoveRight += MoveToRight;
        if (characterPrefabs == null)
        {
            return;
        }
        ButtonController.ButtonClicks += MoveToRight;
        emotions = emotion.GetComponent<Emotions>();

        bagsController.InstantiateBag();
        scoreController.ShowReward(bagsController.ibag.Reward);

        Init();
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < currentCharacters.Count; i++)
        {
            if (currentCharacters[i].PreviousPosition == POSITION_AT_COUNTER - 1)
            {
                currentCharacters[i].transform.position = Vector3.MoveTowards(currentCharacters[i].transform.position,
                                                                         path.PathElements[currentCharacters[i].CurrentPosition].position,
                                                                         Time.fixedDeltaTime * currentCharacters[i].Speed);
                if (IsClick)
                {
                    scoreController.HideReward();
                    if (currentCharacters[i].transform.position != path.PathElements[currentCharacters[i].CurrentPosition].position)
                    {
                        currentCharacters[i].Animator.SetBool("walk", true);
                        if (bagsController.isFail)
                        {
                            bag.transform.position = Vector3.MoveTowards(bag.transform.position,
                                                                         pathForBag.PathElements[1].position,
                                                                         Time.deltaTime * speed);
                        }
                        else
                        {
                            bag.transform.position = Vector3.MoveTowards(bag.transform.position,
                                                                         pathForBag.PathElements[2].position,
                                                                         Time.deltaTime * speed);
                        }
                        ShowButton?.Invoke(false);

                    }
                    else
                    {
                        currentCharacters[i].Animator.SetBool("walk", false);
                        DestroyCharacter(i);
                        i = 0;
                        IsMove = true;
                        IsClick = false;
                    }
                }
            }
            if (IsMove)
            {
                bag.transform.position = Vector3.MoveTowards(bag.transform.position,
                                                             pathForBag.PathElements[0].position,
                                                             Time.deltaTime * speed);

                if (currentCharacters[i].PreviousPosition == POSITION_AT_COUNTER - 1)
                {
                    continue;
                }
                currentCharacters[i].transform.position = Vector3.MoveTowards(currentCharacters[i].transform.position,
                                                                      path.PathElements[currentCharacters[i].CurrentPosition].position,
                                                                      Time.fixedDeltaTime * currentCharacters[i].Speed);

                if (currentCharacters[i].transform.position != path.PathElements[currentCharacters[i].CurrentPosition].position)
                {
                    currentCharacters[i].Animator.SetBool("walk", true);
                }
                else
                {
                    currentCharacters[i].Animator.SetBool("walk", false);                    
                }

                var valueFinishetCharacters = 0;
                foreach (var character in currentCharacters)
                {
                    if (!character.Animator.GetBool("walk"))
                        valueFinishetCharacters++;
                }
                if (valueFinishetCharacters == currentCharacters.Count) 
                {
                    IsMove = false;
                    bagsController.InstantiateBag();
                    scoreController.ShowReward(bagsController.ibag.Reward);

                    ShowButton?.Invoke(!IsMove);
                    SpawnCharacter();
                }
                valueFinishetCharacters = 0;
            }
        }
    }
    private void MoveToLeft()
    {
        IsSmile = true;
        StartCoroutine(Emotion(IsSmile));
        SetNextPoints();
        IsClick = true;
        TransformDiretion();
    }

   private void MoveToRight()
   {
        IsSmile = false;
        bagsController.isFail = true;
        StartCoroutine(Emotion(IsSmile));
        SetNextPoints();
        IsClick = true;
        TransformDiretion();
    }

    private void SetEmotion(bool isSuccess)
    {
        if (isSuccess)
        {
            emotions.SetEmotionSmile();
            scoreController.UpdateScore(bagsController.ibag.Reward);
        }
        else
            emotions.SetEmotionAngry();
    }

    private IEnumerator Emotion(bool isSuccess)
    {
        SetEmotion(isSuccess);
        yield return new WaitForSeconds(1.5f);
        emotions.ResetEmotion();
    }
    private void SetNextPoints()
    {
        var lengthPathElement = path.PathElements.Length;

        if ((path == null) ||
           (lengthPathElement < 1) ||
           (characterPrefabs == null))
        {
            return;
        }

        for (var i = 0; i < currentCharacters.Count; i++)
        {
            if (currentCharacters[i].CurrentPosition == lengthPathElement - 3)
            {
                if (!bagsController.isFail)
                {
                    currentCharacters[i].CurrentPosition = lengthPathElement - 1;
                }
                else
                {
                    currentCharacters[i].CurrentPosition = lengthPathElement - 2;
                }
                currentCharacters[i].PreviousPosition = lengthPathElement - 3;
            }

            else if (currentCharacters[i].CurrentPosition < lengthPathElement - 3)
            {
                currentCharacters[i].PreviousPosition = currentCharacters[i].CurrentPosition;
                currentCharacters[i].CurrentPosition++;
            }
        }
    }
    private void TransformDiretion()
    {
        var x = 0f;
        var z = 0f;

        for (var i = 0; i < currentCharacters.Count; i++)
        {
            x = path.PathElements[currentCharacters[i].CurrentPosition].position.x - 
                path.PathElements[currentCharacters[i].PreviousPosition].position.x;
            z = path.PathElements[currentCharacters[i].CurrentPosition].position.z -
                path.PathElements[currentCharacters[i].PreviousPosition].position.z;

            var angle = Mathf.Atan(z / x) * 180 / Mathf.PI;

            var rotation = x <= 0 ? currentCharacters[i].StartRotation : currentCharacters[i].StartRotation - 180;

            currentCharacters[i].transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                                  rotation - angle,
                                                  transform.rotation.eulerAngles.z);
        }
    }
    private void Init()
    {
        int startPosition;
        Vector3 position;
        Quaternion rotation;

        for (var i = 0; i < POSITION_AT_COUNTER; i++)
        {
            var character = Instantiate(characterPrefabs[UnityEngine.Random.Range(0, characterPrefabs.Length)]);
            startPosition = character.GetComponent<Follower>().StartPosition = i;

            position = path.PathElements[startPosition].position;
            rotation = path.PathElements[startPosition].rotation;

            character.transform.position = position;
            character.transform.rotation = rotation;

            currentCharacters.Add(character.GetComponent<Follower>());
        }

    }
    private void SpawnCharacter()
    {
        int startPosition;
        Vector3 position;
        Quaternion rotation;

        var character = Instantiate(characterPrefabs[UnityEngine.Random.Range(0, characterPrefabs.Length)]);
        startPosition = character.GetComponent<Follower>().StartPosition = 0;

        position = path.PathElements[startPosition].position;
        rotation = path.PathElements[startPosition].rotation;

        character.transform.position = position;
        character.transform.rotation = rotation;

        currentCharacters.Add(character.GetComponent<Follower>());
    }
    private void DestroyCharacter(int position)
    {
        Destroy(currentCharacters[position].gameObject);
        currentCharacters.RemoveAt(position);
    }
}
