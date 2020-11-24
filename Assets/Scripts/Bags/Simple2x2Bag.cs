using System;
using System.Collections;
using UnityEngine;

public class Simple2x2Bag : MonoBehaviour, IBag
{
    [SerializeField] private Transform[] failTransforms;
    [SerializeField] private int speed = 100;
    [SerializeField] private GameObject go;
    [SerializeField] private PairCells[] pairCells;
    [SerializeField] private int reward;

    [SerializeField] private Transform closingPart;

    private Animator animator;

    public int Reward
    {
        get { return reward; }
        private set { reward = value; }
    }
    public float RotationX { get; private set; }
    public float RotationY { get; private set; }

    private void Start()
    {
        DragItem.ResetActionShowButtonGo();
        DragItem.ShowButtonGo += ShowBattonGo;
        animator = GetComponent<Animator>();
        go.SetActive(false);
    }

    public void Closing()
    {
        closingPart.rotation = Quaternion.Euler(closingPart.rotation.eulerAngles.x,
                                                closingPart.rotation.eulerAngles.y + Time.fixedDeltaTime * speed,
                                                closingPart.rotation.eulerAngles.z);

        RotationX = closingPart.eulerAngles.x;
        RotationY = closingPart.eulerAngles.y;
    }

    public bool IsFail()
    {
        if (failTransforms == null)
        {
            animator.SetBool("success", true);
            return false;
        }

        foreach (var failTransform in failTransforms)
        {
            if(failTransform.childCount > 0)
            {
                animator.SetBool("fail", true);
                return true;
            }
        }
        foreach (var cell in pairCells)
        {
            if((cell.FirstCell.transform.childCount == 1) &&
               (cell.SecondCell.transform.childCount == 1))
            {
                animator.SetBool("fail", true);
                return true;
            }
        }

        animator.SetBool("success", true);
        return false;
    }

    public void ClickGo()
    {
        BagsController.isGo = true;
        go.SetActive(false);
    }
    public void CloseBag()
    {
        StartCoroutine(Close());        
    }
    private IEnumerator Close()
    {
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("success", false);
        animator.SetBool("fail", false);
        transform.gameObject.SetActive(false);
        Destroy(transform.gameObject); 
        if (IsFail())
        {
            FollowerController.MoveRight?.Invoke();
        }
        else
        {
            FollowerController.MoveLeft?.Invoke();
            SaveManager.Instance.SaveGame();
        }
    }

    private void ShowBattonGo(bool isVisible)
    {
        go.SetActive(isVisible);
    }
}