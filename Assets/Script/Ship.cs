using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Ship : NetworkBehaviour
{
    public abstract string shipName { get; }

    public abstract int atkPoints { get; }
    public abstract int spdPoints { get; }
    public abstract int hltPoints { get; }

    public GameObject shipCamera;
    public GameObject miniCamera;

    float horizontalMove = 0;
    float verticalMove = 0;

    protected InputMap _inputs;

    Rigidbody2D _rigidbody;

    public NetworkVariable<bool> isDeath = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        _inputs = new InputMap();
        _inputs.Ship.Enable();
        _rigidbody = GetComponent<Rigidbody2D>();
        this.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        if (IsOwner)
        {
            shipCamera.SetActive(true);
            miniCamera.SetActive(true);
        }
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            int damege = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>().damage.Value;
            if (damege >= hltPoints)
            {
                Debug.Log("Destroy");
                isDeath.Value = true;
                foreach (var ship in GameObject.FindObjectsOfType<Ship>())
                {
                    if (!ship.isDeath.Value)
                    {
                        ship.shipCamera.SetActive(true);
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {

        Vector2 movementInput = _inputs.Ship.Move.ReadValue<Vector2>();

        horizontalMove += movementInput.x * spdPoints;
        verticalMove = movementInput.y * spdPoints;

        this.transform.rotation = Quaternion.AngleAxis(horizontalMove, Vector3.forward);
        _rigidbody.AddRelativeForce(new Vector2(0, verticalMove), ForceMode2D.Impulse);
    }

    protected void CheckDamge()
    {
        //Debug.Log("Hola");
        int damege = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>().damage.Value;
        if (damege >= hltPoints)
        {
            Debug.Log("Destroy");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            PlayerNetScript playerNet = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetScript>();
            if (playerNet.shield.Value > 0)
            {
                playerNet.shield.Value -= enemy.dmgPoints;
            }
            else
            {
                playerNet.damage.Value += enemy.dmgPoints;
            }
        }
    }
}
