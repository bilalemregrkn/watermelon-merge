using System.Collections;
using UnityEngine;

public class RaycastTool : MonoBehaviour
{
    [SerializeField] private GameArea _gameArea;
    [SerializeField] private LayerMask _layerMask;

    private bool _isWorking;

    private RaycastHit2D[] _hits;

    void Update()
    {
        // Convert the mouse position to a point in the world space
        var left = _gameArea.GetBorderPositionHorizontal().x;
        var up = _gameArea.GetBorderPositionVertical().y;
        Vector2 startingPoint = new Vector2(left, up);

        // Perform the 2D raycast and get all hits
        var direction = Vector2.right;
        _hits = Physics2D.RaycastAll(startingPoint, direction, 10f,_layerMask);

        if (_hits.Length == 0)
            return;

        if (_isWorking)
            return;

        _isWorking = true;
        StartCoroutine(CheckList());
    }

    private IEnumerator CheckList()
    {
        yield return new WaitForSeconds(2);
        if (_hits.Length > 0)
        {
            GameManager.Instance.GameOver();
        }

        _isWorking = false;
    }
}