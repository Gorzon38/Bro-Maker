﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TonyBrotana_LoadMod;
using BroMaker;
public class TonyBrotana : Maker._BroBase
{
    protected override void Start()
    {
        Main.Log("Spawning " + typeof(TonyBrotana));
        try
        {
            new GameObject(typeof(TonyBrotana).FullName, typeof(TonyBrotana));
        }
        catch(Exception ex)
        {
           Main.Log(ex);
        }
        base.Start();
    }

    public void Setup(SpriteSM attachSprite, Player attachPlayer, int attachplayerNum, SoundHolder attachSoundHolder)
    {
        sprite = attachSprite;
        player = attachPlayer;
        playerNum = attachplayerNum;
        soundHolder = attachSoundHolder;

        this.SpecialAmmo = 5;
        this.originalSpecialAmmo = 5;
        this.health = 1;
     //   this.player.hud.avatar = ;
    }
    protected override void Awake()
    {
        this.isHero = true;
        base.Awake();
    }

    // Reposition of the gun if is not at the place we want.
    protected override void SetGunPosition(float xOffset, float yOffset)
    {
        this.gunSprite.transform.localPosition = new Vector3(xOffset, yOffset, -1f);
    }

    protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed) // What the gun does when you shoot.
    {
        this.gunFrame = 3;
        this.SetGunSprite(this.gunFrame, 1);
        EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.15f, ySpeed * 0.15f, base.transform);
        ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, base.playerNum);
    }

    protected override void UseSpecial()
    {
        try
        {
            Main.Log("Special is use");
            if (this.SpecialAmmo > 0)
            {
		        this.PlayThrowLightSound(0.4f);
                this.SpecialAmmo--;
                if (base.IsMine)
                {
                    if (this.projRocket == null)
                        Main.Log("Rocket is null, how the turntable..");
                    float x = base.X + base.transform.localScale.x * 12f;
                    float y = base.Y + 9f;
                    float xSpeed = base.transform.localScale.x * (float)(150 + ((!Demonstration.bulletsAreFast) ? 0 : 150));
                    float ySpeed = 0f;

                    this.gunFrame = 3;
                    this.SetGunSprite(this.gunFrame, 0);
                    EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
                    ProjectileController.SpawnProjectileOverNetwork(this.projRocket, this, x, y, xSpeed, ySpeed, false, base.playerNum, false, false, 0f);
                }
                this.fireDelay = 0.6f;
                this.PlayAttackSound();
                Map.DisturbWildLife(base.X, base.Y, 80f, base.playerNum);
            }
            else
            {
                HeroController.FlashSpecialAmmo(base.playerNum);
            }
        }
        catch (Exception ex)
        {
            Main.Log(ex);
            //base.UseSpecial();
        }

    }

    public Material materialNormal;
    public Material materialArmless;

    public Projectile projRocket;
}

