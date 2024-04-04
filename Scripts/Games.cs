using UnityEngine;
using UnityEngine.UI;

public class Games : MonoBehaviour
{
    [Header("Game Obj")]
    public Carrot.Carrot carrot;
    public Manager_Penguins manager_p;
    public Manager_GamePad gamepad;
    public Obj_Anim ani;

    [Header("Ui Obj")]
    public Text txt_ui_scores;
    public GameObject panel_home;
    public GameObject panel_play;

    [Header("Sounds")]
    public AudioClip AudioClip_Click;
    public AudioSource[] sound;
    private int ui_scores = 0;

    void Start()
    {
        this.ani.play_load();
        this.carrot.Load_Carrot(this.check_exit_app);
        this.carrot.change_sound_click(this.AudioClip_Click);
        this.carrot.game.load_bk_music(this.sound[0]);

        this.carrot.ads.onRewardedSuccess+=this.manager_p.on_onRewardedSuccess;
        this.manager_p.panel_done.SetActive(false);
        this.manager_p.dice.on_load();
        this.panel_home.SetActive(true);
        this.panel_play.SetActive(false);

        this.ui_scores = PlayerPrefs.GetInt("ui_scores", 0);
        this.update_ui_scores();
        this.gamepad.on_load();
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_play_game(int type)
    {
        this.carrot.ads.show_ads_Interstitial();
        this.ani.play_play();
        this.carrot.ads.Destroy_Banner_Ad();
        this.panel_home.SetActive(false);
        this.panel_play.SetActive(true);
        this.manager_p.play(type);
        this.carrot.play_sound_click();
        this.gamepad.on_play();
    }

    public void btn_user()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.user.show_login();
    }

    public void btn_rate()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.show_rate();
    }

    public void btn_back_home()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.ads.create_banner_ads();
        this.panel_play.SetActive(false);
        this.panel_home.SetActive(true);
        this.carrot.play_sound_click();
        this.manager_p.back_menu();
        this.ani.play_load();
        this.gamepad.on_home();
    }

    public void btn_share()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.show_share();
    }

    public void btn_app_carrot()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.show_list_carrot_app();
    }

    public void btn_setting()
    {
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(this.act_close_setting);
    }

    private void act_close_setting()
    {
        if (this.panel_home.activeInHierarchy)
            this.gamepad.on_home();
        else
            this.gamepad.on_play();
    }

    public void btn_ranks()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.game.Show_List_Top_player();
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    private void update_ui_scores()
    {
        this.txt_ui_scores.text = this.ui_scores.ToString();
    }

    public void add_scores()
    {
        this.ui_scores++;
        PlayerPrefs.SetInt("ui_scores", this.ui_scores);
        this.carrot.game.update_scores_player(this.ui_scores);
        this.update_ui_scores();
    }
}
