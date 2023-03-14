using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ScrollView�Ɏ��ۂɕ\��������f�[�^��n�����肷��B
/// �Q�[�����Ƃ̐ܒ���
/// </summary>
public class SelectViewer : BaseSound
{
    // �X�N���[���p�l��
    [SerializeField] private ScrollView scrollview;

    private void Awake()
    {
        LoadBGM("", true);
    }

    /// <summary>
    /// �J�n����
    /// </summary>
    private void Start()
    {
        StartCoroutine(StartScrollObj());
    }

    private void Update()
    {
        //Debug.Log("bgmClip.Count" + BgmClipCount());
    }

    IEnumerator StartScrollObj()
    {
        yield return new WaitForSeconds(2f);
        var items = Enumerable.Range(0, GetbgmClip().Count).
          Select(i => new MusicItemData(i, $""+ GetBgmClipName(i))).ToArray();
        scrollview.UpdateData(items);
    }
}
