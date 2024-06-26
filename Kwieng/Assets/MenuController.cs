using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_Controller uiController;

    public void OnHowToClick()
    {
        uiController.Activate_HowToPlay(true);
    }

    public void OnStartGameClick()
    {
        uiController.DeactivateAll();
        uiController.Activate_PlayerOrBot(true);
    }

    public void OnPlayerClick()
    {
        uiController.DeactivateAll();
        GameManagers.Instance.InitGameState_VsPlayer();
    }

    public void OnBotClick()
    {
        uiController.DeactivateAll();
        uiController.Activate_Difficulty(true);
        GameManagers.Instance.InitGameState_VsBot();
    }

    public void OnConclusion(string _winnerName)
    {
        uiController.DeactivateAll();
        uiController.Activate_Winner(true, _winnerName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ShareSocial()
    {
        StartCoroutine(TakeScreenShotAndShare());
    }

    IEnumerator TakeScreenShotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D _tx = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _tx.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _tx.Apply();

        string _path = Path.Combine(Application.temporaryCachePath, "sharedImage.png");
        File.WriteAllBytes(_path, _tx.EncodeToPNG());

        Destroy(_tx);

        new NativeShare()
            .AddFile(_path)
            .SetSubject("This is my score")
            .SetText("share your score with your friends")
            .Share();
    }

}
