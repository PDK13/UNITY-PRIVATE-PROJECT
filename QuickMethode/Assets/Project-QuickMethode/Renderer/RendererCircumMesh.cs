using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RendererCircumMesh : MonoBehaviour
{
    [SerializeField][Range(3, 60)] private int m_outerPoint = 3;
    float[] m_outerPointRatio = new float[3];
    //
    [SerializeField][Min(0)] private float m_outerRadius = 2f;
    [SerializeField] private float m_outerDeg = 0f;
    //
    private RendererCircumMeshData m_data;

    public int OuterPoint
    {
        get => m_outerPoint;
        set
        {
            if (value < 3)
                value = 3;
            //
            if (value > 60)
                value = 60;
            //
            m_outerPoint = value;
            m_outerPointRatio = new float[value];
        }
    }

    public float[] OuterPointRatio
    {
        get => m_outerPointRatio;
        set
        {
            if (value.Length < 3)
                return;
            //
            for (int i = 0; i < value.Length; i++) if (value[i] < 0 && value[i] < -OuterRadius) value[i] = -OuterRadius;
            //
            m_outerPoint = value.Length;
            m_outerPointRatio = value;
        }
    }

    public float OuterRadius
    {
        get => m_outerRadius;
        set => m_outerRadius = value >= 0 ? value : m_outerRadius;
    }

    public float OuterDeg
    {
        get => m_outerDeg;
        set => m_outerDeg = value;
    }

    public RendererCircumMeshData Data => m_data;

    public void SetGenerate()
    {
        m_data = new RendererCircumMeshData(OuterPointRatio, OuterRadius, OuterDeg);
    }

    #region Sample

    public void SetSampleStarA()
    {
        OuterPoint = 10;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 2 != 0)
                continue;
            //
            OuterPointRatio[i] = OuterRadius * 0.6f * -1;
        }
        //
        OuterDeg = -18f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    public void SetSampleStarB()
    {
        OuterPoint = 12;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 4 == 0)
                OuterPointRatio[i] = OuterRadius * 0.3f * -1;
            else
            if (i % 2 != 0)
                OuterPointRatio[i] = OuterRadius * 0.6f * -1;
        }
        //
        OuterDeg = 30f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    //

    public void SetSampleStarC()
    {
        OuterPoint = 15;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 != 0)
                continue;
            //
            OuterPointRatio[i] = OuterRadius * 0.5f * -1;
        }
        //
        OuterDeg = -18f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    public void SetSampleStarD()
    {
        OuterPoint = 18;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.4f * -1;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.1f * -1;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }
        }
        //
        OuterDeg = 0f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    //

    public void SetSampleStarE()
    {
        OuterPoint = 24;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.4f * -1;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.2f * -1;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }
        }
        //
        OuterDeg = 22.5f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    public void SetSampleStarF()
    {
        OuterPoint = 36;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.6f * -1;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.3f * -1;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }

        }
        //
        OuterDeg = 15f;
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
    }

    #endregion

    #region Editor

    public void SetEditorPointsRatioChange()
    {
        if (m_outerPointRatio.Length < 3)
        {
            m_outerPoint = 3;
            m_outerPointRatio = new float[3];
        }
        else
        if (m_outerPointRatio.Length != m_outerPoint)
        {
            m_outerPointRatio = new float[m_outerPoint];
        }
    }

    private void OnDrawGizmosSelected()
    {
        QGizmos.SetWireSphere(this.transform.position, OuterRadius, Color.gray);
        //
        SetGenerate();
        //
        Data.SetOuterGenerate(m_outerPointRatio, m_outerRadius, m_outerDeg);
        //
        Vector3 PointA, PointB;
        for (int i = 0; i < Data.Point - 1; i++)
        {
            PointA = this.transform.position + Data.Points[i];
            PointB = this.transform.position + Data.Points[i + 1];
            QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
        }
        PointA = this.transform.position + Data.Points[Data.Points.Length - 1];
        PointB = this.transform.position + Data.Points[0];
        QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
    }

    #endregion
}

public class RendererCircumMeshData
{
    public int Point { private set; get; } = 0;
    public float Radius { private set; get; } = 0;
    public float Deg { private set; get; } = 0;

    public Vector3[] Points { private set; get; } = new Vector3[0];
    public float[] PointsRatio { private set; get; } = new float[0];

    public RendererCircumMeshData()
    {
        //...
    }

    public RendererCircumMeshData(int Point, float Radius, float Deg)
    {
        SetOuterGenerate(Point, Radius, Deg);
    }

    public RendererCircumMeshData(float[] PointRatio, float Radius, float Deg)
    {
        SetOuterGenerate(PointRatio, Radius, Deg);
    }

    //

    public bool SetOuterGenerate(int Point, float Radius, float Deg)
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.Point = Point;
        this.PointsRatio = new float[0];
        this.Radius = Radius;
        this.Deg = Deg;
        //
        this.Points = Getm_outerPoint();
        //
        return true;
    }

    public bool SetOuterGenerate(float[] PointRatio, float Radius, float Deg)
    {
        if (PointRatio.Length < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.Point = PointRatio.Length;
        this.PointsRatio = PointRatio;
        this.Radius = Radius;
        this.Deg = Deg;
        //
        this.Points = Getm_outerPoint();
        this.Points = GetOuterPointRatio();
        //
        return true;
    }

    private Vector3[] Getm_outerPoint()
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return null;
        //
        List<Vector3> Points = new List<Vector3>();
        //
        float RadSpace = (360 / Point) * (Mathf.PI / 180);
        float RadStart = (Deg) * (Mathf.PI / 180);
        float RadCur = RadStart;
        //
        Vector3 PointStart = new Vector3(Mathf.Cos(RadStart) * Radius, Mathf.Sin(RadStart) * Radius, 0f);
        Points.Add(PointStart);
        for (int i = 1; i < Point; i++)
        {
            RadCur += RadSpace;
            Vector3 NewPoint = new Vector3(Mathf.Cos(RadCur) * Radius, Mathf.Sin(RadCur) * Radius, 0f);
            Points.Add(NewPoint);
        }
        //
        return Points.ToArray();
    }

    private Vector3[] GetOuterPointRatio()
    {
        Vector3[] Points = this.Points;
        for (int i = 0; i < PointsRatio.Length; i++)
            Points[i] = Points[i] + Points[i].normalized * PointsRatio[i];
        return Points;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RendererCircumMesh))]
public class RendererCircumMeshEditor : Editor
{
    private RendererCircumMesh m_target;

    private SerializedProperty m_outerPoint;
    private SerializedProperty m_outerRadius;
    private SerializedProperty m_outerDeg;

    private Vector2 m_scrollOuterPointRatio;

    private void OnEnable()
    {
        m_target = target as RendererCircumMesh;
        //
        m_outerPoint = QEditorCustom.GetField(this, "m_outerPoint");
        m_outerRadius = QEditorCustom.GetField(this, "m_outerRadius");
        m_outerDeg = QEditorCustom.GetField(this, "m_outerDeg");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditor.SetLabel("SETTING", QEditor.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        QEditorCustom.SetField(m_outerPoint);
        //
        if (m_target.OuterPointRatio.Length != m_target.OuterPoint)
            m_target.OuterPointRatio = new float[m_target.OuterPoint];
        //
        int i = 0;
        m_scrollOuterPointRatio = QEditor.SetScrollViewBegin(m_scrollOuterPointRatio, QEditor.GetGUIHeight(105));
        while (i < m_target.OuterPoint)
        {
            if (m_target.OuterPointRatio[i] < 0 && m_target.OuterPointRatio[i] < -m_target.OuterRadius)
                m_target.OuterPointRatio[i] = -m_target.OuterRadius;
            //
            //VIEW:
            QEditor.SetHorizontalBegin();
            QEditor.SetLabel(string.Format("{0}", i), QEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QEditor.GetGUIWidth(25));
            m_target.OuterPointRatio[i] = QEditor.SetField(m_target.OuterPointRatio[i], null, QEditor.GetGUIWidth(50));
            //
            if (i < m_target.Data.Points.Length)
                QEditor.SetLabel(((Vector2)m_target.Data.Points[i]).ToString(), QEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter));
            //
            QEditor.SetHorizontalEnd();
            //VIEW:
            //
            i++;
        }
        QEditor.SetScrollViewEnd();
        //
        QEditorCustom.SetField(m_outerRadius);
        QEditorCustom.SetField(m_outerDeg);
        //
        QEditor.SetSpace(10);
        //
        QEditor.SetLabel("SAMPLE", QEditor.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        QEditor.SetHorizontalBegin();
        if (QEditor.SetButton("Star A"))
            m_target.SetSampleStarA();
        if (QEditor.SetButton("Star B"))
            m_target.SetSampleStarB();
        QEditor.SetHorizontalEnd();
        //
        QEditor.SetHorizontalBegin();
        if (QEditor.SetButton("Star C"))
            m_target.SetSampleStarC();
        if (QEditor.SetButton("Star D"))
            m_target.SetSampleStarD();
        QEditor.SetHorizontalEnd();
        //
        QEditor.SetHorizontalBegin();
        if (QEditor.SetButton("Star E"))
            m_target.SetSampleStarE();
        if (QEditor.SetButton("Star F"))
            m_target.SetSampleStarF();
        QEditor.SetHorizontalEnd();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif