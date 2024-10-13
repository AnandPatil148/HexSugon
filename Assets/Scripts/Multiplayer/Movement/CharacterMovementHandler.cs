using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{

    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;

    PlayerMovementNetwork playerMovementNetwork;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if( GetInput(out NetworkInputData networkInputData) )
        {
            
        }
    }

}
