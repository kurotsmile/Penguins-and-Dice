using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Tray_Chess_Type {none,step,start,dice,penguin}
public class Tray_Chess : MonoBehaviour
{
    public Image img_border;
    public Transform tr_body;
    public GameObject obj_img_focus;
    public GameObject obj_img_foot;
    public GameObject obj_img_attack;
    public GameObject obj_img_dice;
    public GameObject obj_img_penguin;
    public GameObject obj_img_move;
    public Tray_Chess_Type type = Tray_Chess_Type.none;
    public int index_num;
    public Animator ani;

    private bool is_focus = false;
    private List<Tray_Chess> list_tray_move_foot;
    private Penguin penguin_cur;

    public void on_load(Color color_block)
    {
        this.ani.Play("tray_nomal");
        this.img_border.color = color_block;
        this.obj_img_focus.SetActive(false);
        this.obj_img_foot.SetActive(false);
        this.obj_img_attack.SetActive(false);
        this.obj_img_dice.SetActive(false);
        this.obj_img_move.SetActive(false);
        this.obj_img_penguin.SetActive(false);

        if (this.type == Tray_Chess_Type.start)
        {
            this.obj_img_move.SetActive(true);
        }
        else if (this.type == Tray_Chess_Type.penguin)
        {
            this.obj_img_penguin.SetActive(true);
        } 
        else if (this.type == Tray_Chess_Type.dice)
        {
            this.obj_img_dice.SetActive(true);
        }
            
    }

    public Tray_Chess_Type get_type(){
        return this.type;
    }

    public void on_focus()
    {
        this.ani.Play("tray_focus");
        this.obj_img_focus.SetActive(true);
        this.is_focus = true;
    }

    public void on_select(string id_color)
    {
        this.ani.Play("tray_select_"+id_color);
        this.obj_img_focus.SetActive(true);
    }

    public void check_attack(Penguin p)
    {
        if (this.penguin_cur != null)
        {
            if (this.penguin_cur.id_name != p.id_name)
            {
                this.obj_img_attack.SetActive(true);
                this.obj_img_foot.SetActive(false);
            }
        }
    }

    public void un_focus()
    {
        this.ani.Play("tray_nomal");
        this.obj_img_focus.SetActive(false);
        this.is_focus = false;
        this.obj_img_foot.SetActive(false);
        this.obj_img_attack.SetActive(false);
    }

    public void on_foot(List<Tray_Chess> l)
    {
        this.ani.Play("tray_focus");
        this.is_focus = true;
        this.list_tray_move_foot = l;
        this.obj_img_foot.SetActive(true);
        this.obj_img_attack.SetActive(false);
    }

    public void click()
    {
        if (this.is_focus)
            GameObject.Find("Games").GetComponent<Games>().manager_p.move_to(this);
        else
            GameObject.Find("Games").GetComponent<Games>().play_sound(4);
    }

    public List<Tray_Chess> get_list_tray_move()
    {
        return this.list_tray_move_foot;
    }

    public void set_penguin_cur(Penguin p)
    {
        this.penguin_cur = p;
    }

    public Penguin get_penguin()
    {
        return this.penguin_cur;
    }
}
