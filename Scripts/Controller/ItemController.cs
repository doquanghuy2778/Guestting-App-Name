using com.ootii.Messages;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    private List<Transform> _listPos;
    private List<GameObject> _cellPos;
    private int _index;
    public Button Item;
    public Image Item_img;
    private bool _isDone, _isAcpReturn;
    private Dictionary<GameObject, Vector3> _dicOriginPos;
    public int ID;
    private int _indexRemove;

    void Start()
    {
        _dicOriginPos = GameController.Instance.DicOriginPos;
        _listPos = GameController.Instance.ListPos;
        _cellPos = GameController.Instance.Cells;
        Item.onClick.AddListener(MoveTargetDelay);
        Item.onClick.AddListener(MoveToOriPos);
        _isAcpReturn = false;
    }

    private void MoveTargetDelay()
    {
        StartCoroutine(MoveTarget());
    }

    private IEnumerator MoveTarget()
    {
        if (!_isDone)
        {
            _index = 0;
            _index = ItemManager.Instance.GetIndex();
            ItemManager.Instance.SubscribeItem(_index, this);
            if (_index <= _listPos.Count - 1)
            {
                Vector3 target = new Vector3(_listPos[_index].position.x,
                                            _listPos[_index].position.y,
                                            _listPos[_index].position.z);
                MessageDispatcher.SendMessage(this, Message.OnClickItem, GetNameItem(), 0);
                gameObject.transform.DOMove(target, 0.5f).OnComplete(() =>
                {
                    _cellPos[_index].GetComponent<Image>().enabled = false;
                });
                _isDone = true;
                yield return new WaitForSeconds(0.5f);
                _isAcpReturn = true;
            }
        }
    }

    private string GetNameItem()
    {
        string[] parts = Item_img.sprite.name.Split(' ');
        return parts[1];
    }

    private void MoveToOriPos()
    {
        StartCoroutine(MoveToOriginPosition()); 
    }

    IEnumerator MoveToOriginPosition()
    {
        if (_isAcpReturn)
        {
            ItemManager.Instance.UnsubcribeItem(this);
            MessageDispatcher.SendMessage(this, Message.ReturnItem, GetNameItem(), 0);
            yield return new WaitForSeconds(0.1f);
            gameObject.transform.DOMove(_dicOriginPos[gameObject], 0.5f);
            _cellPos[_index].GetComponent<Image>().enabled = true;
            _isDone = false;
            _isAcpReturn = false;
        }
    }
}
