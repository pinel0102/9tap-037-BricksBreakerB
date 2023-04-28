using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfile : MonoBehaviour
{
    [Header("★ [Reference] User Profile")]
    public Image currentAvatar;
    public Image currentFrame;
    public List<Toggle> avatarItem;
    public List<Toggle> frameItem;

    [Header("★ [Reference] User Profile Out")]
    public List<Image> currentAvatarOut = new List<Image>();
    public List<Image> currentFrameOut = new List<Image>();

    public void Initialize()
    {
        SetupButtons();

        SetAvatar(GlobalDefine.UserInfo.Settings_Avatar);
        SetFrame(GlobalDefine.UserInfo.Settings_Frame);
        RefreshList();
    }

    private void SetupButtons()
    {
        for(int i=0; i < avatarItem.Count; i++)
        {
            int index = i;
            avatarItem[index].onValueChanged.AddListener((isOn) => ChangeAvatar(isOn, index));
        }

        for(int i=0; i < frameItem.Count; i++)
        {
            int index = i;
            frameItem[index].onValueChanged.AddListener((isOn) => ChangeFrame(isOn, index));
        }
    }

    private void RefreshList()
    {
        for(int i=0; i < avatarItem.Count; i++)
        {
            int index = i;
            avatarItem[index].transform.GetChild(1).gameObject.SetActive(index == GlobalDefine.UserInfo.Settings_Avatar);
        }

        for(int i=0; i < frameItem.Count; i++)
        {
            int index = i;
            frameItem[index].transform.GetChild(1).gameObject.SetActive(i == GlobalDefine.UserInfo.Settings_Frame);
        }
    }

    private void SetAvatar(int index)
    {
        Debug.Log(CodeManager.GetMethodName() + index);
        currentAvatar.sprite = avatarItem[index].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;

        for (int i=0; i < currentAvatarOut.Count; i++)
        {
            currentAvatarOut[i].sprite = currentAvatar.sprite;
        }
    }

    private void SetFrame(int index)
    {
        Debug.Log(CodeManager.GetMethodName() + index);
        currentFrame.sprite = frameItem[index].transform.GetChild(0).GetComponent<Image>().sprite;

        for (int i=0; i < currentFrameOut.Count; i++)
        {
            currentFrameOut[i].sprite = currentFrame.sprite;
        }
    }

    private void ChangeAvatar(bool isOn, int index)
    {
        if (!isOn || GlobalDefine.UserInfo.Settings_Avatar == index)
            return;

        GlobalDefine.UserInfo.Settings_Avatar = index;
        GlobalDefine.SaveUserData();

        SetAvatar(index);
        RefreshList();
    }

    private void ChangeFrame(bool isOn, int index)
    {
        if (!isOn || GlobalDefine.UserInfo.Settings_Frame == index)
            return;

        GlobalDefine.UserInfo.Settings_Frame = index;
        GlobalDefine.SaveUserData();
        
        SetFrame(index);
        RefreshList();
    }
}