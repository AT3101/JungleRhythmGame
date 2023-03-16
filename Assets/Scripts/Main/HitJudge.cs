using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;

public class HitJudge : MonoBehaviour
{


    [SerializeField] MainManager mainManager;
    [SerializeField] GameObject[] JudgeMsgObj;
    [SerializeField] NotesManager notesManager;
    [SerializeField] SoundMain    soundMain;

    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject hitEffect;


    [SerializeField] float PerfectSecond= 0.10f;
    [SerializeField] float GreatSecond  = 0.15f;
    [SerializeField] float BadSecond    = 0.20f;
    [SerializeField] float MissSecond   = 0.20f;

    enum PusingKey
    {
        D,F,J,K,
    }

    bool[] touchKeyState = new bool[4] { false, false, false, false };
    bool[] pushingKeyState = new bool[4] { false, false, false, false };

    List<NoteData> longNoteDataList = new List<NoteData>();

    // Update is called once per frame
    void Update()
    {

        if (mainManager.isStart && !mainManager.isEnd)
        {

            UpdatePushingKeyState();


            // �ʏ�m�[�c����
            for(int laneNum = 0; laneNum < pushingKeyState.Length; laneNum++)
            {
                if (touchKeyState[laneNum])
                {
                    for (int noteTiming = 0; noteTiming < pushingKeyState.Length; noteTiming++)
                    {
                        if (notesManager.NoteDataAll.Count - 1 < noteTiming)
                        {
                            break;
                        }
                        if (laneNum == notesManager.NoteDataAll[noteTiming].laneNum)
                        {
                            if (notesManager.NoteDataAll[noteTiming].type != 2)
                            {
                                CheckHitTiming(Mathf.Abs(Time.time - (notesManager.NoteDataAll[noteTiming].time + mainManager.startTime)), noteTiming);
                                break;  // �אڂ��Ă���m�[�c����ɂ��Ԃ�Ȃ��悤break���Ă���
                            }
                            else
                            {
                                NoteData tmpNote = new NoteData(notesManager.NoteDataAll[noteTiming]);
                                longNoteDataList.Add(tmpNote);
                                CheckHitTiming(Mathf.Abs(Time.time - (notesManager.NoteDataAll[noteTiming].time + mainManager.startTime)), noteTiming);
                            }
                        }
                    }
                    soundMain.PlaySE((int)SoundMain.SE.Touch);
                }
            }
            

            // �~�X����
            if (Time.time > notesManager.NoteDataAll[0].time + MissSecond + mainManager.startTime)
            {
                PopupJudgeMsg(3);
                DeleteData(0);
                mainManager.ResetCombo();
                mainManager.AddJudgeCount(3);
            }
        }
        

        if (longNoteDataList.Count != 0)
        {
            // NoteData���ʂŌ���
            for (int noteNum = longNoteDataList.Count - 1; noteNum >= 0; noteNum--)
            {
                if (!PushingKey() && Mathf.Abs(Time.time - (longNoteDataList[noteNum].longNotes[longNoteDataList[noteNum].longNotes.Count - 1].time + mainManager.startTime)) > BadSecond)
                {
                    foreach (LongNoteData longnote in longNoteDataList[noteNum].longNotes)
                    {
                        PopupJudgeMsg(3, longnote.laneNum);
                        mainManager.ResetCombo();
                        mainManager.AddJudgeCount(3);
                        Destroy(longnote.notes);
                    }
                    longNoteDataList.Remove(longNoteDataList[noteNum]);
                }
                else
                {
                    Debug.Log(noteNum);
                    UpdateLongNotes(longNoteDataList[noteNum]);

                }
            }
            for (int i = longNoteDataList.Count - 1; i >= 0; i--)
            {
                if (longNoteDataList[i].isEnd)
                {
                    foreach (LongNoteData longnote in longNoteDataList[i].longNotes)
                    {
                        Destroy(longnote.notes);
                    }
                    longNoteDataList.RemoveAt(i);
                }
            }


        }

    }

    private void FixedUpdate()
    {
        UpdateText();
    }

    void CheckHitTiming(float timeLag,int offset, bool isLong = false)
    {
        if (timeLag <= PerfectSecond)
        {
            PopupJudgeMsg(0, offset);
            mainManager.AddCombo();
            mainManager.AddJudgeCount(0);
        }
        else if (timeLag <= GreatSecond)
        {
            PopupJudgeMsg(1, offset);
            mainManager.AddCombo();
            mainManager.AddJudgeCount(1);
        }
        else if (timeLag <= BadSecond)
        {
            PopupJudgeMsg(2, offset);
            mainManager.ResetCombo();
            mainManager.AddJudgeCount(2);
        }
        // �����O�m�[�c�ł���ꍇ�͂��ׂč폜���Ȃ�
        if (!isLong && timeLag <= BadSecond)
        {
            
            DeleteData(offset);
        }
        else if (isLong && timeLag <= BadSecond)
        {
            notesManager.NoteDataAll.RemoveAt(offset);
            Destroy(notesManager.NotesObj[offset]);
        }
    }

    void UpdateLongNotes(NoteData notedata)
    {
        //for (int i = 0; i�@< notedata.longNotes.Count; i++)
        for (int i = notedata.longNotes.Count - 1; i >= 0; i--)
        {

            // �����O�m�[�c���ɏo�Ă���m�[�c����
            if (i != notedata.longNotes.Count - 1)
            {
                // �p�[�t�F�N�g����͈̔͂ɓ����Ă��Ă܂����肵�Ă��Ȃ���Ԃł���Ώ���
                if ((Mathf.Abs(Time.time - (notedata.longNotes[i].time + mainManager.startTime))) <= PerfectSecond && !notedata.longNotes[i].passnext)
                {
                    if (pushingKeyState[notedata.longNotes[i].laneNum])
                    {
                        PopupJudgeLongMsg(0, notedata.longNotes[i].laneNum);
                        notedata.longNotes[i].passnext = true;
                        soundMain.PlaySE((int)SoundMain.SE.Touch);
                    }
                }
            }
            // �Ō�̃����O�m�[�c�̔���
            else
            {
                if ((Mathf.Abs(Time.time - (notedata.longNotes[i].time + mainManager.startTime))) <= MissSecond && !notedata.isEnd)
                {
                    if (!pushingKeyState[notedata.longNotes[i].laneNum])
                    {
                        if ((Mathf.Abs(Time.time - (notedata.longNotes[i].time + mainManager.startTime)) <= PerfectSecond))
                        {
                            PopupJudgeLongMsg(0,notedata.longNotes[i].laneNum);
                            mainManager.AddCombo();
                            mainManager.AddJudgeCount(0);
                        }
                        else if ((Mathf.Abs(Time.time - (notedata.longNotes[i].time + mainManager.startTime)) <= GreatSecond))
                        {
                            PopupJudgeMsg(1, notedata.longNotes[i].laneNum);
                            mainManager.AddCombo();
                            mainManager.AddJudgeCount(1);
                        }
                        else if ((Mathf.Abs(Time.time - (notedata.longNotes[i].time + mainManager.startTime)) <= BadSecond))
                        {
                            PopupJudgeMsg(2, notedata.longNotes[i].laneNum);
                            mainManager.ResetCombo();
                            mainManager.AddJudgeCount(2);
                        }
                        else if ((Mathf.Abs(Time.time - (notedata.longNotes[i].time + mainManager.startTime)) <= MissSecond))
                        {
                            PopupJudgeMsg(3, notedata.longNotes[i].laneNum);
                            mainManager.ResetCombo();
                            mainManager.AddJudgeCount(3);
                        }
                        soundMain.PlaySE((int)SoundMain.SE.Touch);
                        notedata.isEnd = true;
                    }
                }
            }
        }
        
    }

    
    void UpdatePushingKeyState()
    {
        // enum��PushingKey�ɂȂ��Ă��܂� �ύX��
        touchKeyState[(int)PusingKey.D] = Input.GetKeyDown(KeyCode.D);
        touchKeyState[(int)PusingKey.F] = Input.GetKeyDown(KeyCode.F);
        touchKeyState[(int)PusingKey.J] = Input.GetKeyDown(KeyCode.J);
        touchKeyState[(int)PusingKey.K] = Input.GetKeyDown(KeyCode.K);

        pushingKeyState[(int)PusingKey.D] = Input.GetKey(KeyCode.D);
        pushingKeyState[(int)PusingKey.F] = Input.GetKey(KeyCode.F);
        pushingKeyState[(int)PusingKey.J] = Input.GetKey(KeyCode.J);
        pushingKeyState[(int)PusingKey.K] = Input.GetKey(KeyCode.K);
    }

    void DeleteData(int offset)
    {

        notesManager.NoteDataAll.RemoveAt(offset);
        Destroy(notesManager.NotesObj[offset]);
        notesManager.NotesObj.RemoveAt(offset);

        if (notesManager.NoteDataAll.Count <= 0)
        {
            mainManager.isEnd = true;
        }
    }

    void UpdateText()
    {
        comboText.text = "Combo\n"+mainManager.GetCombo().ToString();
        scoreText.text = "Score:" + mainManager.GetPoint().ToString();
    }
    void PopupJudgeMsg(int judge,int offset = 0)
    {
        // Instance�̍폜�����̓I�u�W�F�N�g�ɋL�q
        Instantiate(JudgeMsgObj[judge], new Vector3(notesManager.NoteDataAll[offset].laneNum - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));

        if (judge != 3)
        {
            Instantiate(hitEffect, new Vector3(notesManager.NoteDataAll[offset].laneNum - 1.5f, 0.6f, 0f), Quaternion.Euler(90, 0, 0));
        }
    }
    void PopupJudgeLongMsg(int judge,int laneNum)
    {
        // Instance�̍폜�����̓I�u�W�F�N�g�ɋL�q
        Instantiate(JudgeMsgObj[judge], new Vector3(laneNum - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));
    }


    bool PushingKey()
    {
        if (Input.anyKey)
        {
            return true;
        }
        return false;
    }
    void AddPoint()
    {
        mainManager.point = (int)Math.Round(1000000 * Math.Floor(mainManager.playerScore / mainManager.maxScore * 1000000) / 1000000);
    }
}
