using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SelectUiAnimatioin : MonoBehaviour
{
    // �A�j���[�V����������e�L�X�g
    [SerializeField] TextMeshProUGUI gorillaDialogue = default;

    /// <summary>
    /// �J�n����
    /// </summary>
    void Start()
    {
        TextDOTweenAnim();
    }

    /// <summary>
    /// UI�A�j���[�V��������
    /// </summary>
    private void TextDOTweenAnim()
    {
        //DOTweenTMPAnimator���쐬
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(gorillaDialogue);

        //1�������A�j���[�V������ݒ�(i�����Ԗڂ̕������̃C���f�b�N�X)
        //Sequence�őS�����̃A�j���[�V�������܂Ƃ߂�
        var sequence = DOTween.Sequence();

        sequence.SetLoops(-1);//�������[�v�ݒ�


        //�ꕶ�����ɃA�j���[�V�����ݒ�
        var duration = 0.2f;//1��ӂ��Tween����
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            sequence.Join(DOTween.Sequence()
              //��Ɉړ����Ė߂�
              .Append(animator.DOOffsetChar(i, animator.GetCharOffset(i) + new Vector3(0, 10, 0), duration).SetEase(Ease.OutFlash, 2))
              //������1.2�{�Ɋg�債�Ė߂�
              .Join(animator.DOScaleChar(i, 1.2f, duration).SetEase(Ease.OutFlash, 2))
              //�����ɐF�����F�ɂ��Ė߂�
              .Join(animator.DOColorChar(i, Color.gray, duration * 0.5f).SetLoops(2, LoopType.Yoyo))
              //�A�j���[�V������A1�b�̃C���^�[�o���ݒ�
              .AppendInterval(2f)
              //�J�n��0.15�b�����炷
              .SetDelay(0.3f * i)
            );
        }
    }
}
