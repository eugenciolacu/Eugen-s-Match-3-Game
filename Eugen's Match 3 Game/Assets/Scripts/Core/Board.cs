using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {

    public GameObject m_ingredientRef;

    public int m_height = 9;
    public int m_width = 9;

    public GameObject[,] m_board;

    public List<GameObject> m_ingredientsToMoveDown = new List<GameObject>();

    public int m_score = 0;
    
    public int m_moves = 20;

    public RectTransform m_scoreText;
    public RectTransform m_movesLeftText;

    public RectTransform m_gameOverPanelRef;
    public RectTransform m_scoreMsgRef;
    public RectTransform m_scorePanelRef;
    public RectTransform m_movesPanelRef;

    private Vector2 m_touchMovement;

    private int m_minSwipeDistance = 10;

    private int i, j;

    private bool m_isMouving = false;

    // directions
    // "none", "left", "right", "up", "down"

    private string GetDirection(Vector2 swipeMovement)
    {
        string swipeDir = "none";

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = swipeMovement.x > 0 ? "right" : "left";
        }
        else
        {
            swipeDir = swipeMovement.y > 0 ? "up" : "down";
        }

        return swipeDir;
    }

    private int GetX(Touch touch)
    {
        float x = Camera.main.ScreenToWorldPoint(touch.position).x;

        return Mathf.RoundToInt(x);
    }

    private int GetY(Touch touch)
    {
        float y = Camera.main.ScreenToWorldPoint(touch.position).y;

        return Mathf.RoundToInt(y);
    }








    void Awake()
    {
        m_board = new GameObject[m_height, m_width];
        m_scoreText.GetComponent<Text>().text = "Score:\n" + m_score.ToString(); ;
        m_movesLeftText.GetComponent<Text>().text = "Moves left:\n" + m_moves.ToString();
    }

    private void InstantiateIngredientAtIndex(int i, int j)
    {
        if (m_board[i, j] != null)
        {
            DestroyImmediate(m_board[i, j]);
        }

        m_board[i, j] = Instantiate(m_ingredientRef, new Vector3(j, i, 0f), Quaternion.identity);
        m_board[i, j].transform.parent = transform;
        m_board[i, j].GetComponent<Ingredient>().SetIJ(i, j);
    }

    private void PopulateUpperRow()
    {
        int i = m_height - 1;
        for (int j = 0; j < m_width; j++)
        {
            if (m_board[i, j] == null)
            {
                
                InstantiateIngredientAtIndex(i, j);

                string ij1 = m_board[i, j].GetComponent<Ingredient>().m_ingredient;
                string j1, j2;

                if (j >= 2)
                {
                    j1 = m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient;
                    j2 = m_board[i, j - 2].GetComponent<Ingredient>().m_ingredient;

                    if (ij1 == j1 && ij1 == j2)
                    {
                        do
                        {
                            InstantiateIngredientAtIndex(i, j);
                        }
                        while (ij1 == m_board[i, j].GetComponent<Ingredient>().m_ingredient);
                    }
                }

                string ij2 = m_board[i, j].GetComponent<Ingredient>().m_ingredient;
                string i1, i2;

                if (m_board[i - 1, j] != null && m_board[i - 2, j] != null)
                {
                    i1 = m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient;
                    i2 = m_board[i - 2, j].GetComponent<Ingredient>().m_ingredient;

                    if (ij2 == i1 && ij2 == i2)
                    {
                        do
                        {
                            InstantiateIngredientAtIndex(i, j);
                        }
                        while (ij2 == m_board[i, j].GetComponent<Ingredient>().m_ingredient || ij1 == m_board[i, j].GetComponent<Ingredient>().m_ingredient);
                    }
                } 
            }
        }
    }

    public bool CheckIfIngredientsAreMouvingDown()
    {
        foreach (Transform elem in transform)
        {
            if (elem.GetComponent<Ingredient>().m_isMovingDown == true)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckIfIngredientsAreMouving()
    {
        foreach (Transform elem in transform)
        {
            if (elem.GetComponent<Ingredient>().m_isMoving == true)
            {
                return true;
            }
        }

        return false;
    }

    public void ReconstructBoard()
    {
        GameObject[,] m_boardNew = new GameObject[m_height, m_width];

        foreach (Transform elem in transform)
        {
            int ii = elem.gameObject.GetComponent<Ingredient>().i;
            int jj = elem.gameObject.GetComponent<Ingredient>().j;

            m_boardNew[ii, jj] = elem.gameObject;
        }

        m_board = m_boardNew;
    }

    public bool DetectIngredientsToMoveDown()
    {
        m_ingredientsToMoveDown.Clear();

        for (int i = 1; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (m_board[i, j] != null && m_board[i - 1, j] == null && m_board[i, j].GetComponent<Ingredient>().m_isMovingDown == false)
                {
                    int ii = i;

                    while(ii < m_height)
                    {
                        m_ingredientsToMoveDown.Add(m_board[ii, j]);
                        ii++;
                    }
                }
            }
        }

        return m_ingredientsToMoveDown.Count > 0 ? true : false;
    }

    public void MoveDown()
    {
        foreach (GameObject elem in m_ingredientsToMoveDown)
        {
            if (elem != null)
                elem.GetComponent<Ingredient>().m_isMovingDown = true;
        }
    }

    public void CheckMatches()
    {
        if (transform.childCount != m_height * m_width)
            return;

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                CheckMatch_5_horizontal(i, j);
                CheckMatch_4_horizontal(i, j);
                CheckMatch_3_horizontal(i, j);

                CheckMatch_5_vertical(i, j);
                CheckMatch_4_vertical(i, j);
                CheckMatch_3_vertical(i, j);
            }
        }

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (m_board[i, j] != null && m_board[i, j].GetComponent<Ingredient>().name == "destroy")
                {
                    m_score += m_board[i, j].GetComponent<Ingredient>().pointsHorizontalMatch;
                    m_score += m_board[i, j].GetComponent<Ingredient>().pointsVerticalMatch;
                    m_scoreText.GetComponent<Text>().text = "Score:\n" + m_score.ToString();
                    DestroyImmediate(m_board[i, j]);
                }
            }
        }
    }

    private void CheckMatch_5_vertical(int i, int j)
    {
        if (i >= 4 && m_board[i - 4, j] != null && m_board[i - 3, j] != null && m_board[i - 2, j] != null && m_board[i - 1, j] != null && m_board[i, j] != null)
        {
            if (m_board[i - 4, j].GetComponent<Ingredient>().m_ingredient == m_board[i - 3, j].GetComponent<Ingredient>().m_ingredient
                && m_board[i - 3, j].GetComponent<Ingredient>().m_ingredient == m_board[i - 2, j].GetComponent<Ingredient>().m_ingredient
                && m_board[i - 2, j].GetComponent<Ingredient>().m_ingredient == m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient
                && m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient == m_board[i, j].GetComponent<Ingredient>().m_ingredient)
            {
                m_board[i - 4, j].GetComponent<Ingredient>().SetPointsVerticalMatch(30);
                m_board[i - 4, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i - 3, j].GetComponent<Ingredient>().SetPointsVerticalMatch(30);
                m_board[i - 3, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i - 2, j].GetComponent<Ingredient>().SetPointsVerticalMatch(30);
                m_board[i - 2, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i - 1, j].GetComponent<Ingredient>().SetPointsVerticalMatch(30);
                m_board[i - 1, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j].GetComponent<Ingredient>().SetPointsVerticalMatch(30);
                m_board[i, j].GetComponent<Ingredient>().name = "destroy";
            }
        }
    }

    private void CheckMatch_4_vertical(int i, int j)
    {
        if (i >= 3 && m_board[i - 3, j] != null && m_board[i - 2, j] != null && m_board[i - 1, j] != null && m_board[i, j] != null)
        {
            if (m_board[i - 3, j].GetComponent<Ingredient>().m_ingredient == m_board[i - 2, j].GetComponent<Ingredient>().m_ingredient
                && m_board[i - 2, j].GetComponent<Ingredient>().m_ingredient == m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient
                && m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient == m_board[i, j].GetComponent<Ingredient>().m_ingredient)
            {
                m_board[i - 3, j].GetComponent<Ingredient>().SetPointsVerticalMatch(20);
                m_board[i - 3, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i - 2, j].GetComponent<Ingredient>().SetPointsVerticalMatch(20);
                m_board[i - 2, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i - 1, j].GetComponent<Ingredient>().SetPointsVerticalMatch(20);
                m_board[i - 1, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j].GetComponent<Ingredient>().SetPointsVerticalMatch(20);
                m_board[i, j].GetComponent<Ingredient>().name = "destroy";
            }
        }
    }

    private void CheckMatch_3_vertical(int i, int j)
    {
        if (i >= 2 && m_board[i - 2, j] != null && m_board[i - 1, j] != null && m_board[i, j] != null)
        {
            if (m_board[i - 2, j].GetComponent<Ingredient>().m_ingredient == m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient
                && m_board[i - 1, j].GetComponent<Ingredient>().m_ingredient == m_board[i, j].GetComponent<Ingredient>().m_ingredient)
            { 
                m_board[i - 2, j].GetComponent<Ingredient>().SetPointsVerticalMatch(15);
                m_board[i - 2, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i - 1, j].GetComponent<Ingredient>().SetPointsVerticalMatch(15);
                m_board[i - 1, j].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j].GetComponent<Ingredient>().SetPointsVerticalMatch(15);
                m_board[i, j].GetComponent<Ingredient>().name = "destroy";
            }
        }
    }

    private void CheckMatch_5_horizontal(int i, int j)
    {
        if (j >= 4 && m_board[i, j - 4] != null && m_board[i, j - 3] != null && m_board[i, j - 2] != null && m_board[i, j - 1] != null && m_board[i, j] != null)
        {
            if (m_board[i, j - 4].GetComponent<Ingredient>().m_ingredient == m_board[i, j - 3].GetComponent<Ingredient>().m_ingredient 
                && m_board[i, j - 3].GetComponent<Ingredient>().m_ingredient == m_board[i, j - 2].GetComponent<Ingredient>().m_ingredient
                && m_board[i, j - 2].GetComponent<Ingredient>().m_ingredient == m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient
                && m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient == m_board[i, j].GetComponent<Ingredient>().m_ingredient)
            {
                m_board[i, j - 4].GetComponent<Ingredient>().SetPointsHorizontalMatch(30);
                m_board[i, j - 4].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j - 3].GetComponent<Ingredient>().SetPointsHorizontalMatch(30);
                m_board[i, j - 3].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j - 2].GetComponent<Ingredient>().SetPointsHorizontalMatch(30);
                m_board[i, j - 2].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j - 1].GetComponent<Ingredient>().SetPointsHorizontalMatch(30);
                m_board[i, j - 1].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j].GetComponent<Ingredient>().SetPointsHorizontalMatch(30);
                m_board[i, j].GetComponent<Ingredient>().name = "destroy";
            }
        }
    }

    private void CheckMatch_4_horizontal(int i, int j)
    {
        if (j >= 3 && m_board[i, j - 3] != null && m_board[i, j - 2] != null && m_board[i, j - 1] != null && m_board[i, j] != null)
        {
            if (m_board[i, j - 3].GetComponent<Ingredient>().m_ingredient == m_board[i, j - 2].GetComponent<Ingredient>().m_ingredient
                && m_board[i, j - 2].GetComponent<Ingredient>().m_ingredient == m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient
                && m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient == m_board[i, j].GetComponent<Ingredient>().m_ingredient)
            {
                m_board[i, j - 3].GetComponent<Ingredient>().SetPointsHorizontalMatch(20);
                m_board[i, j - 3].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j - 2].GetComponent<Ingredient>().SetPointsHorizontalMatch(20);
                m_board[i, j - 2].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j - 1].GetComponent<Ingredient>().SetPointsHorizontalMatch(20);
                m_board[i, j - 1].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j].GetComponent<Ingredient>().SetPointsHorizontalMatch(20);
                m_board[i, j].GetComponent<Ingredient>().name = "destroy";
            }
        }
    }

    private void CheckMatch_3_horizontal(int i, int j)
    {
        if (j >= 2 && m_board[i, j - 2] != null && m_board[i, j - 1] != null && m_board[i, j] != null)
        {
            if (m_board[i, j - 2].GetComponent<Ingredient>().m_ingredient == m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient 
                && m_board[i, j - 1].GetComponent<Ingredient>().m_ingredient == m_board[i, j].GetComponent<Ingredient>().m_ingredient)
            {
                m_board[i, j - 2].GetComponent<Ingredient>().SetPointsHorizontalMatch(15);
                m_board[i, j - 2].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j - 1].GetComponent<Ingredient>().SetPointsHorizontalMatch(15);
                m_board[i, j - 1].GetComponent<Ingredient>().name = "destroy";
                m_board[i, j].GetComponent<Ingredient>().SetPointsHorizontalMatch(15);
                m_board[i, j].GetComponent<Ingredient>().name = "destroy";
            }
        }
    }

    private void Update()
    {
        if (CheckIfIngredientsAreMouvingDown() == false)
        {
            ReconstructBoard();

            CheckMatches();

            PopulateUpperRow();

            if (DetectIngredientsToMoveDown() && CheckIfIngredientsAreMouving() == false)
            {
                MoveDown();
            }




            if (Input.touchCount > 0 && CheckIfIngredientsAreMouving() == false)
            {
                Touch touch = Input.touches[0];

                if (touch.phase == TouchPhase.Began)
                {
                    m_touchMovement = Vector2.zero;
                    i = GetY(touch);
                    j = GetX(touch);
                    m_isMouving = true;
                }
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    m_touchMovement += touch.deltaPosition;

                    if (m_touchMovement.magnitude > m_minSwipeDistance && m_isMouving)
                    {
                        string tmp = GetDirection(m_touchMovement);
                        
                        if (tmp == "up" && m_board[i, j].GetComponent<Ingredient>().i < m_height - 1)
                        {
                            m_board[i, j].GetComponent<Ingredient>().m_direction = tmp;
                            m_board[i, j].GetComponent<Ingredient>().m_isMoving = true;
                            
                            m_board[i + 1, j].GetComponent<Ingredient>().m_direction = "down";
                            m_board[i + 1, j].GetComponent<Ingredient>().m_isMoving = true;
                        }
                        else if (tmp == "down" && m_board[i, j].GetComponent<Ingredient>().i > 0)
                        {
                            m_board[i, j].GetComponent<Ingredient>().m_direction = tmp;
                            m_board[i, j].GetComponent<Ingredient>().m_isMoving = true;

                            m_board[i - 1, j].GetComponent<Ingredient>().m_direction = "up";
                            m_board[i - 1, j].GetComponent<Ingredient>().m_isMoving = true;
                        }
                        else if (tmp == "left" && m_board[i, j].GetComponent<Ingredient>().j > 0)
                        {
                            m_board[i, j].GetComponent<Ingredient>().m_direction = tmp;
                            m_board[i, j].GetComponent<Ingredient>().m_isMoving = true;

                            m_board[i, j - 1].GetComponent<Ingredient>().m_direction = "right";
                            m_board[i, j - 1].GetComponent<Ingredient>().m_isMoving = true;
                        }
                        else if (tmp == "right" && m_board[i, j].GetComponent<Ingredient>().j < m_width - 1)
                        {
                            m_board[i, j].GetComponent<Ingredient>().m_direction = tmp;
                            m_board[i, j].GetComponent<Ingredient>().m_isMoving = true;

                            m_board[i, j + 1].GetComponent<Ingredient>().m_direction = "left";
                            m_board[i, j + 1].GetComponent<Ingredient>().m_isMoving = true;
                        }

                        m_isMouving = false;

                        m_moves--;
                        m_movesLeftText.GetComponent<Text>().text = "Moves left:\n" + m_moves.ToString();
                    }
                }
            }

            if (m_moves == 0 && CheckIfIngredientsAreMouvingDown() == false && CheckIfIngredientsAreMouving() == false)
            {
                m_gameOverPanelRef.gameObject.SetActive(true);
                m_scoreMsgRef.GetComponent<Text>().text = "Score:\n" + m_score.ToString();
                m_movesPanelRef.gameObject.SetActive(false);
                m_scorePanelRef.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}