using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Values",menuName ="Values", order = 1)]
public class Values : ScriptableObject
{
    public static string POINTER = "Pointer";
    public static string MAIN_CANVAS = "MainCanvas";
    public static string EMPTY_STRING = "";
    public static string NOTICE_SELECT_START_POS = "Select start position"; 
    public static string NOTICE_SELECT_END_POS = "Select end position";
    public static string NOTICE_DRAW_BLOCK = "Draw block to make your maze";
    public static string NOTICE_DRAW_WAY = "Let solve the maze";
    public static string NOTICE_CAN_RUNNING = "Press run button !!!";

    public static bool SHOW_STEPS = true;
    public static bool SHOW_PATH = true;
    public static bool ENABLE_DRAW_WAY = false;
    public static bool ENABLE_DRAW_BLOCK = false;

    public const int TileWeight_Default = 1;
    public const int TileWeight_Expensive = 100;
    public const int TileWeight_Infinity = int.MaxValue;

    public static float PlaySite_Tranform_Left = 0f;
    public static float PlaySite_Tranform_Top = 0f;
    public static float PlaySite_Tranform_Right = 0f;
    public static float PlaySite_Tranform_Bot = 0f;
}
