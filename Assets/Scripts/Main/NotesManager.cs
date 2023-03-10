using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using LitJson;

[Serializable]
public class NoteData
{
    public int type;
    public float num;
    public int block;
    public int LPB;
    public List<LongNoteData> longNotes;

    public NoteData(int type,int num,int block,int LPB) 
    { 
        
    }
}

[Serializable]
public class LongNoteData
{
    public int type;
    public float num;
    public int block;
    public int LPB;
}

enum NotesType
{
    None,
    NormalNotes,
    LongNotes
}

public class NotesManager : MonoBehaviour
{
    public int noteNum;

    [SerializeField] MainManager mainManager;

    public List<NoteData> NoteDataAll = new List<NoteData>();

    public List<int> LaneNum = new List<int>();
    public List<int> NoteType = new List<int>();
    public List<float> NotesTime = new List<float>();
    public List<GameObject> NotesObj = new List<GameObject>();
    public List<GameObject> LongNotesObj = new List<GameObject>();

    [SerializeField] float NotesSpeed;
    [SerializeField] GameObject noteObj;

    void Start()
    {
        noteNum = 0;
        Load(mainManager.songName);

    }

    private void Load(string SongName)
    {
        AsyncOperationHandle json;
        json = Addressables.LoadAssetAsync<TextAsset>($"Assets/Resource/Scores/{mainManager.songName}.json");
        var scoreLoad = json.WaitForCompletion();

        TextAsset score = json.Result as TextAsset;
        //Data inputJson = JsonMapper.ToObject<Data>(score.text);  // LitJson

        JsonData jsonData = JsonMapper.ToObject(score.ToString());

        Addressables.Release(json);

        noteNum = jsonData["notes"].Count;
        string BPM = jsonData["BPM"].ToString();
        string OFFSET = jsonData["offset"].ToString();

        for (int i = 0; i < jsonData["notes"].Count; i++)
        {
            string LPB = jsonData["notes"][i]["LPB"].ToString();
            string NUM = jsonData["notes"][i]["num"].ToString();
            string BLOCK = jsonData["notes"][i]["block"].ToString();
            string TYPE = jsonData["notes"][i]["type"].ToString();

            float space = 60 / (float.Parse(BPM) * float.Parse(LPB));
            float beatSec = space * float.Parse(LPB);
            float time = (beatSec * float.Parse(NUM) / float.Parse(LPB) + float.Parse(OFFSET) * 0.01f);

            //NoteData noteData = new NoteData();

            NotesTime.Add(time);
            LaneNum.Add(int.Parse(BLOCK));
            NoteType.Add(int.Parse(TYPE));

            float z = NotesTime[NotesTime.Count - 1] * NotesSpeed;
            NotesObj.Add(Instantiate(noteObj, new Vector3(int.Parse(BLOCK) - 1.5f, 0.55f, z), Quaternion.identity));
            // ロングノーツ判定
            if ((int)jsonData["notes"][i]["type"] == 2)
            {
                // ロングノーツ作成処理
                for (int j = 0; j < jsonData["notes"][i]["notes"].Count; j++){
                    // JsonDataクラスで(flaot)(int)キャストするとエラーになるため
                    // 一時的に保存する変数を作成
                    LPB      = jsonData["notes"][i]["notes"][j]["LPB"].ToString();
                    NUM      = jsonData["notes"][i]["notes"][j]["num"].ToString();
                    BLOCK    = jsonData["notes"][i]["notes"][j]["block"].ToString();
                    TYPE     = jsonData["notes"][i]["notes"][j]["type"].ToString();

                    space = 60 / (float.Parse(BPM) * float.Parse(LPB));
                    beatSec = space * float.Parse(LPB);
                    time    = (beatSec * float.Parse(NUM) / float.Parse(LPB) + float.Parse(OFFSET) * 0.01f);

                    NotesTime.Add(time);
                    LaneNum.Add(int.Parse(BLOCK));
                    NoteType.Add(int.Parse(TYPE));

                    z = NotesTime[NotesTime.Count - 1] * NotesSpeed;
                    
                    noteNum++;
                    
                    NotesObj.Add(Instantiate(noteObj, new Vector3(int.Parse(BLOCK) - 1.5f, 0.55f, z), Quaternion.identity));

                    LongNotesCreate(NotesObj[NotesObj.Count - 2].transform,NotesObj[NotesObj.Count - 1].transform);
                }
                
            }
        }
        Debug.Log(noteNum);
        mainManager.maxScore = noteNum * mainManager.MAX_RAITO_POINT;
    }

    private const int LANE_WIDTH = 1;
    private void LongNotesCreate(Transform start, Transform end)
    {


        GameObject longNotesLine = new GameObject();
        longNotesLine.AddComponent<MeshFilter>();
        longNotesLine.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        longNotesLine.GetComponent<MeshFilter>().mesh = mesh;
        Vector3[] vertices = new Vector3[4];
        int[] triangles = { 0, 2, 1, 3, 1, 2 };

        vertices[0] = start.position + new Vector3(-LANE_WIDTH / 2, 0, 0);
        vertices[1] = start.position + new Vector3( LANE_WIDTH / 2, 0, 0);
        vertices[2] = end.position   + new Vector3( LANE_WIDTH / 2, 0, 0);
        vertices[3] = end.position   + new Vector3(-LANE_WIDTH / 2, 0, 0);

        mesh.vertices  = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}