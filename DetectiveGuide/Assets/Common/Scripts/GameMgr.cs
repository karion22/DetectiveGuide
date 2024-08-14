using KRN.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// 게임 모드
public enum eGameMode
{
    Clue,
    HarryPotter,
    Custom
}

public enum eDataType
{
    Name,
    Weapon,
    Place,
}

// 플레이 유저 정보
public class UserData
{
    public string UserName;
    public List<string> Items = new List<string>();

    public void Clear()
    {
        UserName = "";
        Items.Clear();
    }
}

// 진행 정보
public class PlayDetailItem
{
    public eDataType m_Type;
    public string m_Value;

    public string GetStandardValue()
    {
        return Utility.BuildString("<color=#dedede>{0}</color>{1}", GetString(), m_Value);
    }

    private string GetString()
    {
        return (m_Type switch
        {
            eDataType.Name => "이름",
            eDataType.Weapon => "무기",
            eDataType.Place => "장소",
            _ => ""
        });
    }
}

public class PlayItem
{
    public int m_Turn;
    public string m_UserName;
    public List<PlayDetailItem> m_Items = new List<PlayDetailItem>();

    public void Add(PlayDetailItem inItem)
    {
        m_Items.Add(inItem);
    }

    public void Remove(eDataType inType, string inValue)
    {
        var item = m_Items.Find((value) => {
            return (inType == value.m_Type && inValue == value.m_Value);
        });

        if(item != null)
            m_Items?.Remove(item);
    }
}

public class ResultItem
{
    public string Name;
    public ResultSubItem[] Items;
}

public class ResultSubItem
{
    public string Name;
    public string Value;
}

// 게임 진행 정보
public class PlayHistory
{
    public List<PlayItem> m_Items = new List<PlayItem>();

    public void Add(PlayItem item)
    {
        m_Items.Add(item);
    }

    public void Remove(int inTurn, string inUserName)
    {
        var target = m_Items.Find((value) => {
        if (value.m_Turn == inTurn && value.m_UserName.Equals(inUserName))
            return true;
        else
            return false;
        });

        if (target != null)
            m_Items.Remove(target);
    }

    public void Clear()
    {
        m_Items.Clear();
    }
}

public class GameMgr : Singleton<GameMgr>
{
    // 게임 모드
    public eGameMode GameMode = eGameMode.Clue;

    // 사용되는 아이템 개수
    public int ItemCount = 3;

    // 최대 아이템 개수
    public const int MAX_ITEM_COUNT = 4;

    // 플레이 유저 숫자
    public int UserCount = 4;

    // 최대 인원 수
    public const int MAX_USER_COUNT = 8;

    // 게임 셋팅 값
    private GameDataSetScriptable m_DataSet;
    private List<GameDataSetScriptable> m_DataSetList;

    // 유저 값
    private List<UserData> m_UserList = new List<UserData>();

    // 플레이 정보
    private List<PlayHistory> m_PlayStack = new List<PlayHistory>();

    public override void Awake()
    {
        base.Awake();

        m_DataSetList.Clear();
        var async = Addressables.LoadAssetsAsync<GameDataSetScriptable>("GameData/GameRules", (value) => { m_DataSetList.Add(value); });
    }

    public void SetGameMode(eGameMode inGameMode)
    {
        GameMode = inGameMode;
        m_PlayStack.Clear();

         //(int)(GameMode);
        switch (GameMode)
        {
            case eGameMode.Clue:
                break;

            case eGameMode.HarryPotter:
                // TODO
                break;

            case eGameMode.Custom:
                break;
        }

        Restart();
    }

    #region Item Count Control
    // 아이템 개수를 증가시킨다.
    public void IncreaseItemCount(int inValue = 1)
    {
        ItemCount = Mathf.Min(ItemCount + inValue, MAX_ITEM_COUNT);
    }

    // 아이템 개수를 감소시킨다.
    public void DecreaseItemCount(int inValue = 1)
    {
        ItemCount = Mathf.Max(ItemCount - inValue, 1);
    }

    public void SetItemCount(int inValue)
    {
        ItemCount = Mathf.Max(Mathf.Min(inValue, MAX_ITEM_COUNT), 1);
    }
    #endregion

    #region User Count Control
    // 유저 수를 증가시킨다.
    public void IncreaseUserCount(int inValue = 1)
    {
        UserCount = Mathf.Min(UserCount + inValue, MAX_USER_COUNT);
    }

    // 유저 수를 감소시킨다.
    public void DecreaseUserCount(int inValue = 1)
    {
        UserCount = Mathf.Max(UserCount - inValue, 1);
    }

    public void SetUserCount(int inValue)
    {
        UserCount = Mathf.Max(Mathf.Min(inValue, MAX_USER_COUNT), 1);
    }
    #endregion

    #region User Info Control
    public void ClearUserList()
    {
        m_UserList.Clear();
    }

    public void AddUserData(UserData inUserData)
    {
        if (m_UserList.Contains(inUserData) == false)
            m_UserList.Add(inUserData);
    }

    public void RemoveUserData(UserData inUserData)
    {
        m_UserList.Remove(inUserData);
    }
    #endregion

    public void Restart()
    {
        m_PlayStack.Clear();
    }
}