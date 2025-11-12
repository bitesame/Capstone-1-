using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSave : MonoBehaviour
{
    public TMP_InputField inputUserID;
    public TMP_InputField inputPassword;

    public TMP_Text displayMessage;
    public TMP_Text newUserText;
    

    private string username;
    private string password;


    private const string LAST_USERNAME_KEY = "LastLoginUsername";
    private const string LAST_PASSWORD_KEY = "LastLoginPassword";

    //로드씬에서 로그 실패
    public GameObject newUserGroup;
    // Start is called before the first frame update
    void Start()
    {
        PlayFabSettings.TitleId = "162CA1";

        if (PlayerPrefs.HasKey(LAST_USERNAME_KEY))
        {
            inputUserID.text = PlayerPrefs.GetString(LAST_USERNAME_KEY);
            username = inputUserID.text;
        }

        if (PlayerPrefs.HasKey(LAST_PASSWORD_KEY))
        {
            inputPassword.text = PlayerPrefs.GetString(LAST_PASSWORD_KEY);
            password = inputPassword.text;
        }



        newUserGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UsernameValueChanged() { username = inputUserID.text; }
    public void PasswordValueChanged() { password = inputPassword.text; }

    // 기존 유저 로그인 (이메일 필요 없음)
    public void LoginExistingUser()
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(
            request,
            OnLoginSuccess,
            OnLoginFailure
        );
    }

    private void OnLoginSuccess(LoginResult result)
    {
        displayMessage.text = "Login success";

        // 저장된 유저 데이터 불러오는 함수 (아직 미구현)
        // 로그인 성공 시 PlayerPrefs에 저장
        PlayerPrefs.SetString(LAST_USERNAME_KEY, username);
        PlayerPrefs.SetString(LAST_PASSWORD_KEY, password);

        // 게임 시작 or 이어하기 씬 이동
        StartGame();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
        displayMessage.text = error.GenerateErrorReport();
        newUserText.text = "New?";
   
        newUserGroup.SetActive(true);

    }

    private void StartGame()
    {
        Debug.Log("Load Scene Login success → Now move to game scene");
       
        //성공하면 씬 이동하게 이동할 메인 씬 추가
        //SceneManager.LoadScene("MainScene"); 
    }


    // 저장된 정보 불러오기
    // void LoadUserData(string playFabId)
    // {
    //     // 플레이 데이터 불러오는 기능 만들면 여기 작성
    // }

    //  저장씬에서 호출될 저장 기능
    // public void SaveUserData()
    // {
    //     // 저장씬 만들면 이 함수에서 PlayFab에 저장하면 됨
    // }
}
