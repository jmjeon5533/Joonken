using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Image[] charimage;
    public Sprite[] charSprite;

    //public int[] SelectIndex = new int[3];
    //public Transform SelectImage;

    public Transform buttonParent;
    void Start()
    {
        var s = SceneManager.instance;
        for(int i = 0; i < buttonParent.childCount; i++)
        {
            var num = i;
            buttonParent.GetChild(i).GetComponent<Button>().onClick.AddListener(() => 
            {
                var input = s.charIndex[0] == 99 ? 0 : 1;
                s.charIndex[input] = num;
                charimage[input].sprite = charSprite[num];
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        //SelectImage.localPosition = 
    }
}
