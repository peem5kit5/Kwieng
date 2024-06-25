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
    [SerializeField] private string userName, email;
    [SerializeField] private Sprite imageToLoad;

    [Header("Test References")]
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI Username, Email;

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

    public void InitUI()
    {
        //Test
        profileImage.sprite = imageToLoad;
        Username.text = userName;
        Email.text = email;
    }

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

                userName = user.DisplayName;
                email = user.Email;
               // StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
                DontDestroyOnLoad(this);
                DontDestroyOnLoad(gameObject);

            });

            InitUI();
        }
    }

    private IEnumerator LoadImage(string _imageUrl)
    {
        WWW _www = new WWW(_imageUrl);
        yield return _www;

        imageToLoad = Sprite.Create(_www.texture, new Rect(0, 0, _www.texture.width, _www.texture.height), new Vector2(0, 0));
    }

    private string CheckImageUrl(string _url)
    {
        if (!string.IsNullOrEmpty(_url))
            return _url;

        Debug.LogError("There no Image to Profile.");
        return null;
    }

    public void GuestLogin()
    {

    }


}
