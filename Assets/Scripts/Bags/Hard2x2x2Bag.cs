using System.Collections;
using UnityEngine;

public class Hard2x2x2Bag : MonoBehaviour, IBag
{
    [SerializeField] private Transform[] failTransforms;
    [SerializeField] private int speed = 100;
    [SerializeField] private GameObject go;
    [SerializeField] private TripleCells[] tripleCells;
    [SerializeField] private int reward;
    private bool isHorizontalRotate = true;

    [SerializeField] private Transform closingPart;
    [SerializeField] private Transform closingPartHorizontalAxis;

    private Animator animator;
    public float RotationX { get; private set; }
    public float RotationY { get; private set; }
    public int Reward {
        get { return reward; } 
        private set { reward = value; }
    }

    private void Start()
    {
        DragItem.ResetActionShowButtonGo();
        DragItem.ShowButtonGo += ShowBattonGo;

        animator = GetComponent<Animator>();
        go.SetActive(false);
    }

    public void Closing()
    {
        if ((closingPartHorizontalAxis.rotation.x + 0.0005 < 1) && isHorizontalRotate)
        {
            closingPartHorizontalAxis.Rotate(speed * Time.fixedDeltaTime, 0.0f, 0.0f);
        }
        else
        {
            isHorizontalRotate = false;
            closingPart.rotation = Quaternion.Euler(closingPart.rotation.eulerAngles.x,
                                                   closingPart.rotation.eulerAngles.y + Time.fixedDeltaTime * speed,
                                                   closingPart.rotation.eulerAngles.z);
            closingPartHorizontalAxis.rotation = Quaternion.Euler(closingPartHorizontalAxis.rotation.eulerAngles.x,
                                                   closingPartHorizontalAxis.rotation.eulerAngles.y + Time.fixedDeltaTime * speed,
                                                   closingPartHorizontalAxis.rotation.eulerAngles.z);
        }

        RotationX = closingPartHorizontalAxis.rotation.eulerAngles.x;
        RotationY = closingPart.rotation.eulerAngles.y;
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
            if (failTransform.childCount > 0)
            {
                animator.SetBool("fail", true);
                return true;
            }
        }
        foreach (var cell in tripleCells)
        {
            if ((cell.FirstCell.transform.childCount == 1) && 
                (cell.SecondCell.transform.childCount == 1 || cell.ThrirdCell.transform.childCount == 1) ||
                (cell.SecondCell.transform.childCount == 1) && (cell.ThrirdCell.transform.childCount == 1) )
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
