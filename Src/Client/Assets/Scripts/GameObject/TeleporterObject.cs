using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
    public int ID;//���͵�ID
    Mesh mesh = null;

    void Start()
    {
        mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }


    void Update()
    {

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (this.mesh != null)
        {
            //Gizmos.DrawMesh(this.mesh, this.transform.position + Vector3.up * this.transform.localScale.y * .5f, this.transform.rotation, this.transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position,this.transform.rotation, 1f, EventType.Repaint);
    }
#endif

    /// <summary>
    /// ������������ɫ���봥������
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        PlayerInputContorller playerInputContorller = other.GetComponent<PlayerInputContorller>();
        if (playerInputContorller != null && playerInputContorller.enabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td == null)
            {
                Debug.LogErrorFormat("TeleporterObject��Character[{0}] Enter Teleporter [{1}]��But TeleporterDefine not existed", playerInputContorller.character.Info.Name, this.ID);
                return;
            }
            Debug.LogFormat("TeleporterObject��Character[{0}] Enter Teleporter [{1}��{2}]", playerInputContorller.character.Info.Name, this.ID, td.Name);
            //LinkTo��Ŀ�괫�͵�ID
            if (td.LinkTo > 0)//�ж�Ŀ�괫�͵�ID�Ƿ����
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                    MapService.Instance.SendMapTeleport(this.ID);//����ͼ���ʹ�������
                else
                    Debug.LogErrorFormat("Teleporter ID��{0} LinkID {1} error", td.ID, td.LinkTo);
            }
        }
    }
}
