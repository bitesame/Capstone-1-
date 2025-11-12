using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoginSave : MonoBehaviour
{
    public TMP_InputField inputUserID;
    public TMP_InputField inputPassword;
    public TMP_InputField inputEmail;

    public TMP_Text displayMessage;

    private string username;
    private string password;
    private string email;
    private const string LAST_USERNAME_KEY = "LastLoginUsername";
    // Start is called before the first frame update
    void Start()
    {
        PlayFabSettings.TitleId = "162CA1";

        // 이전에 저장된 사용자 이름을 로드하여 InputField에 표시
        /*if (PlayerPrefs.HasKey(LAST_USERNAME_KEY))
        {
            inputUserID.text = PlayerPrefs.GetString(LAST_USERNAME_KEY);
            // 저장된 값을 변수에도 할당하여 바로 사용할 수 있도록 준비
            username = inputUserID.text;
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UsernameValueChanged() { username = inputUserID.text.ToString(); }
    public void PasswordValueChanged() { password = inputPassword.text.ToString(); }
    public void EmailValueChanged() { email = inputEmail.text.ToString(); }

    public void Login()
    {
        var request = new LoginWithPlayFabRequest { Username = username, Password = password };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        displayMessage.text = "Login successfully";
        // 로그인 성공 시 사용자 이름(ID)을 PlayerPrefs에 저장
        //PlayerPrefs.SetString(LAST_USERNAME_KEY, username);
        //PlayerPrefs.Save();

        StartGame();
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
        displayMessage.text = error.GenerateErrorReport();
    }
    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Username = username, Password = password, Email = email };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }
    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        displayMessage.text = "Signup successfully";
    }
    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
        displayMessage.text = error.GenerateErrorReport();
    }
    void StartGame() 
    { 
        Debug.Log("Now, start the game, enjoy it");
        //성공 시 플레이할 수 있도록 메인 플레이씬
        //SceneManager.LoadScene("MainScene");

    }

    void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
{"Ancestor", "Arthur"},
{"Successor", "Fred"}
}
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }


    void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("Ancestor")) Debug.Log("No Ancestor");
            else Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }


}
