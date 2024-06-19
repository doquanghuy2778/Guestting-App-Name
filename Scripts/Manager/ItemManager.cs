using com.ootii.Messages;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int Index;
    private static ItemManager _instance;
    public static ItemManager Instance { get => _instance; }
    public List<char> Alphabets;
    public string NameUserGet;
    public List<int> _indexRemove;
    int a = 0;
    public bool IsRemoveDone;
    private List<GameObject> _cellsPos => GameController.Instance.Cells;
    private ItemController[] _items;
    //private bool test;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _indexRemove = new List<int>();
        Index = 0;
        Alphabets = new List<char>();
    }

    private void OnEnable()
    {
        MessageDispatcher.AddListener(Message.OnClickItem, IndexPlus, true);
        MessageDispatcher.AddListener(Message.ReturnItem, RemoveItem, true);
    }

    private void OnDisable()
    {
        MessageDispatcher.RemoveListener(Message.OnClickItem, IndexPlus, true);
        MessageDispatcher.RemoveListener(Message.ReturnItem, RemoveItem, true);
    }

    private void IndexPlus(IMessage message)
    {
        string name = (string)message.Data;
        Alphabets.Add(char.Parse(name));
    }

    private void RemoveItem(IMessage message)
    {
        a = Index;
        IsRemoveDone = true;
        string nameItem = (string)message.Data;
        Alphabets.Remove(char.Parse(nameItem));
    }

    public string GetName()
    {
        string result = "";
        foreach (var obj in Alphabets)
        {
            result += obj.ToString();
        }
        return result;
    }

    public void SubscribeItem(int index, ItemController item)
    {
        _items[index] = item;
    }

    public void UnsubcribeItem(ItemController item)
    {
        for(int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == item)
            {
                _items[i] = null;
            }
        }
    }

    public int GetIndex()
    {
        if(_items == null)
            _items = new ItemController[_cellsPos.Count];

        for (int i = 0; i < _cellsPos.Count; i++) 
        {
            if (_items[i] == null)
                return i;
        }
        return _cellsPos.Count - 1;
    }
}
