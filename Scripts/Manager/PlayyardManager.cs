using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayyardManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _spritesGdGet;
    [SerializeField] private List<Sprite> _spritesForLevel;
    public InputField NumWords, NameColor;
    [SerializeField] Button _blackBT, _whiteBT, _yellowBT;
    [SerializeField] Button _getButton;
    [SerializeField] Image ImagePrefab;
    [SerializeField] Transform _creatingTransform;
    private string NameColor2;
    public int Number;
    private static PlayyardManager _instance;
    public static PlayyardManager Instance { get => _instance; }
     
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _blackBT.onClick.AddListener(ButtonBlack);
        _whiteBT.onClick.AddListener(ButtonWhite);
        _yellowBT.onClick.AddListener(ButtonYellow);
        _getButton.onClick.AddListener(GDGetSprite);
    }

    public void GDGetSprite()
    {
        if (NameColor != null && NumWords != null)
        {
            Number = int.Parse(NumWords.text);
            string key = "number" + SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt(key, Number);
            PlayerPrefs.Save();
            string path = NameColor.text + "/" + NameColor2 + NameColor.text;
            Object[] sprites = Resources.LoadAll(path, typeof(Sprite));
            foreach (Object obj in sprites) 
            {
                _spritesGdGet.Add(obj as Sprite);
                CreateObject(obj as Sprite);  
            }
            Debug.Log(path);
        }
        else
        {
            Debug.LogError("Chua Nhap Du Thong Tin");
        }
    }

    private void CreateObject(Sprite sprite)
    {
        Image image = Instantiate(ImagePrefab, _creatingTransform);
        image.sprite = sprite;
        image.gameObject.name = sprite.name;
        image.gameObject.tag = "Item";
        image.enabled = false;
        image.raycastTarget = false;
    }

    private void ButtonBlack()
    {
        NameColor2 = "Black_";
    }

    private void ButtonWhite()
    {
        NameColor2 = "White_";
    }

    private void ButtonYellow()
    {
        NameColor2 = "Yellow_";
    }
}
