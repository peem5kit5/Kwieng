using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using Google;
using TMPro;
using Firebase.Auth;

public class LoginController : MonoBehaviour
{
    [Header("GoogleAPI")]
    public string GoogleAPI = "402164554722-on0nl7ha7moecu9c69imntkarh771dj2.apps.googleusercontent.com";

    [Header("Loaded Data")]
    public string UserName;
    public string Email;
    public Sprite ImageToLoad;

    [Header("Test References")]
    public bool IsHavingGoogle;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI username, email;

    private GoogleSignInConfiguration googleConfiguration;
    private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;


    private void Awake()
    {
        googleConfiguration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleAPI,
            RequestIdToken = true
        };
    }

    private void Start() => auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    public void GoogleLogin()
    {
        GoogleSignIn.Configuration = googleConfiguration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnFinishLogin);
    }

    private void OnFinishLogin(Task<GoogleSignInUser> _task)
    {
        if (_task.IsFaulted)
        {
            Debug.LogError("Faulted");
            return;
        }

        else if (_task.IsCanceled)
        {
            Debug.LogError("Cancelled");
            return;
        }
        else
        {
            Firebase.Auth.Credential _credential = Firebase.Auth.GoogleAuthProvider.GetCredential(_task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(_credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;

                UserName = user.DisplayName;
                Email = user.Email;
                IsHavingGoogle = true;

                DontDestroyOnLoad(this);
                DontDestroyOnLoad(gameObject);
                SceneManager.LoadScene(1);
            });
        }
    }

    public void LoadImage(Image _image)
    {
        StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString()), _image));
    }
    private IEnumerator LoadImage(string _imageUrl, Image _image)
    {
        using (UnityWebRequest _www = UnityWebRequestTexture.GetTexture(_imageUrl))
        {
            yield return _www.SendWebRequest();

            if (_www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load image: " + _www.error);
            }
            else
            {
                Texture2D _texture = DownloadHandlerTexture.GetContent(_www);
                _image.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }

        Debug.LogError("There is no image to profile.");
        return null;
    }

    public void GuestLogin()
    {
        SceneManager.LoadScene(1);
    }


}
