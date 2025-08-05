using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailableActions", menuName = "Utilities/Available Actions")]
public class AvailableActionsData : ScriptableObject
{
    [System.Serializable]
    public class ActionEntry
    {
        public string key;
        [TextArea]
        public string value;
    }

    public List<ActionEntry> actions = new List<ActionEntry>();
}