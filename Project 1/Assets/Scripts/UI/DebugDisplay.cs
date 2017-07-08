﻿using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    public float fpsUpdateInterval = 0.5f;

    private int m_framesOverInterval = 0;
    private float m_fpsOverInterval = 0f;
    private float m_timeLeftBeforeUpdate;
    private float m_fps;

    private Text m_text;
    
    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    private void Start()
    {
        m_timeLeftBeforeUpdate = fpsUpdateInterval;
    }

    private void LateUpdate()
    {
        // update fps
        m_fpsOverInterval += Time.timeScale / Time.deltaTime;
        m_framesOverInterval++;

        m_timeLeftBeforeUpdate -= Time.deltaTime;
        if (m_timeLeftBeforeUpdate <= 0.0f)
        {
            m_fps = (m_fpsOverInterval / m_framesOverInterval);
            m_timeLeftBeforeUpdate = fpsUpdateInterval;
            m_fpsOverInterval = 0.0f;
            m_framesOverInterval = 0;
        }

        m_text.enabled = SettingManager.Instance.ShowFPS;

        // set display information
        if (m_text.enabled)
        {
            string text = "";

            text += "fps: " + m_fps + "\n";

            m_text.text = text;
        }
    }
}
