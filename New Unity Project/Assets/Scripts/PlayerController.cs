using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    
    private GameController _gameController;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

    private void OnEnable()
    {
        // inicializacao de variavel
        _gameController = new GameController();
        
        // referencias dos componetes no mesmo objeto da unity
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        
        //Referencia para a camera main guardada na classe camera 
        _mainCamera = Camera.main;
        
        
        // Atribuindo ao delegate do aca
        _playerInput.onActionTriggered += OnActionTiggered;
        


    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= OnActionTiggered;
    }

    private void OnActionTiggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name.CompareTo(_gameController.Gameplay.Movement.name)==0 )
        {
            // atribuir ao moveInput o valor proveniente do input do jogador como Vector2
            _moveInput = obj.ReadValue<Vector2>();
        }
    }

    private void move()
    {
        // calcula o movimento no eixo da camera para movimento frente/ tras 
        Vector3 moveVertical = _mainCamera.transform.forward * _moveInput.y;
        // calcula o movimento no eixo da camera para movimento esquerda/direita
        Vector3 moveHorizontal = _mainCamera.transform.right * _moveInput.x;
        
        // Adiciona a forca no objeto pelo rigidBody, com intencidade dada por moveSpeed
        _rigidbody.AddForce((moveVertical + moveHorizontal ) *  moveSpeed*Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        move();
    }
}
