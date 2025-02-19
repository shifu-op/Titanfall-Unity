﻿using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterBody))]
[RequireComponent(typeof(CharacterInventory))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterInteraction))]
[RequireComponent(typeof(CharacterView))]
[RequireComponent(typeof(CharacterPresentation))]
class Character : Equipable
{
    public Character()
    {
        _charAsset = null;
        _behaviours = new CharacterBehaviour[0];
        _charInitializer = null;
        _charBody = null;
        _charInventory = null;
        _charMovement = null;
        _charInteraction = null;
        _charView = null;
        _charPresentation = null;
        _controller = null;
        _charMass = 0f;
    }

    //// -------------------------------------------------------------------------------------------
    //// Events Begin
    //// -------------------------------------------------------------------------------------------

    protected virtual void Awake()
    {
        _CollectBehaviours();
        _FetchInitializer();

        OnInit();
        _DispatchCharCreateEvent();

        if (_charInitializer is not null && _charInitializer.destroyOnUse)
        {
            _DestroyInitializer();
        }
    }

    protected virtual void Update()
    {
        _DispatchCharPreUpdateEvent();
        _DispatchCharUpdateEvent();
        _DispatchCharPostUpdateEvent();
    }

    protected virtual void FixedUpdate()
    {
        _DispatchCharFixedUpdateEvent();
    }

    protected virtual void OnInit()
    {
        if (_charInitializer is not null)
        {
            _charAsset = _charInitializer.charAsset;
        }

        if (_charAsset is not null)
        {
            _charMass = _charAsset.characterMass;
        }
    }

    protected virtual void OnDestroy()
    {
        Destroy();
    }

    public virtual void Destroy()
    {
        _DispatchCharDestroyEvent();
    }

    public virtual void Reset()
    {
        _DispatchCharResetEvent();
    }

    public virtual void OnSpawn()
    {
        _DispatchCharSpawnEvent();
    }

    public virtual void OnDespawn()
    {
        _DispatchCharDespawnEvent();
    }

    public virtual void OnPossess(Controller controller)
    {
        _controller = controller;
        _DispatchCharPossessEvent();
    }

    //// -------------------------------------------------------------------------------------------
    //// Events End
    //// -------------------------------------------------------------------------------------------

    protected T _GetBehaviour<T>()
        where T : CharacterBehaviour
    {
        foreach (var behaviour in _behaviours)
        {
            T customBehaviour = behaviour as T;
            if (customBehaviour is not null)
            {
                return customBehaviour;
            }
        }

        return null;
    }

    protected void _CollectBehaviours()
    {
        _behaviours = GetComponents<CharacterBehaviour>();
        _charBody = _GetBehaviour<CharacterBody>();
        _charInventory = _GetBehaviour<CharacterInventory>();
        _charMovement = _GetBehaviour<CharacterMovement>();
        _charInteraction = _GetBehaviour<CharacterInteraction>();
        _charView = _GetBehaviour<CharacterView>();
        _charPresentation = _GetBehaviour<CharacterPresentation>();
    }

    protected void _FetchInitializer()
    {
        _charInitializer = GetComponent<CharacterInitializer>();
    }

    protected void _DestroyInitializer()
    {
        Destroy(_charInitializer);
    }

    protected void _DispatchCharCreateEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterCreate(this, _charInitializer);
        }
    }

    protected void _DispatchCharSpawnEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterSpawn();
        }
    }

    protected void _DispatchCharPreUpdateEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterPreUpdate();
        }
    }

    protected void _DispatchCharUpdateEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterUpdate();
        }
    }

    protected void _DispatchCharPostUpdateEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterPostUpdate();
        }
    }

    protected void _DispatchCharFixedUpdateEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterFixedUpdate();
        }
    }

    protected void _DispatchCharDeadEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterDead();
        }
    }

    protected void _DispatchCharDespawnEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterDespawn();
        }
    }

    protected void _DispatchCharDestroyEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterDestroy();
        }
    }

    protected void _DispatchCharPossessEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterPossess(_controller);
        }
    }

    protected void _DispatchCharResetEvent()
    {
        foreach (var behaviour in _behaviours)
        {
            behaviour.OnCharacterReset();
        }
    }

    //// -------------------------------------------------------------------------------------------
    //// Properties and Fields
    //// -------------------------------------------------------------------------------------------

    public CharacterAsset charAsset => _charAsset;
    public CharacterInitializer charInitializer => _charInitializer;
    public CharacterBody charBody => _charBody;
    public CharacterInventory charInventory => _charInventory;
    public CharacterMovement charMovement => _charMovement;
    public CharacterInteraction charInteraction => _charInteraction;
    public CharacterView charView => _charView;
    public CharacterPresentation charAnimation => _charPresentation;
    public Quaternion rotation => transform.rotation;
    public Vector3 forward => transform.forward;
    public Vector3 back => -transform.forward;
    public Vector3 right => transform.right;
    public Vector3 left => -transform.right;
    public Vector3 up => transform.up;
    public Vector3 down => -transform.up;
    public float mass => _charMass;
    public float scaledMass => transform.lossyScale.magnitude * _charMass;
    public Controller controller => _controller;

    protected CharacterAsset _charAsset;
    protected Controller _controller;
    protected CharacterBehaviour[] _behaviours;
    protected CharacterInitializer _charInitializer;
    protected CharacterBody _charBody;
    protected CharacterInventory _charInventory;
    protected CharacterMovement _charMovement;
    protected CharacterInteraction _charInteraction;
    protected CharacterView _charView;
    protected CharacterPresentation _charPresentation;
    protected float _charMass;
}
