using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Dialogue Data", menuName = "Yarn Data/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public Sprite portrait;
    [SerializeField] public AnimationClip animationClip; 
}
