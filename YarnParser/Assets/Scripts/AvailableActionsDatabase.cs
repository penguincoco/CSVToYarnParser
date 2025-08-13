using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AvailableActionsDatabase", menuName = "Utilities/Available Actions Database")]
public class AvailableActionsDatabase : ScriptableObject
{
    public List<AvailableActionsData> actionsDataList = new List<AvailableActionsData>();
}

