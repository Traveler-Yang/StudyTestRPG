using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideController : MonoBehaviour
{
    public Transform mountPoint;//��˵�
    public EntityContorller rider;//�����
    public Vector3 offset;//ƫ��
    public Animator anim;

    void Start()
    {
        //this.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.mountPoint == null || this.rider == null) return;//���û����˵��������ߣ��򷵻�

        //������������ߵ�pos
        this.rider.SetridePosition(mountPoint.position + this.mountPoint.TransformDirection(offset));
    }
    
    /// <summary>
    /// ���������
    /// </summary>
    /// <param name="rider"></param>
    public void SetRider(EntityContorller rider)
    {
        this.rider = rider;
    }

    /// <summary>
    /// ����״̬�����¼�
    /// </summary>
    /// <param name="entityEvent"></param>
    /// <param name="param"></param>
    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetBool("Idle", true);
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                anim.SetBool("Idle", false);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                anim.SetBool("Idle", false);
                break;
            case EntityEvent.Jump:
                //anim.SetTrigger("Jump");
                break;
        }
    }
}
