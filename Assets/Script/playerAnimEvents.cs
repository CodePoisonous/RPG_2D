using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player = null;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    /////////////////////////////////////////////
    private void AnimationTrigger()
    {
        player.AttachOver();
    }
}
