using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Anim : MonoBehaviour
{

    public Animator ani;

    public void play_load()
    {
        this.ani.Play("game_ani_load");
    }

    public void play_play()
    {
        this.ani.Play("game_ani_play");
    }

    public void play_done()
    {
        this.ani.Play("game_ani_done");
    }

    public void play_punched()
    {
        this.ani.Play("game_ani_punched");
    }

    public void stop_anim()
    {
        this.ani.Play("game_ani");
    }
}
