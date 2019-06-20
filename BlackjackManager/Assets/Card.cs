using UnityEngine;

[System.Serializable]
public class Card
{
    #region 欄位宣告

    private int _number;
    private float _xPosition;
    private float _yPosition;
    private float _zPosition;
    private Sprite _image;
    private Sprite _cImage;
    private bool _openCard;
    private string _objectName;

    private GameObject _gameObject;

    #endregion

    #region 屬性設定

    public int number { get { return (_number>10)?10:_number; } }
    public float xPosition { get { return _xPosition; } set { _xPosition = value; } }
    public float yPosition { get { return _yPosition; } set { _yPosition = value; } }
    public float zPosition { get { return _zPosition; } set { _zPosition = value; } }
    public Sprite image { get { return _image; } }
    public Sprite cImage { get { return _cImage; } }
    public bool openCard { get { return _openCard; } set { _openCard = value; } }
    public string objectName { get { return _objectName; }}

    #endregion

    #region 建構方法

    /// <summary>
    /// 類別Card的建構方法
    /// </summary>
    /// <param name="number">牌值，為1~10</param>
    /// <param name="xPosition">卡片生成之x座標</param>
    /// <param name="yPosition">卡片生成之y座標</param>
    /// <param name="zPosition">卡片生成之z座標(作為上下層使用)</param>
    /// <param name="openCard">卡片是否打開(預設Dealer第二張牌在遊戲開始階段是蓋著的))</param>
    /// <param name="image">使用卡圖</param>
    /// <param name="cImage">使用卡套圖</param>
    public Card ( int number, float xPosition, float yPosition, float zPosition, bool openCard, Sprite image, Sprite cImage, string objectName )
    {
        _number = number;
        _xPosition = xPosition;
        _yPosition = yPosition;
        _zPosition = zPosition;
        _openCard = openCard;
        _image = image;
        _cImage = cImage;
        _objectName = objectName;

        _gameObject = new GameObject(objectName);
        _gameObject.AddComponent<SpriteRenderer>();
        _gameObject.GetComponent<SpriteRenderer>().sprite = openCard ? image : cImage;
        _gameObject.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, zPosition);
    }

    #endregion

    #region 運算方法

    public void CardOpen(float xPosition, float yPosition, float zPosition)
    {
        _openCard = true;
        _gameObject.GetComponent<SpriteRenderer>().sprite = image;
        _gameObject.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, zPosition);
    }

    #endregion
}
