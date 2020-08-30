using UnityEngine.UI;
using UnityEngine;

public class AndroidToast : MonoBehaviour
{
    //public Button showToastButton;
    public string toastText = "This is a Toast";

    public static void show(string msg)
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            try
            {
                //create a Toast class object
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");

                //create an array and add params to be passed
                object[] toastParams = new object[3];
                AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                toastParams[0] = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");
                toastParams[1] = msg;
                toastParams[2] = toastClass.GetStatic<int>("LENGTH_LONG");

                //call static function of Toast class, makeText
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", toastParams);

                //show toast
                toastObject.Call("show");
            }
            catch (System.Exception)
            {
                print("This platform does not support form android Toast");
            }
        }
    }
}