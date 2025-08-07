using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInternet : UICanvas
{


    private void Update()
    {
        this.transform.SetAsLastSibling();
    }

    public void ButtonTryAgain()
    {
        if (IsInternet())
        {
            UIManager.Ins.CloseUI<NoInternet>();
        }
    }

    public static bool Check()
    {
        bool isInternet = IsInternet();
        if (!isInternet)
        {
            UIManager.Ins.OpenUI<NoInternet>();
        }
        return isInternet;
    }

    public static bool IsInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
