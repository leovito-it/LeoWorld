using UnityEngine;
using UnityEngine.UI;

public class Tile
{
    public TileGrid Grid { get; private set; }
    public int Row { get; private set; }
    public int Col { get; private set; }
    public int Weight { get; set; }
    public int Cost { get; set; }
    public Tile PrevTile { get; set; }

    private GameObject _gameObject;
    private Image _spriteRenderer;
    private Text _textComponent;
    private Button _buttonComponent;

    public Tile(TileGrid grid, int row, int col, int weight)
    {
        Grid = grid;
        Row = row;
        Col = col;
        Weight = weight;
    }

    public void InitGameObject(Transform parent, GameObject prefab)
    {
        _gameObject = GameObject.Instantiate(prefab,parent);
        _gameObject.name = $"Tile({Row}, {Col})";
        _gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        _spriteRenderer = _gameObject.GetComponent<Image>();
        _textComponent = _gameObject.GetComponentInChildren<Text>();
        _buttonComponent = _gameObject.TryGetComponent(out _buttonComponent) ? _buttonComponent : 
            _gameObject.GetComponent<Button>();
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void SetText(string text)
    {
        _textComponent.text = text;
    }

    public GameObject GetGameObject()
    {
        return _gameObject;
    }

    public Button GetButton()
    {
        return _buttonComponent;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(Col, Row);
    }
}