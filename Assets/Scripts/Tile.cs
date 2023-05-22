using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum ItemType {jellyfish, crab, oyster, wave, beachBall, starfish, leaf, tree }

    public Sprite[] tileSprites;

    [Header("Item Properties")]
    [SerializeField] private string tileName = "";
    public bool isUnder = false;
    public bool isFinal = false;
    public ItemType itemType;

    [Header("Item Components")]
    [SerializeField] private Image mainImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image disableImage;

    private int myIindex, enemyIndex;
    //[SerializeField] private 

    private void Awake()
    {
        // Manuel
        mainImage = gameObject.GetComponent<Image>();
        itemImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        disableImage = gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();

    }
    public void SetupTile(ItemType type)
    {
        itemType = type;
        itemImage.sprite = tileSprites[((int)type)];

        Initialize();
    }

    public void ReadyTile()
    {
        isFinal = false;
    }

    private void Initialize()
    {
        tileName = itemImage.sprite.name;
        CheckItem(false);

        if (isUnder)
        {
            CheckItem(true);
            mainImage.raycastTarget = false;
        }
    }

    public void CheckItem(bool value)
    {
        disableImage.enabled = value;
        if (value)
        {
            isUnder = true;
            disableImage.enabled = !value;
        }
    }

    public void DisableItem()
    {
        isUnder = true;
        disableImage.enabled = true;
    }

    public void EnableItem()
    {
        isUnder = false;
        disableImage.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("<color=yellow> Down </color>");
        // Start animation of scaling


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //print("<color=red> Up </color>");
        // Stop animation of scaling


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //print("<color=green> Click </color>");
        // Stop animation of scaling
        // Check for game manager

        if (isUnder) return;
        GameManager.Instance.CheckTile(this);
    }
}
