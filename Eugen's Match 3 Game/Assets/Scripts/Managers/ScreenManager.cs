using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour {

    public RectTransform m_backgroundRef;

    public RectTransform m_backgroundEffectRef;

    public RectTransform m_scorePanelRef;

    public RectTransform m_movesPanelRef;

    public RectTransform m_GameOverPanelRef;

    public RectTransform m_exitRef;

    public RectTransform m_restartRef;

    public Camera m_cameraRef;

    public int m_screenWidth;
    public int m_screenHeight;
    public bool m_isPortrait;
    public float m_pixelsInUnit;
    public float m_screenRatio;
    public const float m_GAME_WORLD_SCREEN_RATIO_PORTRAIT = 10f / 15f;
    public const float m_GAME_WORLD_SCREEN_RATIO_LANDSCAPE = 15f / 10f;

    private Camera m_cameraComponentRef;

    private void Start()
    {
        m_cameraComponentRef = m_cameraRef.GetComponent<Camera>();
        m_GameOverPanelRef.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        GetScreenParameters();

        if (m_isPortrait)
        {
            SetScreenForPortraitOrientation();
        }
        else
        {
            SetScreenForLandscapeOrientation();
        }
    }

    void SetScreenForPortraitOrientation()
    {
        SetCameraSize();
        m_cameraComponentRef.transform.position = new Vector3(4f, 1.75f, -10f);

        m_scorePanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4f * m_pixelsInUnit);
        m_scorePanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4f * m_pixelsInUnit);
        m_movesPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4f * m_pixelsInUnit);
        m_movesPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4f * m_pixelsInUnit);

        float tmp_y = (m_cameraComponentRef.orthographicSize - m_cameraComponentRef.transform.position.y - 3f) * m_pixelsInUnit;
        float tmp_x = m_screenWidth / 2f + 2.5f * m_pixelsInUnit;
        m_scorePanelRef.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(tmp_x, tmp_y, 0f), Quaternion.identity);

        float tmp_y1 = tmp_y;
        float tmp_x1 = m_screenWidth / 2f - 2.5f * m_pixelsInUnit;
        m_movesPanelRef.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(tmp_x1, tmp_y1, 0f), Quaternion.identity);

        m_GameOverPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4f * m_pixelsInUnit);
        m_GameOverPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4f * m_pixelsInUnit);
        m_GameOverPanelRef.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(m_screenWidth / 2, m_screenHeight / 2, 0f), Quaternion.identity);

        m_exitRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.9f * m_pixelsInUnit);
        m_exitRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.9f * m_pixelsInUnit);

        m_restartRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.9f * m_pixelsInUnit);
        m_restartRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.9f * m_pixelsInUnit);
    }

    void SetScreenForLandscapeOrientation()
    {
        SetCameraSize();
        m_cameraComponentRef.transform.position = new Vector3(1.75f, 4f, -10f);

        m_scorePanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4f * m_pixelsInUnit);
        m_scorePanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4f * m_pixelsInUnit);
        m_movesPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4f * m_pixelsInUnit);
        m_movesPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4f * m_pixelsInUnit);

        float tmp_y = (m_cameraComponentRef.orthographicSize - 2.5f) * m_pixelsInUnit;
        float tmp_x = m_screenWidth / 2f - 4.75f * m_pixelsInUnit;
        m_scorePanelRef.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(tmp_x, tmp_y, 0f), Quaternion.identity);

        float tmp_y1 = (m_cameraComponentRef.orthographicSize + 2.5f) * m_pixelsInUnit;
        float tmp_x1 = tmp_x;
        m_movesPanelRef.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(tmp_x1, tmp_y1, 0f), Quaternion.identity);

        m_GameOverPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4f * m_pixelsInUnit);
        m_GameOverPanelRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4f * m_pixelsInUnit);
        m_GameOverPanelRef.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(m_screenWidth / 2, m_screenHeight / 2, 0f), Quaternion.identity);

        m_exitRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.9f * m_pixelsInUnit);
        m_exitRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.9f * m_pixelsInUnit);

        m_restartRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.9f * m_pixelsInUnit);
        m_restartRef.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.9f * m_pixelsInUnit);
    }

    void GetScreenParameters()
    {
        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;
        m_isPortrait = m_screenHeight > m_screenWidth ? true : false;
        m_pixelsInUnit = (float)m_screenHeight / (m_cameraRef.GetComponent<Camera>().orthographicSize * 2);
        m_screenRatio = (float)m_screenWidth / m_screenHeight;
    }

    void SetCameraSize()
    {
        if (m_isPortrait)
        {
            if (m_screenRatio < m_GAME_WORLD_SCREEN_RATIO_PORTRAIT)
            {
                m_cameraComponentRef.orthographicSize = ((10f * m_screenHeight) / m_screenWidth) / 2f;
            }
            else
            {
                float x = (15f * m_screenWidth) / m_screenHeight;
                m_cameraComponentRef.orthographicSize = ((x * m_screenHeight) / m_screenWidth) / 2f;
            }
        }
        else
        {
            if (m_screenRatio > m_GAME_WORLD_SCREEN_RATIO_LANDSCAPE)
            {
                float x = (10f * m_screenWidth) / m_screenHeight;
                m_cameraComponentRef.orthographicSize = ((x * m_screenHeight) / m_screenWidth) / 2f;
            }
            else
            {
                m_cameraComponentRef.orthographicSize = ((15f * m_screenHeight) / m_screenWidth) / 2f;
            }
        }

        
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

}
