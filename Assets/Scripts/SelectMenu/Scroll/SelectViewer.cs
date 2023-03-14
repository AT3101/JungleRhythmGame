using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ScrollView�Ɏ��ۂɕ\��������f�[�^��n�����肷��B
/// �Q�[�����Ƃ̐ܒ���
/// </summary>
public class SelectViewer : MonoBehaviour
{
    // �X�N���[���p�l��
    [SerializeField] private ScrollView scrollview;

    /// <summary>
    /// �J�n����
    /// </summary>
    private void Start()
    {
        var items = Enumerable.Range(0, 10).
            Select(i => new MusicItemData(i, $"{i:D2}�Ȗ�")).ToArray();

        scrollview.UpdateData(items);
    }
}
