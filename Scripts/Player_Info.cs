using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Info : MonoBehaviour
{
    public Sprite sp_avatar_gameover;
    public Sprite sp_mode_robot;
    public Sprite sp_mode_human;
    public Image img_avatar;
    public Image img_model_play;
    public Text txt_name_player;
    public Text txt_tip_player;
    public Animator ani;
    public GameObject effect_punched_obj;
    public GameObject obj_btn_add_heart;

    private List<Penguin> list_penguin;
    private Area_create_pengin tray_create;
    private Penguin penguin_select;
    private bool is_gameover = false;
    private Sprite sp_avatar;
    private bool is_robot;
    private int index_tr_start;
    private int index_penguin_prefab;
    private GameObject p_prefab;
    private Carrot.Carrot_Window_Msg box_msg;

    private int index_btn_gamepad = 0;
    private List<GameObject> list_obj_btn_gamepad = new List<GameObject>();

    public void on_load(int index_tray_start, Area_create_pengin tr_create, GameObject penguin_prefab,bool robot,int index_penguis)
    {
        this.index_tr_start = index_tray_start;
        this.index_penguin_prefab = index_penguis;
        this.tray_create = tr_create;
        this.p_prefab = penguin_prefab;
        this.obj_btn_add_heart.SetActive(true);

        this.list_penguin = new List<Penguin>();
        this.ani.Play("Player_info_nomal");

        this.img_avatar.sprite = penguin_prefab.GetComponent<Penguin>().icon;
        this.sp_avatar= penguin_prefab.GetComponent<Penguin>().icon;
        this.create_penguin(index_tray_start,tr_create, penguin_prefab); 
        this.create_penguin(index_tray_start,tr_create, penguin_prefab);
        this.create_penguin(index_tray_start, tr_create, penguin_prefab);

        this.is_robot = robot;

        if (this.is_robot)
            this.txt_tip_player.text = "Robot";
        else
            this.txt_tip_player.text = "People";

        this.effect_punched_obj.SetActive(false);
        this.is_gameover = false;
        this.update_icon_model_ui();
    }

    public void set_name(string s_name)
    {
        this.txt_name_player.text = s_name;
    }

    public void add_penguin()
    {
        GameObject.Find("Games").GetComponent<Games>().play_sound(9);
        this.create_penguin(this.index_tr_start, this.tray_create, p_prefab);
    }

    private void create_penguin(int index_tr_play,Area_create_pengin tr_create,GameObject penguin_prefab)
    {
        this.tray_create = tr_create;
        GameObject obj_penguin = Instantiate(penguin_prefab);
        obj_penguin.transform.SetParent(this.tray_create.area_body);
        obj_penguin.transform.localPosition = new Vector3(0, 0, 0);
        obj_penguin.transform.localScale = new Vector3(1f, 1f, 1f);

        Penguin p_penguin = obj_penguin.GetComponent<Penguin>();
        p_penguin.on_load(index_tr_play, this.tray_create.area_body,this);
        this.list_penguin.Add(p_penguin);
    }

    public void clear_all_penguin()
    {
        for(int i = 0; i < this.list_penguin.Count; i++)
        {
           if(this.list_penguin[i]!=null) Destroy(this.list_penguin[i].gameObject);
        }
        this.list_penguin = new List<Penguin>();
    }

    public void on_focus()
    {
        this.effect_punched_obj.SetActive(false);
        this.ani.Play("Player_info_focus");
        this.on_all_on_focus_penguin();
    }

    public void un_focus()
    {
        this.ani.Play("Player_info_nomal");
        this.on_all_un_focus_penguin();
    }

    public void on_play_robot()
    {
        if (this.is_gameover == false)
        {
            int rand_penguin = Random.Range(0, this.list_penguin.Count);
            if (this.list_penguin[rand_penguin] != null)
                this.list_penguin[rand_penguin].act_click();
            else
                this.on_play_robot();
        }
    }

    public void on_punched()
    {
        this.effect_punched_obj.SetActive(true);
    }

    public void on_all_penguin_nomal()
    {
        for (int i = 0; i < this.list_penguin.Count; i++)
        {
            if(this.list_penguin[i]!=null) this.list_penguin[i].on_waiting();
        }
    }

    public void on_all_on_focus_penguin()
    {
        List<GameObject> list_btn_penguin = new List<GameObject>();
        for (int i = 0; i < this.list_penguin.Count; i++)
        {
            if (this.list_penguin[i] != null)
            {
                this.list_penguin[i].on_focus();
                list_btn_penguin.Add(this.list_penguin[i].gameObject);
            }
        }
        this.set_list_btn_gamepad(list_btn_penguin);
    }

    public void on_all_un_focus_penguin()
    {
        for (int i = 0; i < this.list_penguin.Count; i++)
        {
            if (this.list_penguin[i] != null) this.list_penguin[i].on_waiting();
        }
    }

    public int get_index_cur()
    {
        return this.penguin_select.get_index_tray_play();
    }

    public void set_penguin_select(Penguin p)
    {
        this.penguin_select = p;
    }

    public Penguin get_penguin()
    {
        return this.penguin_select;
    }

    public void check_gameover()
    {
        int count_over = 0;
        for (int i = 0; i < this.list_penguin.Count; i++) if (this.list_penguin[i] == null) count_over++;

        if (count_over >= this.list_penguin.Count-1)
        {
            this.is_gameover = true;
            this.act_gameover();
            GameObject.Find("Games").GetComponent<Games>().manager_p.check_done();
        }
    }

    public bool get_status_gameover()
    {
        return this.is_gameover;
    }

    private void act_gameover()
    {
        this.img_avatar.sprite = this.sp_avatar_gameover;
        this.txt_tip_player.text = "Loser";
    }

    public Sprite get_sp_avatar()
    {
        return this.sp_avatar;
    }

    public bool get_status_robot()
    {
        return this.is_robot;
    }

    public void btn_change_model_robot_and_human()
    {
        GameObject.Find("Games").GetComponent<Games>().carrot.ads.show_ads_Interstitial();
        if (this.is_robot)
        {
            this.is_robot = false;
            GameObject.Find("Games").GetComponent<Games>().play_sound(10);
        }
        else
        {
            GameObject.Find("Games").GetComponent<Games>().play_sound(11);
            this.is_robot = true;
        }
            
        this.update_icon_model_ui();
    }

    private void update_icon_model_ui()
    {
        if (this.is_robot)
            this.img_model_play.sprite = this.sp_mode_robot;
        else
            this.img_model_play.sprite = this.sp_mode_human;
    }

    public void btn_gamepad()
    {
        GameObject.Find("Games").GetComponent<Manager_GamePad>().show_setting_gamepad(this.index_penguin_prefab);
    }

    public void btn_add_heart()
    {
        this.box_msg=GameObject.Find("Games").GetComponent<Games>().carrot.Show_msg("Watch ads to get rewards","Do you want to watch all the ads to get more penguins?",Carrot.Msg_Icon.Question);
        this.box_msg.add_btn_msg("Yes", ()=>act_ads_yes());
        this.box_msg.add_btn_msg("Cancel", () => act_ads_cancel());
    }

    private void act_ads_yes()
    {
        GameObject.Find("Games").GetComponent<Games>().manager_p.set_player_info_add_heart_temp(this);
        GameObject.Find("Games").GetComponent<Games>().carrot.ads.show_ads_Rewarded();
        this.box_msg.close();
    }

    private void act_ads_cancel()
    {
        this.box_msg.close();
    }

    public void act_rewarded_Success()
    {
        this.obj_btn_add_heart.SetActive(false);
        this.img_avatar.sprite = this.sp_avatar;
        this.is_gameover = false;
        this.add_penguin();
    }

    public void gamepad_next_emp()
    {
        this.index_btn_gamepad++;
        if (this.index_btn_gamepad >= this.list_obj_btn_gamepad.Count) this.index_btn_gamepad = 0;
        this.update_ui_gamepad_emp();
    }

    public void gamepad_prev_emp()
    {
        this.index_btn_gamepad--;
        if (this.index_btn_gamepad < 0) this.index_btn_gamepad = this.list_obj_btn_gamepad.Count-1;
        this.update_ui_gamepad_emp();
    }

    private void update_ui_gamepad_emp()
    {
        if (this.index_btn_gamepad != -1)
        {
            for (int i = 0; i < this.list_obj_btn_gamepad.Count; i++)
            {
                Destroy(this.list_obj_btn_gamepad[i].GetComponent<Outline>());
            }

            if (this.list_obj_btn_gamepad[this.index_btn_gamepad] != null)
            {
                this.list_obj_btn_gamepad[this.index_btn_gamepad].AddComponent<Outline>();
                Outline o_line = this.list_obj_btn_gamepad[this.index_btn_gamepad].GetComponent<Outline>();
                o_line.effectColor = Color.red;
                o_line.effectDistance = new Vector2(5f, -5f);
            }
        }

    }

    public void set_list_btn_gamepad(List<GameObject> list_bts)
    {
        if (this.list_obj_btn_gamepad != null)
        {
            for (int i = 0; i < this.list_obj_btn_gamepad.Count; i++)
            {
                if (this.list_obj_btn_gamepad[i] != null) Destroy(this.list_obj_btn_gamepad[i].GetComponent<Outline>());
            }
        }
        this.list_obj_btn_gamepad = list_bts;
        this.index_btn_gamepad = -1;
    }

    public void gamepad_click_emp()
    {
        if (this.index_btn_gamepad != -1)
        {
            Penguin p = this.list_obj_btn_gamepad[this.index_btn_gamepad].GetComponent<Penguin>();
            if (p != null)
                p.click();
            else
                this.list_obj_btn_gamepad[this.index_btn_gamepad].GetComponent<Button>().onClick.Invoke();
        }
    }
}
