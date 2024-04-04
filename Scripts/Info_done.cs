using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info_done : MonoBehaviour
{
    public Image img_avatar;
    public Text txt_name_player;
    public Text txt_msg;

    public void onload(Sprite sp_avatar,string s_name_player,string s_msg)
    {
        this.img_avatar.sprite = sp_avatar;
        this.txt_name_player.text = s_name_player;
        this.txt_msg.text = s_msg;
    }

    public void btn_back_home()
    {
        GameObject.Find("Games").GetComponent<Games>().btn_back_home();
    }

    public void btn_reset()
    {
        GameObject.Find("Games").GetComponent<Games>().manager_p.on_reset();
    }
}
