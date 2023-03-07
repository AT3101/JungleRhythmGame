using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SelectManager : MonoBehaviour
{
    // �J�ڃ{�^��
    [SerializeField] private GameObject buttonPanel;

    // �{�^���i�[�ϐ�
    [SerializeField] Button yesbutton;
    [SerializeField] Button nobutton;
    [SerializeField] Button closebutton;

    [SerializeField] GameObject panel;

    /// <summary>
    /// �J�n����
    /// 1�񂵂��Ă΂Ȃ�����Awake
    /// </summary>
    private void Awake()
    {
        // �p�l���̏����X�P�[���̐ݒ�
        buttonPanel.transform.localScale = Vector3.zero;

        // �e�{�^���ɃA�N�V�������i�[
        nobutton.onClick.AddListener(OnClickCloseButton);
        closebutton.onClick.AddListener(OnClickCloseButton);
        yesbutton.onClick.AddListener(OnClickYesButton);
        // �p�l�����\��
        panel.SetActive(false);
    }

    /// <summary>
    /// �������A��������X�{�^�������������̏���
    /// </summary>
    private void OnClickCloseButton()
    {
        panel.SetActive(false);
        // �I�v�V�����E�B���h�E�����񂾂񏬂���
        buttonPanel.transform.DOScale(Vector3.zero, 0.2f);

        // �X�P�[����0�ɂȂ������\���ɂ���
        if(buttonPanel.transform.localScale== Vector3.zero)
        {
            buttonPanel.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �Ȃ�I�������Ƃ�
    /// TODO:�ʏ����ɂ���\��
    /// </summary>
    public void OnClickSelectMusic()
    {
        panel.SetActive(true);

        // �{�^���p�l����\��
        buttonPanel.gameObject.SetActive(true);

        // �I�v�V�����E�B���h�E�����񂾂�g��
        buttonPanel.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
    }

    /// <summary>
    /// �u�͂��v�̃{�^�����������Ƃ�
    /// </summary>
    private void OnClickYesButton()
    {
        // �t�F�[�h����
        // �����ɏ���

        SceneManager.LoadScene("MainScene");

    }

}
