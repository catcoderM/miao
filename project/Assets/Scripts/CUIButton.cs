using UnityEngine;
using System.Collections;

public class CUIButton : MonoBehaviour {


    protected enum StateID
    { 
        NORMAL=0,
        FOCUS,
        ACTIVE,
    }

    protected StateID m_state = StateID.NORMAL;
    public Texture[] m_ButtonSkin;
    public int m_ID = 0;
    protected bool m_isOnActiv = false;
    public float m_scale = 1.0f;
    Vector2 m_screenPoint;
    public GUITexture m_texture;

    void Awake()
    {
        m_texture = this.guiTexture;
        m_screenPoint = new Vector2(m_texture.pixelInset.x,m_texture.pixelInset.y);

        SetState(StateID.NORMAL);
    }

    public int UpdateState(bool mouse,Vector3 mousepos)
    {
        int result = -1;

        if (m_texture.HitTest(mousepos))
        {
            if (mouse)
            {
                SetState(StateID.ACTIVE);
                return m_ID;
            }
            else
            {
                SetState(StateID.FOCUS);
            }
        }
        else
        {
            if (m_isOnActiv)
            {
                SetState(StateID.ACTIVE);
            }
            else
            {
                SetState(StateID.NORMAL);
            }
        }
        return result;
    }
    protected virtual void SetState(StateID stateID)
    {
        if (m_state == stateID)
            return;

        m_state = stateID;
        m_texture.texture= m_ButtonSkin[(int)m_state];
        float w = m_ButtonSkin[(int)m_state].width * m_scale;
        float h = m_ButtonSkin[(int)m_state].height * m_scale;

        m_texture.pixelInset = new Rect(this.m_screenPoint.x,m_screenPoint.y,w,h);
    }

    public virtual void SetScale(float scale)
    {
        m_scale = scale;
        float w = m_ButtonSkin[0].width * scale;
        float h = m_ButtonSkin[0].height * scale;

        m_screenPoint.x *= scale;
        m_screenPoint.y *= scale;

        m_texture.pixelInset = new Rect(m_screenPoint.x,m_screenPoint.y,w,h);
    }

    public virtual void SetOnActive(bool isactiv)
    {
        if (isactiv)
        {
            SetState(StateID.ACTIVE);
        }
        else if (m_isOnActiv)
        {
            SetState(StateID.NORMAL);
        }

        m_isOnActiv = isactiv;
    }
}
