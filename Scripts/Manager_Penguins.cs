using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Penguins : MonoBehaviour
{
    [Header("Main Object")]
    public Dice dice;
    public GameObject player_prefab;
    public GameObject info_done_prefab;
    public GameObject[] penguin_prefab;
    public Tray_Chess[] tray_check;
    public Tray_Chess[] tray_checss_play;

    [Header("Ui Object")]
    public GameObject panel_dice;
    public GameObject panel_done;
    public Image img_dice_done;
    public Transform[] tr_area_player;
    public Transform[] tray_done_info;
    public Area_create_pengin[] area_create_penguins;

    [Header("Asset Color")]
    public Color32 color_none;
    public Color32 color_step;
    private int type_mode;
    private List<Player_Info> list_player;

    private int index_player_focus;
    private Transform[] tr_path_move_penguin;
    private bool is_play = false;

    private Player_Info player_add_heart_temp;

    public void play(int type)
    {
        this.type_mode = type;

        this.act_reset();
        for(int i=0;i<this.area_create_penguins.Length;i++) this.area_create_penguins[i].gameObject.SetActive(false);

        if (type == 0)
        {
            this.create_player("Player 1",0,0,0,true);

            if (this.GetComponent<Games>().carrot.user.get_id_user_login() != "")
            {
                this.create_player(this.GetComponent<Games>().carrot.user.get_data_user_login("name"), 1, 1, 13, false);
            }
            else
            {
                this.create_player("Player 2", 1, 1, 13, false);
            }
            
        } else if (type == 1)
        {
            this.create_player("Player 1",2, 2, 6,false);
            this.create_player("Player 2",3, 3, 19,false);
        } else if (type == 2)
        {
            this.create_player("Player 1",0,0,0,false);
            this.create_player("Player 2",1,1,13,false);
            this.create_player("Player 3",2,2,6,false);
            this.create_player("Player 4",3,3,19,false);
        }
        else if (type == 3)
        {
            this.create_player("Player 1",0, 0, 0,true);
            this.create_player("Player 2",1, 1, 13,false);
            this.create_player("Player 3",2, 2, 6,true);
            this.create_player("Player 4",3, 3, 19,true);
        }

        for(int i = 0; i < this.tray_check.Length; i++)
        {
            if(this.tray_check[i].get_type()==Tray_Chess_Type.none)
                this.tray_check[i].on_load(this.color_none);
            else
                this.tray_check[i].on_load(this.color_step);
        }

        for(int i = 0; i < this.tray_checss_play.Length; i++)
        {
            this.tray_checss_play[i].index_num = i;
        }

        this.is_play = true;
        this.GetComponent<Games>().carrot.delay_function(1f, this.check_player_first);
    }

    public void create_player(string s_name,int index_area,int index_create_penguin,int index_tray_start,bool robot)
    {
        GameObject obj_player = Instantiate(this.player_prefab);
        this.area_create_penguins[index_create_penguin].gameObject.SetActive(true);
        obj_player.transform.SetParent(this.tr_area_player[index_area]);
        obj_player.transform.localPosition = Vector3.zero;
        obj_player.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_player.transform.localRotation = Quaternion.Euler(0, 0,0);

        Player_Info p_info = obj_player.GetComponent<Player_Info>();
        p_info.set_name(s_name);
        p_info.on_load(index_tray_start,this.area_create_penguins[index_create_penguin], this.penguin_prefab[index_create_penguin],robot, index_create_penguin);
        this.GetComponent<Games>().gamepad.set_p_info(index_create_penguin, p_info);
        this.list_player.Add(p_info);
    }

    private void check_player_first()
    {
        if (this.is_play)
        {
            this.index_player_focus = Random.Range(0, this.list_player.Count);
            this.act_player_cur();
        }
    }

    public void next_player(bool is_reload=false)
    {
        for(int i = 0; i < this.list_player.Count; i++)
        {
            this.list_player[i].un_focus();
        }

        this.panel_dice.SetActive(false);
        if(is_reload==false) this.index_player_focus++;
        if (this.index_player_focus >= this.list_player.Count) this.index_player_focus = 0;
        this.act_player_cur();
    }

    private void act_player_cur()
    {
        if (this.list_player[index_player_focus].get_status_gameover())
        {
            this.next_player();
        }
        else
        {
            Player_Info player_play = this.list_player[index_player_focus];
            player_play.on_focus();
            if (player_play.get_status_robot())
            {
                this.GetComponent<Games>().carrot.delay_function(1f, act_player_robot);
            }
        }
    }

    private void act_player_robot()
    {
        if(this.is_play) this.get_player_cur().on_play_robot();
    }

    public void back_menu()
    {
        this.dice.on_back_menu();
        this.act_reset();
        this.is_play = false;
    }

    private void act_reset()
    {
        this.panel_dice.SetActive(false);
        this.panel_done.SetActive(false);
        if (this.list_player != null)
        {
            for (int i = 0; i < this.list_player.Count; i++)
            {
                this.list_player[i].clear_all_penguin();
                Destroy(this.list_player[i].gameObject);
            }
        }
        this.list_player = new List<Player_Info>();
    }

    public void on_reset()
    {
        this.GetComponent<Games>().carrot.ads.show_ads_Interstitial();
        this.play(this.type_mode);
        this.GetComponent<Games>().play_sound(8);
    }

    public int get_mode()
    {
        return this.type_mode;
    }

    public void select_penguin(Penguin p)
    {
        this.un_focus_all_tray();
        this.GetComponent<Games>().play_sound(2);
        this.dice.show_dice(p.icon,p.color_penguin);
        this.get_player_cur().set_penguin_select(p);
        this.panel_dice.SetActive(false);
    }

    public void done_dice(Sprite sp_dice,int dice_num)
    {
        this.img_dice_done.sprite = sp_dice;
        this.panel_dice.SetActive(true);

        for(int i = 0; i < this.list_player.Count; i++) this.list_player[i].on_all_penguin_nomal();

        Penguin p= this.get_player_cur().get_penguin();
        p.on_select();
        int index_cur = this.get_player_cur().get_index_cur();
        Tray_Chess[] l_c;
        if (p.get_status_moved())
            l_c = this.GetAdjacentTray(index_cur,dice_num+1);
        else
            l_c = this.GetAdjacentTray(index_cur, dice_num);

        List<Tray_Chess> l_c_left = new List<Tray_Chess>();
        List<Tray_Chess> l_c_right= new List<Tray_Chess>();
        this.tr_path_move_penguin = new Transform[l_c.Length];
        for (int i = 0; i < l_c.Length; i++)
        {
            if(i<= (l_c.Length/2)) l_c_left.Add(l_c[i]); 
            if(i>= (l_c.Length/2)) l_c_right.Add(l_c[i]);

            l_c[i].on_select(p.id_name);
            l_c[i].check_attack(p);
            this.tr_path_move_penguin[i] = l_c[i].transform;
        }

        l_c_left.Reverse();
        l_c[0].on_foot(l_c_left);
        l_c[l_c.Length-1].on_foot(l_c_right);

        if (this.get_player_cur().get_status_robot())
        {
            if (Random.Range(0, 2) == 0)
                l_c[0].click();
            else
                l_c[l_c.Length - 1].click();
        }
        else
        {
            List<GameObject> list_btn_gampead = new List<GameObject>();
            list_btn_gampead.Add(l_c[0].gameObject);
            list_btn_gampead.Add(l_c[l_c.Length - 1].gameObject);
            this.get_player_cur().set_list_btn_gamepad(list_btn_gampead);
        }
    }

    public Tray_Chess[] GetAdjacentTray(int index,int n)
    {
        Tray_Chess[] result = new Tray_Chess[2 * n + 1];

        for (int i = 0; i < 2 * n + 1; i++)
        {
            int currIndex = (index + i - n + tray_checss_play.Length) % tray_checss_play.Length;
            result[i] = tray_checss_play[currIndex];
        }

        return result;
    }

    public void un_focus_all_tray()
    {
        for(int i = 0; i < this.tray_checss_play.Length; i++)
        {
            this.tray_checss_play[i].un_focus();
        }
    }

    public Player_Info get_player_cur()
    {
        return this.list_player[this.index_player_focus];
    }

    public void move_to(Tray_Chess tray)
    {
        this.GetComponent<Games>().play_sound(3);
        Penguin p_sel = this.get_player_cur().get_penguin();
        p_sel.set_path_move(tray.get_list_tray_move());
        p_sel.set_index_tray_play(tray.index_num);
        p_sel.set_tray_cur(tray);

        for (int i = 0; i < this.tray_checss_play.Length; i++)
        {
            this.tray_checss_play[i].un_focus();
        }
    }

    public void check_done()
    {
        int count_over = 0;
        for(int i = 0; i < this.list_player.Count; i++)
        {
            if (this.list_player[i].get_status_gameover()) count_over++;
        }

        if (count_over>=this.list_player.Count-2) this.show_done();
    }

    [ContextMenu("Show Done")]
    public void show_done()
    {
        this.GetComponent<Games>().carrot.ads.show_ads_Interstitial();
        this.is_play = false;
        this.panel_done.SetActive(true);
        List<Info_done> list_info_done = new List<Info_done>();
        for (int i = 0; i < this.list_player.Count; i++)
        {
            list_info_done.Add(this.create_done_info(i));
            if (this.list_player[i].get_status_gameover())
                list_info_done[i].txt_msg.text = "You lose!";
            else
                list_info_done[i].txt_msg.text = "You win!";
        }

        if (this.type_mode == 0)
        {
            if (this.list_player[1].get_status_gameover() != false) this.GetComponent<Games>().add_scores();
        }

        this.GetComponent<Games>().play_sound(7);
        this.GetComponent<Games>().ani.play_done();
    }

    private Info_done create_done_info(int index_player)
    {
        Player_Info p_player = this.list_player[index_player];
        GameObject obj_done = Instantiate(this.info_done_prefab);
        obj_done.transform.SetParent(this.tray_done_info[index_player]);
        obj_done.transform.localPosition = Vector3.zero;
        obj_done.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_done.transform.localRotation = Quaternion.identity;

        Info_done info_done= obj_done.GetComponent<Info_done>();
        info_done.onload(p_player.get_sp_avatar(), p_player.txt_name_player.text, "Win");
        return info_done;
    }

    public void set_player_info_add_heart_temp(Player_Info p)
    {
        this.player_add_heart_temp = p;
    }

    public void on_onRewardedSuccess()
    {
        if (this.player_add_heart_temp != null)
        {
            this.player_add_heart_temp.act_rewarded_Success();
            this.player_add_heart_temp = null;
        }
    }
}
