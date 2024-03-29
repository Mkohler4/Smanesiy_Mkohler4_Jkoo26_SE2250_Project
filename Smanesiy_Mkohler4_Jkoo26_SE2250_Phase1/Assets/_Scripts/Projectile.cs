﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    //Declare variables
    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;

    // This public property masks the field _type and takes action when it is set
    public WeaponType type
    {
        get
        {
            return (_type);

        }
        set
        {
            SetType(value);
        }
    }
    //Get components
    void Awake()
    {
        //Get components
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
  
    // Update is called once per frame
    void Update()
    {
        //Delete projectile if it goes off the screen
        if (_bndCheck.offUp)
        {
            Destroy(gameObject);
        }
        else if (_bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }
}
