using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Holoville.HOTween;

public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;
    private GameObject[,] Grid;

    public int StartingMoves = 50;
    private int _numMoves;
    public int NumMoves
    {
        get
        {
            return _numMoves;
        }

        set
        {
            _numMoves = value;
            MovesText.text = _numMoves.ToString();
        }
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            ScoreText.text = _score.ToString();
        }
    }

    public GameObject GameOverMenu;
    public TextMeshProUGUI MovesText;
    public TextMeshProUGUI ScoreText;

    public static GridManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        Score = 0;
        NumMoves = StartingMoves;
    }

    void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        GameOverMenu.SetActive(false);
        InitGrid();
    }

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f - (Distance / 2.0f), 
                                                                  GridDimension * Distance / 2.0f - (Distance / 2.0f), 0);

        for (int row = 0; row < GridDimension; row++)
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject newTile = Instantiate(TilePrefab);

                List<Sprite> possibleSprites = new List<Sprite>(Sprites);

                //Choose what sprite to use for this cell
                Sprite left1 = GetSpriteAt(column - 1, row);
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2)
                {
                    possibleSprites.Remove(left1);
                }

                Sprite down1 = GetSpriteAt(column, row - 1);
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];

                Tile tile = newTile.AddComponent<Tile>();
                tile.Position = new Vector2Int(column, row);

                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
                
                Grid[column, row] = newTile;
            }
    }

    Sprite GetSpriteAt(int column, int row) // TODO
    {
        if (column < 0 || column >= GridDimension
         || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    SpriteRenderer GetSpriteRendererAt(int column, int row) // TODO
    {
        if (column < 0 || column >= GridDimension
         || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer;
    }

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        //SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();
        
        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
        //SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        //Sprite temp = renderer1.sprite;
        //renderer1.sprite = renderer2.sprite;
        //renderer2.sprite = temp;

        ReInitSwapedSpritesInGrid(tile1Position, tile2Position);

        bool changesOccurs = CheckMatches();
        Debug.Log(changesOccurs);
        if(!changesOccurs)
        {
            //temp = renderer1.sprite;
            //renderer1.sprite = renderer2.sprite;
            //renderer2.sprite = temp;

            ReInitSwapedSpritesInGrid(tile1Position, tile2Position);

            SoundManager.Instance.PlaySound(SoundType.TypeMove);
        }
        else
        {
            DoSwapMotion(tile1.transform, tile2.transform);

            SoundManager.Instance.PlaySound(SoundType.TypePop);
            NumMoves--;
            do
            {
                FillHoles();
            } while (CheckMatches());

            if (NumMoves <= 0)
            {
                NumMoves = 0;
                GameOver();
            }
        }

        for (int row = 0; row < 3; row++)
            for (int column = 0; column < 3; column++)
            {
                //Debug.Log(Grid[row, column].GetComponent<Tile>().Position.ToString());
            }
    }

    private void ReInitSwapedSpritesInGrid(Vector2Int tile1Position, Vector2Int tile2Position) // TODO
    {
        Vector2Int tempPosition = Grid[tile1Position.x, tile1Position.y].GetComponent<Tile>().Position;
        ///Debug.Log(tile1Position.ToString());
        //Debug.Log(tile2Position.ToString());

        Grid[tile1Position.x, tile1Position.y].GetComponent<Tile>().Position = 
            Grid[tile2Position.x, tile2Position.y].GetComponent<Tile>().Position;
        //Debug.Log("tile1 Position" + Grid[1, 1].GetComponent<Tile>().Position.ToString());

        Grid[tile2Position.x, tile2Position.y].GetComponent<Tile>().Position = tempPosition;
        //Debug.Log("tile2 Position" + Grid[1, 0].GetComponent<Tile>().Position.ToString());

        //GameObject tempTile = Grid[tile1Position.x, tile1Position.y];
        //Grid[tile1Position.x, tile1Position.y] = Grid[tile2Position.x, tile2Position.y];
        //Grid[tile2Position.x, tile2Position.y] = tempTile;
    }

    // Swap Motion Animation, to animate the switching arrays
    void DoSwapMotion(Transform a, Transform b)
    {
        Vector3 posA = a.localPosition;
        Vector3 posB = b.localPosition;
        TweenParms parms = new TweenParms().Prop("localPosition", posB).Ease(EaseType.EaseOutQuart);
        HOTween.To(a, 0.2f, parms).WaitForCompletion();
        parms = new TweenParms().Prop("localPosition", posA).Ease(EaseType.EaseOutQuart);
        HOTween.To(b, 0.2f, parms).WaitForCompletion();
    }

    bool CheckMatches()
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                SpriteRenderer current = GetSpriteRendererAt(column, row);

                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }

                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
        }
        Score += matchedTiles.Count;
        return matchedTiles.Count > 0;
    }

    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    void FillHoles() // TODO
    {
        for (int column = 0; column < GridDimension; column++)
            for (int row = 0; row < GridDimension; row++)
            {
                while (GetSpriteRendererAt(column, row).sprite == null)
                {
                    SpriteRenderer current = GetSpriteRendererAt(column, row);
                    SpriteRenderer next = current;
                    for (int filler = row; filler < GridDimension - 1; filler++)
                    {
                        next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                        current = next;
                    }
                    next.sprite = Sprites[Random.Range(0, Sprites.Count)];
                }
            }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        PlayerPrefs.SetInt("score", Score);
        GameOverMenu.SetActive(true);
        SoundManager.Instance.PlaySound(SoundType.TypeGameOver);
    }
}
