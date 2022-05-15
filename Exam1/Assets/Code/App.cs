using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    public Vector3 from;
    public Vector3 to;
    public bool pingPong;
    public EaseType easeType;
    public float time;
    private GameObject m_obj;
    
    void Start()
    {
        m_obj = GameObject.Find("MoveObj");
        Button testBtn = GameObject.Find("TestBtn").transform.GetComponent<Button>();
        testBtn.onClick.AddListener(() =>
        {
            //调用示例.
            MoveManager.CreateMoveCommand(m_obj)
            .FromTo(from, to, time)
            .SetPingPong(pingPong)
            .SetEaseType(easeType)
            .Play();
        });
        Button stopBtn = GameObject.Find("StopBtn").transform.GetComponent<Button>();
        stopBtn.onClick.AddListener(() =>
        {
            MoveManager.StopAll();
        });

    }

    void Update()
    {
        MoveManager.Update();
    }
}
