using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyBoxManager : MonoBehaviour
{
    // �J�����̉�]�X�s�[�h
    [Tooltip("�J�����̉�]�X�s�[�h")]
    [Range(0.01f, 1f)]
    [SerializeField] float rotateSpeed = 0.01f;

    // �X�J�C�{�b�N�X�}�e���A��
    [SerializeField] Material sky;

    // skybox�̃u�����h�l
    float alphaValue = 0f;

    // �J����
    private Camera cam;

    // �J�����̉�]
    private Vector3 newAngle = new Vector3(0f, 0f, 0f);


    private void Start()
    {
        // ���C���J�����̎擾
        cam = Camera.main;
        
        // �u�����h�̊����̏����l�ݒ�
        alphaValue = 0f;
        sky.SetFloat("_value", alphaValue);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        // �t�F�[�h���l��1�bskybox�̍X�V������҂�
        Invoke("ChangeSkyBox", 1);
        
        // �J�����̉�]����
        CamRotate();
    }

    /// <summary>
    /// �J�����̉�]����
    /// </summary>
    void CamRotate()
    {
        // �}�E�X�̈ړ��ʕ��J��������]������.
        newAngle.y += rotateSpeed;

        cam.gameObject.transform.localEulerAngles = newAngle;
    }

    /// <summary>
    /// skybox�̕ύX����
    /// </summary>
    private void ChangeSkyBox()
    {
        if (SceneManager.GetActiveScene().name == "ResultScene")
        {
            sky.SetFloat("_value", alphaValue);

            if (alphaValue <= 1)
            {
                alphaValue += 0.005f;
            }
        }
    }
}
