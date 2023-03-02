using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BaseSound : MonoBehaviour
{

    AsyncOperationHandle sound;
    //BGM
    // Inspector�Őݒ�
    public AudioSource audioSourceBGM;
    public List<AudioClip> audioClipsBGM;
    //SE
    public AudioSource audioSourceSE;
    public AudioClip[] audioClipsSE;

    public void LoadMusic(string songName)
    {
        sound = Addressables.LoadAssetAsync<AudioClip>($"Assets/Resource/Musics/{songName}.wav");
        audioSourceBGM.clip = (AudioClip)sound.Result;
        //audioClipsBGM.Add((AudioClip)sound.Result);
    }

    /// <summary>
    /// BGM�̍Đ�
    /// </summary>
    public void PlayBGM(int num)
    {
        // �񋓌^���痬������BGM��I�ԁiint�ŃL���X�g�j
        //audioSourceBGM.clip = audioClipsBGM[num];
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

    private void Start()
    {
        LoadMusic("Guren");
    }

}
