using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager
{
    private static List<Move> m_moveList = new List<Move>();   //正在Update中做位移动画的List.
    /// <summary>
    /// 位移动画的数据.
    /// </summary>
    private class MoveData
    {
        public Vector3 begin;   //起始点.
        public Vector3 end;     //结束点.
        public bool pingPong;   //是否是PingPong动画.
        public float time;      //一次动画的总时长.
        public EaseType easeType;   //头尾渐入渐出类型.
    }
    /// <summary>
    /// 位移动画的类(这里采用链式编程,能提升扩展性(比如可以加入SetDelay等方法),并且调用起来非常清晰).
    /// </summary>
    public class Move
    {
        private GameObject m_target;    //目标GameObject.
        private float m_startTime = 0;  //动画开始的时间.
        private MoveData m_moveData;    //动画数据.
        private bool m_finished = false;        //是否已经结束了.
        private bool m_moveBack = false;        //是否是PingPong动画正在返回.
        /// <summary>
        /// 初始化动画.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Move Prepare(GameObject obj)
        {
            m_startTime = Time.time;
            m_target = obj;
            m_moveData = new MoveData();
            return this;
        }

        /// <summary>
        /// 从begin位置移动到end位置.
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Move FromTo(Vector3 begin, Vector3 end, float time)
        {
            m_moveData.begin = begin;
            m_moveData.end = end;
            m_moveData.time = time;
            return this;
        }

        /// <summary>
        /// 设定使用渐入渐出类型.
        /// </summary>
        /// <param name="easeType"></param>
        /// <returns></returns>
        public Move SetEaseType(EaseType easeType)
        {
            m_moveData.easeType = easeType;
            return this;
        }

        /// <summary>
        /// 设定动画类型为PingPong.
        /// </summary>
        /// <returns></returns>
        public Move SetPingPong(bool pingPong)
        {
            m_moveData.pingPong = pingPong;
            return this;
        }

        /// <summary>
        /// 播放动画.
        /// </summary>
        public void Play()
        {
            m_moveList.Add(this);
        }

        public void OnUpdate()
        {
            //当前动画播放的进度百分比.
            float percent = (Time.time - m_startTime) / m_moveData.time;
            if (m_moveBack)
            {
                percent = 1 - percent;
                if (percent <= 0)
                {
                    //回归动画播完了.
                    m_moveBack = false;
                    //重置开始时间.
                    m_startTime = Time.time;
                }
            }
            if (percent >= 1)
            {
                if (m_moveData.pingPong)
                {
                    //要播回归动画了.
                    m_moveBack = true;
                    //重置回归动画的开始时间.
                    m_startTime = Time.time;
                }
                else
                {
                    //不是pingpong动画,到这里就结束了.
                    m_finished = true;
                }
            }

            //在Update里插值.
            m_target.transform.localPosition =
                new Vector3(GetEaseValue(m_moveData.easeType, m_moveData.begin.x, m_moveData.end.x, percent)
                , GetEaseValue(m_moveData.easeType, m_moveData.begin.y, m_moveData.end.y, percent)
                , GetEaseValue(m_moveData.easeType, m_moveData.begin.z, m_moveData.end.z, percent));
        }

        public bool IsFinished()
        {
            return m_finished;
        }

        /// <summary>
        /// 获取不同EaseType下的瞬时值.
        /// </summary>
        public float GetEaseValue(EaseType easeType, float begin, float end, float percent)
        {
            //这里在工作中的话会用AnimationCurve来编辑几条Curve来简单实现.
            switch (easeType)
            {
                case EaseType.None:
                    return Mathf.Lerp(begin, end, percent);
                //这是一条沿Y轴呈轴对称且a>0的抛物线,表达式"y = x^2".
                case EaseType.EaseIn:
                    return Mathf.Lerp(begin, end, Mathf.Pow(percent, 2));
                //这是一条沿 y = 1轴呈轴对称,且a<0的抛物线,表达式"y = -x^2 + 2x".
                case EaseType.EaseOut:
                    return Mathf.Lerp(begin, end, -Mathf.Pow(percent, 2) + (2 * percent));
                //这是一条cos函数曲线,表达式"y= (-cos(x * PI) + 1) / 2".
                case EaseType.EaseInOut:
                    return Mathf.Lerp(begin, end, (-Mathf.Cos(percent * Mathf.PI) + 1) / 2);
            }
            return 0;
        }
    }
    /// <summary>
    /// 传入一个物体.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Move CreateMoveCommand(GameObject obj)
    {
        return new Move().Prepare(obj);
    }
    public static void Update()
    {
        for (int i = 0; i < m_moveList.Count; i++)
        {
            if (m_moveList[i] == null)
            {
                continue;
            }
            m_moveList[i].OnUpdate();
            if (m_moveList[i].IsFinished())
            {
                m_moveList.RemoveAt(i);
                i--;
            }
        }
    }
    public static void StopAll()
    {
        m_moveList.Clear();
    }
}
/// <summary>
/// 渐入渐出枚举.
/// </summary>
public enum EaseType
{
    None,       //无渐入渐出效果.
    EaseIn,     //缓慢渐入.
    EaseOut,    //缓慢渐出.
    EaseInOut,  //缓慢渐入+渐出.
}