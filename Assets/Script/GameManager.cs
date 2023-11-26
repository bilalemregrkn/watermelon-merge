using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private FruitObjectSetting setting;
    [SerializeField] private GameArea gameArea;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private TextMeshProUGUI textScore;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            if (value == 0)
                return;

            textScore.SetText(value.ToString());
        }
    }

    private int _score;

    private bool IsClick => Input.GetMouseButtonDown(0);
    private readonly Vector2Int _fruitRange = new Vector2Int(0, 4);

    private void Awake()
    {
        Instance = this;
    }

    private float GetInputHorizontalPosition()
    {
        var inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        var limit = gameArea.GetBorderPositionHorizontal();
        var result = Mathf.Clamp(inputPosition, limit.x, limit.y);
        return result;
    }

    private void OnClicked()
    {
        var index = Random.Range(_fruitRange.x, _fruitRange.y);
        var spawnPosition = new Vector2(GetInputHorizontalPosition(), spawnPoint.position.y);
        SpawnFruit(index, spawnPosition);
    }

    private void SpawnFruit(int index, Vector2 position)
    {
        var prefab = setting.SpawnObject;
        var fruitObject = Instantiate(prefab, position, quaternion.identity);
        var sprite = setting.GetSprite(index);
        var scale = setting.GetScale(index);
        fruitObject.Prepare(sprite, index, scale);
    }

    public void Merge(FruitObject first, FruitObject second)
    {
        var type = first.type + 1;
        var spawnPosition = (first.transform.position + second.transform.position) * 0.5f;
        Destroy(first.gameObject);
        Destroy(second.gameObject);
        SpawnFruit(type, spawnPosition);

        Score += type * 2;
    }

    private void Update()
    {
        if (IsClick)
        {
            OnClicked();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}