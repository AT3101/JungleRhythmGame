using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSound : MonoBehaviour
{
    //BGM
    [SerializeField] AudioSource audioSourceBGM;
    [SerializeField] AudioClip[] audioClipsBGM;

    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;

    /// <summary>
    /// BGM�̍Đ�
    /// </summary>
    public void PlayBGM(int num)
    {
        // �񋓌^���痬������BGM��I�ԁiint�ŃL���X�g�j
        audioSourceBGM.clip = audioClipsBGM[(int)num];
        audioSourceBGM.Play();

    }

    public void StopBgm()
    {
        audioSourceBGM.Stop();
    }

    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(int num)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[num]);
    }



    public void LoadMusic(string path)
    {

    }
}
