using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public Transform levelRoot;
    public DialogueManager dialogueManager;
    public GameObject endDayButton;
    public GameObject endDayScreen;

    [Header("Boxes")]
    public GameObject box1Prefab;
    public GameObject box2Prefab;
    public Vector3[] boxPositions;

    [Header("DAY 1")]
    public GameObject[] day1Objects;
    public string[] day1Names;
    public string[] day1Colors;
    public int[] day1Cutoffs;

    [Header("DAY 2")]
    public GameObject[] day2Objects;
    public string[] day2Names;
    public string[] day2Colors;
    public int[] day2Cutoffs;

    [Header("DAY 3")]
    public GameObject[] day3Objects;
    public string[] day3Names;
    public string[] day3Colors;
    public int[] day3Cutoffs;

    [Header("DAY 4")]
    public GameObject[] day4Objects;
    public string[] day4Names;
    public string[] day4Colors;
    public int[] day4Cutoffs;

    [Header("DAY 5")]
    public GameObject[] day5Objects;
    public string[] day5Names;
    public string[] day5Colors;
    public int[] day5Cutoffs;

    [Header("DAY 6")]
    public GameObject[] day6Objects;
    public string[] day6Names;
    public string[] day6Colors;
    public int[] day6Cutoffs;

    [Header("DAY 7")]
    public GameObject[] day7Objects;
    public string[] day7Names;
    public string[] day7Colors;
    public int[] day7Cutoffs;


    GameObject[][] dayObjects;
    string[][] dayNames;
    string[][] dayColors;
    int[][] dayCutoffs;

    int dayIndex = 0;
    BoxDispenser[] boxes;

    void Awake(){
        //Get current save file from static class

        //load in save file

        dayObjects = new GameObject[][] {day1Objects,day2Objects,day3Objects,day4Objects,day5Objects,day6Objects,day7Objects};
        dayNames = new string[][] {day1Names,day2Names,day3Names,day4Names,day5Names,day6Names,day7Names};
        dayColors = new string[][] {day1Colors,day2Colors,day3Colors,day4Colors,day5Colors,day6Colors,day7Colors};
        dayCutoffs = new int[][] {day1Cutoffs,day2Cutoffs,day3Cutoffs,day4Cutoffs,day5Cutoffs,day6Cutoffs,day7Cutoffs};
        
        boxes = new BoxDispenser[2];

        StartNextDay();
    }

    public void EndCurrentDay(){
        //show end screen n stuff
        dialogueManager.EndDialogue();
        endDayButton.SetActive(false);
        endDayScreen.SetActive(true);

        //delete certain books maybe

        dayIndex++;
    }

    public void StartNextDay (){
        endDayScreen.SetActive(false);

        for (int i = 0; i < boxes.Length; i++){
            boxes[i] = Instantiate(box1Prefab,Vector3.zero,Quaternion.identity).GetComponent<BoxDispenser>();
            boxes[i].transform.parent = levelRoot;
            boxes[i].transform.localPosition = boxPositions[i];
        }

        for (int i = 0; i < boxes.Length; i++){
            int start = dayCutoffs[dayIndex][i];
            int end = i+1 < dayCutoffs[dayIndex].Length ? dayCutoffs[dayIndex][i+1] : dayObjects[dayIndex].Length;
            if (start >= end){
                DestroyImmediate(boxes[i].gameObject);
                continue;
            }
            GameObject[] os = new GameObject[end-start];
            string[] ns = new string[end-start];
            string[] cs = new string[end-start];
            for (int j = start; j < end; j++){
                os[j-start] = dayObjects[dayIndex][j];
                ns[j-start] = dayNames[dayIndex][j];
                cs[j-start] = dayColors[dayIndex][j];
            }
            boxes[i].Setup(os,ns,cs,this);
        }

        IPopped(null);//this is just for testing should never actually happen
    }

    public void IPopped(BoxDispenser popee){
        bool isDone = true;
        for (int i = 0; i < boxes.Length; i++){
            if (boxes[i] != null && boxes[i] != popee){
                isDone = false;
            }
        }
        if (isDone){
            endDayButton.SetActive(true);
        }
    }
}
