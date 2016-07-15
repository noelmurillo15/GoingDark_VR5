﻿using UnityEngine;

public class HyperdriveSystem : ShipSystem
{

    #region Properties
    private PlayerMovement stats;
    private GameObject m_playerMove;
    private GameObject particles;
        #endregion

    // Use this for initialization
    void Start () {
        maxCooldown = 40f;

        m_playerMove = GameObject.Find("Player");
        particles = transform.GetChild(0).gameObject;
        stats = m_playerMove.GetComponent<PlayerMovement>();
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        if (Activated) 
            InitializeHyperdriveSequence();
    }

    #region Private Methods
    public void InitializeHyperdriveSequence() {
        DeActivate();
        stats.MoveData.Boost = 5.0f;
        stats.MoveData.Acceleration = 100f;
        stats.boostActive = true;
        particles.SetActive(true);
        Invoke("RevertBoost", 10f);
    }

    public void RevertBoost()
    {
        stats.MoveData.Boost = 1.0f;
        stats.MoveData.Acceleration = 20f;
        stats.boostActive = false;
        particles.SetActive(false);
    }
    #endregion
}
