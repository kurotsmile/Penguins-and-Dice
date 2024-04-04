using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    public Sprite icon;
    public Color32 color_penguin;
    public Animator ani;
    public string id_name;

    private Player_Info player_cur;
    private List<Tray_Chess> path_move;
    private Tray_Chess tray_chess_cur;
    private float speed_move = 3.2f;
    private Transform tr_tray_home;

    private int currentPoint_move = 0;
    private bool is_move = false;

    private bool is_focus = false;
    private int index_tray_play = 0;
    private bool is_moved = false;
    private bool is_moved_go_home = false;
    private bool is_robot = false;

    public void on_load(int index_tr_play,Transform tr_home,Player_Info player)
    {
        this.is_moved = false;
        this.is_moved_go_home = false;
        this.index_tray_play = index_tr_play;
        this.tr_tray_home = tr_home;
        this.player_cur = player;
        this.is_robot = player.get_status_robot();
    }

    public int get_index_tray_play()
    {
        return this.index_tray_play;
    }

    public void set_tray_cur(Tray_Chess tray_new)
    {
        this.tray_chess_cur = tray_new;
    }

    public void set_index_tray_play(int tr_index)
    {
        this.index_tray_play = tr_index;
    }

    public void click()
    {
        if (this.is_focus==true&&this.is_robot==false)
        {
            this.act_click();
        }
        else
        {
            GameObject.Find("Games").GetComponent<Games>().play_sound(4);
        }
    }

    public void act_click()
    {
        if (tray_chess_cur != null)
        {
            this.tray_chess_cur.set_penguin_cur(null);
            this.tray_chess_cur = null;
        }
        GameObject.Find("Games").GetComponent<Games>().manager_p.select_penguin(this);
        this.ani.Play("penguin_" + this.id_name + "_click");
    }

    public void on_waiting()
    {
        this.is_focus = false;
        this.ani.Play("penguin_"+this.id_name);
    }

    public void on_focus()
    {
        this.is_focus = true;
        this.ani.Play("penguin_" + this.id_name+ "_focus");
    }

    public void on_select()
    {
        this.ani.Play("penguin_" + this.id_name + "_focus");
    }

    public void on_die()
    {
        GameObject.Find("Games").GetComponent<Games>().ani.play_punched();
        this.player_cur.on_punched();
        this.ani.Play("penguin_" + this.id_name + "_dice");
        Destroy(this.gameObject, 1f);
        this.player_cur.check_gameover();
    }

    public void act_die()
    {
        Debug.Log("act_die");
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (this.is_move)
        {
            if (this.currentPoint_move >= this.path_move.Count)
            {

                Tray_Chess tr_move_end = this.path_move[this.path_move.Count-1];
                tr_move_end.set_penguin_cur(this);

                this.is_moved = true;
                this.is_move = false;

                this.transform.SetParent(tray_chess_cur.tr_body);
                this.transform.position = tray_chess_cur.transform.position;

                if (tr_move_end.get_type() == Tray_Chess_Type.penguin) this.player_cur.add_penguin();

                if(tr_move_end.get_type()==Tray_Chess_Type.dice)
                    GameObject.Find("Games").GetComponent<Games>().manager_p.next_player(true);
                else
                    GameObject.Find("Games").GetComponent<Games>().manager_p.next_player();
                return;
            }

            Tray_Chess tr_move = this.path_move[this.currentPoint_move];
            Vector3 target = tr_move.transform.position;
            transform.position = Vector3.MoveTowards(this.transform.position, target, this.speed_move * Time.deltaTime);

            if (transform.position == target)
            {
                this.check_step_path(tr_move);
                this.currentPoint_move++;
            }
        }

        if (this.is_moved_go_home)
        {
            Vector3 target = this.tr_tray_home.position;
            transform.position = Vector3.MoveTowards(this.transform.position, target, this.speed_move * Time.deltaTime);
            if (transform.position == target)
            {
                GameObject.Find("Games").GetComponent<Games>().play_sound(6);
                this.transform.SetParent(this.tr_tray_home);
                this.transform.localPosition = Vector3.zero;
                this.is_moved = false;
                this.is_moved_go_home = false;
                this.tray_chess_cur.set_penguin_cur(null);
                this.tray_chess_cur = null;
            }
        }

    }

    private void check_step_path(Tray_Chess tr_move)
    {
        Penguin p_attack = tr_move.get_penguin();
        if (p_attack != null)
        {
            if (p_attack.id_name != this.id_name)
            {
                p_attack.on_die();
                GameObject.Find("Games").GetComponent<Games>().play_sound(5);
                GameObject.Find("Games").GetComponent<Games>().carrot.play_vibrate();
            }
            else
            {
                p_attack.on_go_home();
            }
        }
    }

    [ContextMenu("on_go_home")]
    public void on_go_home()
    {
        this.transform.SetParent(transform.root.transform);
        this.is_moved_go_home = true;
    }

    public void set_path_move(List<Tray_Chess> l)
    {
        this.ani.Play("penguin_" + this.id_name + "_move");
        this.transform.SetParent(transform.root.transform);
        this.path_move = l;
        this.currentPoint_move = 0;
        this.is_move = true;
    }

    public bool get_status_moved()
    {
        return this.is_moved;
    }
}
