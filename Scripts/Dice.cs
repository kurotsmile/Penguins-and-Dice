using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public Animator ani;

    public Sprite[] sp_dice;
    public Image img_dice;
    public Image img_dice_bk;
    public Image img_avatar;
    public Image img_avatar2;

    private int number_dice = 0;
    public void on_load()
    {
        this.gameObject.SetActive(false);
    }

    public void show_dice(Sprite sp_avatar,Color32 color_show)
    {
        this.ani.enabled = true;
        this.gameObject.SetActive(true);
        this.number_dice= Random.Range(0, this.sp_dice.Length);
        this.img_dice_bk.color = color_show;
        this.img_dice.sprite = this.sp_dice[this.number_dice];
        this.img_avatar.sprite = sp_avatar;
        this.img_avatar2.sprite = sp_avatar;
    }

    public void open_dice()
    {
        GameObject.Find("Games").GetComponent<Games>().play_sound(1);
    }

    public void stop_ani()
    {
        GameObject.Find("Games").GetComponent<Games>().manager_p.done_dice(this.img_dice.sprite, number_dice);
        this.ani.enabled = false;
        this.gameObject.SetActive(false);
    }

    public void on_back_menu()
    {
        this.ani.enabled = false;
        this.gameObject.SetActive(false);
    }
}
