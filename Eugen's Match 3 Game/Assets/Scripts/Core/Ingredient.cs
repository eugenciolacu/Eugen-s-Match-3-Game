using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    [Header ("Ingredients as string")]
    public string[] m_ingredients;

    [Header ("Sprites in the same order as in ingredient array")]
    public Sprite[] m_ingredientSpritesRefs;

    [Range(0.1f, 50f)]
    public float m_speed;

    public string m_ingredient;

    public bool m_isMovingDown = false;

    public int i;
    public int j;

    public bool m_isMoving = false;

    public int pointsHorizontalMatch = 0;

    public int pointsVerticalMatch = 0;

    [Header("One of the strings : none, left, right, up, down")]
    public string m_direction;

    
    // Use this for initialization
    void Awake () {
        InstantiateIngredient();
    }

    void InstantiateIngredient()
    {
        int index = UnityEngine.Random.Range(0, m_ingredientSpritesRefs.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = m_ingredientSpritesRefs[index];
        m_ingredient = m_ingredients[index];
        gameObject.name = "clone";
        m_direction = "none";
    }

    public void SetPointsHorizontalMatch (int tmp)
    {
        if (tmp > pointsHorizontalMatch)
        {
            pointsHorizontalMatch = tmp;
        }
    }

    public void SetPointsVerticalMatch (int tmp)
    {
        if (tmp > pointsVerticalMatch)
        {
            pointsVerticalMatch = tmp;
        }
    }

    // Update is called once per frame
    void Update () {
        if (m_isMovingDown)
        {
            float tmpY = m_speed * Time.deltaTime;
            transform.Translate(0f, -tmpY, 0f);

            if (transform.position.y <= i - 1)
            {
                transform.position = new Vector3(transform.position.x, i - 1, transform.position.z);
                m_isMovingDown = false;
                i--;
            }
        }
        else if (m_isMoving && m_direction != "none")
        {
            if (m_direction == "down")
            {
                if (i >= 1)
                {
                    float tmp = m_speed / 4 * Time.deltaTime;
                    transform.Translate(0f, -tmp, 0f);

                    if (transform.position.y <= i - 1)
                    {
                        transform.position = new Vector3(transform.position.x, i - 1, transform.position.z);
                        m_isMoving = false;
                        m_direction = "none";
                        i--;
                    }
                }
            }
            else if (m_direction == "up")
            {
                if (i <= 8)
                {
                    float tmp = m_speed / 4 * Time.deltaTime;
                    transform.Translate(0f, tmp, 0f);

                    if (transform.position.y >= i + 1)
                    {
                        transform.position = new Vector3(transform.position.x, i + 1, transform.position.z);
                        m_isMoving = false;
                        m_direction = "none";
                        i++;
                    }
                }
            }
            else if (m_direction == "left")
            {
                if (j >= 0)
                {
                    float tmp = m_speed / 4 * Time.deltaTime;
                    transform.Translate(-tmp, 0f, 0f);

                    if (transform.position.x <= j - 1)
                    {
                        transform.position = new Vector3(j - 1, transform.position.y, transform.position.z);
                        m_isMoving = false;
                        m_direction = "none";
                        j--;
                    }
                }
            } 
            else if (m_direction == "right")
            {
                if (j <= 8)
                {
                    float tmp = m_speed / 4 * Time.deltaTime;
                    transform.Translate(tmp, 0f, 0f);

                    if (transform.position.x >= j + 1)
                    {
                        transform.position = new Vector3(j + 1, transform.position.y, transform.position.z);
                        m_isMoving = false;
                        m_direction = "none";
                        j++;
                    }
                }
            }   
        }
	}

    public void SetIJ(int i, int j)
    {
        this.i = i;
        this.j = j;
    }
}
