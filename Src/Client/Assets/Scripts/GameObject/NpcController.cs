using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public int npcId;//NPC ID

    SkinnedMeshRenderer renderer;//NPC ����Ⱦ��
    Animator animator;//NPC �Ķ���������
    //Color originalColor; // NPC ��ԭʼ��ɫ

    private bool inInteraction = false; // �Ƿ����ڽ�����

    NpcDefine npc;//NPC ����

    NpcQuestStatus questStatus;

    void Start()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();//��ȡNPC����Ⱦ��
        animator = GetComponent<Animator>();
        //originalColor = renderer.sharedMaterial.color; // ����NPC��ԭʼ��ɫ
        npc = NPCManager.Instance.GetNpcDefine(npcId);
        NPCManager.Instance.UpdateNpcPosition(npcId, this.transform.position);//����NPC��λ�õ�NPCManager��
        //this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }


    void OnQuestStatusChanged(Quest quest)
    {
        //������״̬�����仯ʱ��ˢ��NPC������״̬
        this.RefreshNpcStatus();
    }

    /// <summary>
    /// ˢ��NPC������״̬
    /// </summary>
    void RefreshNpcStatus()
    {
        //��QuestManager��ȡNPC������״̬
        questStatus = QuestManager.Instance.GetNpcQuestStatus(this.npcId);
        //����NPC����״̬��ʾ
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    private void OnDestroy()
    {
        //��NPC������ʱ��ȡ����������״̬�仯�¼�
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        //��UIWorldElementManager���Ƴ�NPC������״̬��ʾ
        if (UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    IEnumerator Actions()
    {
        while (true)
        {
            //���NPC���ڽ����У���ȴ�һ��ʱ����ټ���
            if (inInteraction)
                yield return new WaitForSeconds(2f);
            //���NPCû���ڽ����У���ִ���������
            else
                //����ȴ�5~10��ִ��һ������
                yield return new WaitForSeconds(Random.Range(5f, 10f));

            this.Relax();
        }
    }

    void Update()
    {

    }

    void Relax()
    {
        animator.SetTrigger("Relax");
    }

    /// <summary>
    /// NPC��������
    /// </summary>
    void Interactive()
    {
        // �ж��Ƿ��ڽ�����
        // ������ڽ����У�����ִ�н����߼�
        if (!inInteraction)
        {
            inInteraction = true; // ����Ϊ���ڽ���״̬
            StartCoroutine(DoInteractive());
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    IEnumerator DoInteractive()
    {
        //��ʼ����ʱ������NPC�������
        yield return FaceToPlayer();
        //������Һ󣬿�ʼ����
        //�ж����������͵�NPC
        //���ؽ���Ƿ�ɹ�
        //�ɹ���ִ�н�������
        if (NPCManager.Instance.Interactive(npc))
        {
            animator.SetTrigger("Talk");
        }
        //3���ڲ������ٴν���
        yield return new WaitForSeconds(3f);
        inInteraction = false; // ��������������״̬
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceToPlayer()
    {
        //Ŀ���λ�� ��ȥ NPC��λ�� �õ�һ����������
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(this.transform.position, User.Instance.CurrentCharacterObject.transform.position) > 2f)
        {
            //���NPCʱ������������2�ף���ʼ������NPCλ��
            User.Instance.CurrentCharacterObject.StartNav(this.transform.position);
        }
        //�����NPCʱ����ʼ����
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    /// <summary>
    /// ����NPC����
    /// </summary>
    /// <param name="highlight"></param>
    void Highlight(bool highlight)
    {
        //if (highlight)
        //{
        //    if (renderer.sharedMaterial.color != Color.white)
        //        renderer.sharedMaterial.color = Color.white;
        //}
        //else
        //{
        //    if(renderer.sharedMaterial.color != originalColor)
        //        renderer.sharedMaterial.color = originalColor; // �ָ�ԭʼ��ɫ
        //}
    }
}
