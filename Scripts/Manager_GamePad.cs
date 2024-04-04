using Carrot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_GamePad : MonoBehaviour
{

    public Carrot.Carrot carrot;
    public List<GameObject> list_btn_home;
    public List<GameObject> list_btn_play;

    private Player_Info[] p_info = new Player_Info[4];

    private bool is_home=false;

    public void on_load()
    {

        Carrot_Gamepad gamepad1 = this.carrot.game.create_gamepad("p1");
        KeyCode[] key_code1 = new KeyCode[10];
        key_code1[0] = KeyCode.None;
        key_code1[1] = KeyCode.None;
        key_code1[2] = KeyCode.None;
        key_code1[3] = KeyCode.None;
        key_code1[4] = KeyCode.None;
        key_code1[5] = KeyCode.None;
        key_code1[6] = KeyCode.None;
        key_code1[7] = KeyCode.None;
        key_code1[8] = KeyCode.None;
        key_code1[9] = KeyCode.None;
        gamepad1.set_KeyCode_default(key_code1);
        gamepad1.set_gamepad_keydown_left(() => this.act_next_emp(0));
        gamepad1.set_gamepad_keydown_right(() => this.act_prev_emp(0));
        gamepad1.set_gamepad_keydown_down(() => this.act_prev_emp(0));
        gamepad1.set_gamepad_keydown_up(() => this.act_next_emp(0));
        gamepad1.set_gamepad_keydown_start(() => this.act_enter(0));
        gamepad1.set_gamepad_keydown_select(() => this.act_enter(0));
        gamepad1.set_gamepad_keydown_a(this.player_1_key_a);
       


        Carrot_Gamepad gamepad2 = this.carrot.game.create_gamepad("p2");
        KeyCode[] key_code2 = new KeyCode[10];
        key_code2[0] = KeyCode.None;
        key_code2[1] = KeyCode.None;
        key_code2[2] = KeyCode.None;
        key_code2[3] = KeyCode.None;
        key_code2[4] = KeyCode.None;
        key_code2[5] = KeyCode.None;
        key_code2[6] = KeyCode.None;
        key_code2[7] = KeyCode.None;
        key_code2[8] = KeyCode.None;
        key_code2[9] = KeyCode.None;
        gamepad2.set_gamepad_keydown_left(() => this.act_next_emp(1));
        gamepad2.set_gamepad_keydown_right(() => this.act_prev_emp(1));
        gamepad2.set_gamepad_keydown_down(() => this.act_prev_emp(1));
        gamepad2.set_gamepad_keydown_up(() => this.act_next_emp(1));
        gamepad2.set_gamepad_keydown_start(() => this.act_enter(1));
        gamepad2.set_gamepad_keydown_select(() => this.act_enter(1));
        gamepad2.set_gamepad_keydown_a(this.player_2_key_a);
        gamepad2.set_KeyCode_default(key_code2);

        Carrot_Gamepad gamepad3 = this.carrot.game.create_gamepad("p3");
        KeyCode[] key_code3 = new KeyCode[10];
        key_code3[0] = KeyCode.None;
        key_code3[1] = KeyCode.None;
        key_code3[2] = KeyCode.None;
        key_code3[3] = KeyCode.None;
        key_code3[4] = KeyCode.None;
        key_code3[5] = KeyCode.None;
        key_code3[6] = KeyCode.None;
        key_code3[7] = KeyCode.None;
        key_code3[8] = KeyCode.None;
        key_code3[9] = KeyCode.None;
        gamepad3.set_gamepad_keydown_left(()=>this.act_next_emp(2));
        gamepad3.set_gamepad_keydown_right(() => this.act_prev_emp(2));
        gamepad3.set_gamepad_keydown_down(() => this.act_prev_emp(2));
        gamepad3.set_gamepad_keydown_up(() => this.act_next_emp(2));
        gamepad3.set_gamepad_keydown_start(()=>this.act_enter(2));
        gamepad3.set_gamepad_keydown_select(() => this.act_enter(2));
        gamepad3.set_gamepad_keydown_a(this.player_3_key_a);
        gamepad3.set_KeyCode_default(key_code3);


        Carrot_Gamepad gamepad4 = this.carrot.game.create_gamepad("p4");
        KeyCode[] key_code4 = new KeyCode[10];
        key_code4[0] = KeyCode.None;
        key_code4[1] = KeyCode.None;
        key_code4[2] = KeyCode.None;
        key_code4[3] = KeyCode.None;
        key_code4[4] = KeyCode.None;
        key_code4[5] = KeyCode.None;
        key_code4[6] = KeyCode.None;
        key_code4[7] = KeyCode.None;
        key_code4[8] = KeyCode.None;
        key_code4[9] = KeyCode.None;
        gamepad4.set_gamepad_keydown_left(() => this.act_next_emp(3));
        gamepad4.set_gamepad_keydown_right(() => this.act_prev_emp(3));
        gamepad4.set_gamepad_keydown_down(() => this.act_prev_emp(3));
        gamepad4.set_gamepad_keydown_up(() => this.act_next_emp(3));
        gamepad4.set_gamepad_keydown_start(() => this.act_enter(3));
        gamepad4.set_gamepad_keydown_select(() => this.act_enter(3));
        gamepad4.set_gamepad_keydown_a(this.player_4_key_a);
        gamepad4.set_KeyCode_default(key_code4);

        this.on_home();
    }

    public void on_home()
    {
        this.carrot.game.set_list_button_gamepad_console(this.list_btn_home, -100);
        this.is_home = true;
    }


    private void act_enter(int index_p)
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_enter_console();
        else
            this.p_info[index_p].gamepad_click_emp();
    }

    private void act_next_emp(int index_p)
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_down_console();
        else
           if(this.p_info[index_p]!=null) this.p_info[index_p].gamepad_next_emp();
    }

    private void act_prev_emp(int index_p)
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_up_console();
        else
            if (this.p_info[index_p] != null) this.p_info[index_p].gamepad_prev_emp();
    }

    public void show_setting_gamepad(int index_pad)
    {
        this.carrot.game.get_list_gamepad()[index_pad].show_setting_gamepad();
    }

    private void player_1_key_a()
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_enter_console();
        else
            if (this.p_info[0] != null) this.p_info[0].btn_change_model_robot_and_human();
    }

    private void player_2_key_a()
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_enter_console();
        else
            if (this.p_info[1] != null) this.p_info[1].btn_change_model_robot_and_human();
    }

    private void player_3_key_a()
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_enter_console();
        else
            if (this.p_info[2] != null) this.p_info[2].btn_change_model_robot_and_human();
    }

    private void player_4_key_a()
    {
        if (this.is_home)
            this.carrot.game.gamepad_keydown_enter_console();
        else
            if (this.p_info[3] != null) this.p_info[3].btn_change_model_robot_and_human();

    }

    public void set_p_info(int index_player_in_gamepad,Player_Info p_info)
    {
        this.p_info[index_player_in_gamepad] = p_info;
    }

    public void on_play()
    {
        this.is_home = false;
        this.carrot.game.set_enable_gamepad_console(false);
    }
}
