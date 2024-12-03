<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : Ability
{
    float attackNumber;
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Ability
{
    [SerializeField] 
    float attackPower;
}
>>>>>>> Stashed changes
