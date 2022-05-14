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
    GameObject obj;
    void Start()
    {
        obj = GameObject.Find("MoveObj");
        Button testBtn = GameObject.Find("TestBtn").transform.GetComponent<Button>();
        testBtn.onClick.AddListener(() =>
        {
            MoveManager.CreateMoveCommand(obj)
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

    // Update is called once per frame
    void Update()
    {
        MoveManager.Update();
    }
}
