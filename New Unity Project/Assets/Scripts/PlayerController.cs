using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float maxVelocity;
    public float jumpForce;
    public float rayDistance;

    public LayerMask groumdLayer;
    //public Vector2  maxVelolicity
    
    private GameController _gameController;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;
    private Vector2 _moveInput;
    private bool isGround;

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

        if (obj.action.name.CompareTo(_gameController.Gameplay.Jump.name) == 0)
        {
            if(obj.performed) Jump();
        }
    }

    private void move()
    {
        Vector3 camFoward = _mainCamera.transform.forward;
        camFoward.y = 0;
        
        Vector3 moveVertical = camFoward * _moveInput.y; 
        
        Vector3 camRight = _mainCamera.transform.right;
        
        camRight.y = 0;
        
        Vector3 moveHorizontal = camRight * _moveInput.x;
        
        // Adiciona a forca no objeto pelo rigidBody, com intencidade dada por moveSpeed
        _rigidbody.AddForce((moveVertical + moveHorizontal ) *  moveSpeed*Time.fixedDeltaTime);
    }

    private void LimiteVelocity()
    {
        //pegar a velocidadde do Player
        Vector3 velocity = _rigidbody.velocity;
        
        // checar se a velocidade esta dentro do limite em cada eixo
        // limitando o eixo x usando ifs, Abs e Sin
        if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = Mathf.Sign(velocity.x)*maxVelocity;
        
        // -maxVelocity < velocity.z < maxVelocity
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);
        
        //alterando a velocidade para dentro dos limites 
        _rigidbody.velocity = velocity;
    }

   
    /* Como fazer o jogador pular
     *  1- checar se o jogador esta no chao
     * -- a - checar a colisao a partir da fisica(usando eventos de colisao)
     * -- a - Vantagem: Fácilde implementar(adicionar uma função ja existente na Unity - OncollisionEnter)
     * -- a - Desvantagem: Não sabemos a hora exata que Unity vai chamar essa função (pode ser que o jogador
     * toque no chao e demore alguns frames para o jogo saber que ele eata no chao)
     * -- b - Podemos usar Layers para definir quais objts que o raycast deve checar a colisao
     * -- b - Vantagem: Resposta da colisao é imediata
     * -- b - Desventagem: Um pouco mais trabalhoso de configurar
     * -- Uma variavel bool que vai dizer para o resto codigo se o jogador esta no chao (True) ou não (False)
     * 2 - jogador precisa apertar o bo6tão de pulo
     */

    private void Jump()
    {
        if(isGround) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void CheckGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, rayDistance, groumdLayer);
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position,Vector3.down * rayDistance, Color.yellow);
    }

    private void FixedUpdate()
    {
        move();
        LimiteVelocity();
    }
}
