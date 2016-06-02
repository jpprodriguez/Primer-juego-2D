﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HUD : MonoBehaviour {

    public Sprite[] HeartSprites;
    public Image HeartUI;
    private Player player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        HeartUI.sprite = HeartSprites[player.lifes];
	}
}
