using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PermanentUI : MonoBehaviour
{
    public int cherries = 0;
    public TextMeshProUGUI CherrtText;
    public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!perm)
        {
            perm = this;
        }
        else {
            Destroy(gameObject);
        }
    }
}
