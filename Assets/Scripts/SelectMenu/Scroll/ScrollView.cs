using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;

// �X�N���[���r���E�ŗ��p����f�[�^�^���錾
class MusicItemData
{
    // id
    public int Id;
    // �Ȗ�
    public string musicName;

    // �R���X�g���N�^
    public MusicItemData(int id ,string musicName)
    {
        this.Id = id;
        this.musicName = musicName;
    }
}

class ScrollView : FancyScrollView<MusicItemData>
{
    [SerializeField] private Scroller _scroller;
    [SerializeField] private GameObject _cellPrefab;

    protected override GameObject CellPrefab => _cellPrefab;

    protected override void Initialize()
    {
        _scroller.OnValueChanged(UpdatePosition);
    }

    public void UpdateData(IList<MusicItemData> music)
    {
        UpdateContents(music)
;       _scroller.SetTotalCount(music.Count);
    }
}
