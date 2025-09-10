using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavPathRenderer : MonoSingleton<NavPathRenderer>
{
    LineRenderer pathRenderer;
    NavMeshPath path;

    void Start()
    {
        pathRenderer = GetComponent<LineRenderer>();
        pathRenderer.enabled = false;
    }

    public void SetPath(NavMeshPath path, Vector3 target)
    {
        this.path = path;
        if (this.path == null)
        {
            //���û��·����������Ⱦ��
            pathRenderer.enabled = false;
            pathRenderer.positionCount = 0;
        }
        else
        {
            pathRenderer.enabled = true;//������Ⱦ��
            pathRenderer.positionCount = path.corners.Length + 1;//����path·���Ĺյ㣬����renderer�Ĺյ㣨+1����Ϊ�յ㲻������
            pathRenderer.SetPositions(path.corners);//����rendererÿһ���Ĺյ�λ��
            pathRenderer.SetPosition(pathRenderer.positionCount - 1, target);//����renderer�����һ����ΪĿ���
            for (int i = 0; i < pathRenderer.positionCount; i++)
            {
                //��ÿ����ĸ߶�����Ϊ0.2f��������Ⱦ�ڵ�����
                pathRenderer.SetPosition(i, pathRenderer.GetPosition(i) + Vector3.up * 0.5f);
            }
        }
    }
}
