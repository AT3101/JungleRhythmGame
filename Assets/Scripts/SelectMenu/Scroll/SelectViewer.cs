using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using LitJson;

/// <summary>
/// ScrollView�Ɏ��ۂɕ\��������f�[�^��n�����肷��B
/// �Q�[�����Ƃ̐ܒ���
/// </summary>
public class SelectViewer : BaseSound
{
    public List<string> jsonKey = new List<string>();

    // �X�N���[���p�l��
    [SerializeField] private ScrollView scrollview;

    [SerializeField] SoundSelect soundSelect;

    private void Awake()
    {
        Load();
    }
    /// <summary>
    /// json���[�h
    /// </summary>
    private void Load()
    {
        AsyncOperationHandle json;

        // json��addressable�Ŏ擾
        json = Addressables.LoadAssetAsync<TextAsset>($"Assets/Resource/CutMusic/NameList.json");

        // ����������
        var scoreLoad = json.WaitForCompletion();

        TextAsset score = json.Result as TextAsset;
        JsonData jsonData = JsonMapper.ToObject(score.ToString());

        Addressables.Release(json);

        for (int i = 0; i < jsonData.Count; i++)
        {
            jsonKey.Add(jsonData[i].ToString());
            Debug.Log(jsonKey[i]);
        }
    }

    /// <summary>
    /// �J�n����
    /// </summary>
    private void Start()
    {
        StartCoroutine(StartScrollObj());
        LoadBGM("",true);
    }

    private void Update()
    {
        //Debug.Log("bgmClip.Count" + BgmClipCount());
    }

    IEnumerator StartScrollObj()
    {
        yield return new WaitForSeconds(2f);
        var items = Enumerable.Range(0, jsonKey.Count).
          Select(i => new MusicItemData(jsonKey.Count, jsonKey[i])).ToArray();
        scrollview.UpdateData(items);
    }

   public void PlayBgm()
    {
        soundSelect.PlayBGM(0);
    }
}
