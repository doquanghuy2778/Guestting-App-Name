using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private GameObject _cellPrefab, _cellTarget;
    public Transform CellPos, CellTarget, CellPos_2, CellPos_3;
    public ObjectDatas ObjectDatas;
    public int Numbers;
    public string NameLevel;
    public GameObject[] _objects;
    private List<GameObject> _listLevel;
    private static GameController _instance;
    public static GameController Instance { get => _instance; }
    public List<Transform> ListPos;
    public GridLayoutGroup GridLayoutGroup;
    public Dictionary<GameObject, Vector3> DicOriginPos;
    GameObject[] itemLevel;
    [SerializeField] TextMeshProUGUI _description;
    public List<GameObject> Cells, Items;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        DicOriginPos = new Dictionary<GameObject, Vector3>();
        Cells = new List<GameObject>();
        Items = new List<GameObject>(); 
        _objects = GameObject.FindGameObjectsWithTag("Item");
        _listLevel = new List<GameObject>();
        GenLevel();
    }

    public void GenLevel()
    {
        Numbers = 0;
        Data itemLevel;
        int levelIndex = PlayerPrefs.GetInt("stage", 0);
        itemLevel = ObjectDatas.datas[levelIndex];
        NameLevel = itemLevel.Name;
        _sprite.sprite = itemLevel.Sprite;
        Numbers = itemLevel.Number;
        _description.text = itemLevel.Description;  
        GetCellPos(Numbers);
        GetItem(itemLevel.Name);
    }

    //gen item trong level
    private void GenBlankCells(int nums)
    {
        for (int i = 0; i < nums; i++)
        {
            GameObject picture = Instantiate(_cellPrefab, CellPos);
            picture.tag = "ItemPlay";
            picture.GetComponent<Image>().sprite = _listLevel[i].GetComponent<Image>().sprite;
            picture.name = picture.GetComponent<Image>().sprite.name;
            picture.GetComponent<ItemController>().ID = i;
            Items.Add(picture);
        }
        StartCoroutine(GetOriPos());    
    }

    //gen vi tri cac o trong
    private void GetCellPos(int nums)
    {
        string[] part = NameLevel.Split(' ');
        if(part.Length < 2) 
        {
            for (int i = 0; i < nums; i++)
            {
                CreatItem(_cellTarget, CellTarget);
            }
        }
        else if (part.Length == 2)
        {
            for (int i = 0; i < part[0].Length; i++)
            {
                CreatItem(_cellTarget, CellTarget);
            }

            for (int i = 0; i < part[1].Length; i++)
            {
                CreatItem(_cellTarget, CellPos_2);
            }
        }
        else if(part.Length > 2) 
        {
            for (int i = 0; i < part[0].Length; i++)
            {
                CreatItem(_cellTarget, CellTarget);
            }

            for (int i = 0; i < part[1].Length; i++)
            {
                CreatItem(_cellTarget, CellPos_2);
            }

            for (int i = 0; i < part[2].Length; i++)
            {
                CreatItem(_cellTarget, CellPos_3);
            }
        }
    }

    private void GetItem(string name)
    {
        if(_listLevel.Count > 0)
        {
            _listLevel.Clear();
        }
        char[] nameObj = name.Replace(" ", "").ToUpper().ToCharArray();
        foreach (var item in nameObj)
        {
            foreach (var obj in _objects)
            {
                string[] parts = obj.name.Split(' ');
                char objCheck = char.Parse(parts[1]);
                if (item == objCheck)
                {
                    _listLevel.Add(obj);
                }
            }
        }

        //gen cac item random  
        string key = "number" + SceneManager.GetActiveScene().buildIndex;
        int num = PlayerPrefs.GetInt(key);
        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, _objects.Length - 1);
            _listLevel.Add(_objects[index]);
        }
        FisherYates(_listLevel);
    }

    //xáo trộn danh sách
    private void FisherYates(List<GameObject> objects)
    {
        System.Random rng = new System.Random();
        int count = objects.Count;
        while(count > 1)
        {
            count--;
            int k = rng.Next(count + 1);
            GameObject obj = objects[k];
            objects[k] = objects[count];
            objects[count] = obj; 
        }
        GenBlankCells(_listLevel.Count);
    }

    IEnumerator GetOriPos()
    {
        yield return new WaitForSeconds(0.5f);
        itemLevel = GameObject.FindGameObjectsWithTag("ItemPlay");
        foreach (GameObject obj in itemLevel) 
        {
            DicOriginPos.Add(obj, obj.transform.position);
        }
    }

    private void CreatItem(GameObject itemPrefab, Transform transform)
    {
        GameObject picture = Instantiate(itemPrefab, transform);
        ListPos.Add(picture.transform);
        Cells.Add(picture);
    }
}
