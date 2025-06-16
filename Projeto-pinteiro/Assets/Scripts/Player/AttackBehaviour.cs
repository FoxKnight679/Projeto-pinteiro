using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        PlayerNovo player = animator.GetComponent<PlayerNovo>();
        if (player != null)
        {
            player.EndAttack();
        }
    }
}
