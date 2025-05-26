using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    // Chamado quando a animação de ataque termina
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Tenta obter o script PlayerNovo no GameObject que tem o Animator
        PlayerNovo player = animator.GetComponent<PlayerNovo>();
        if (player != null)
        {
            player.EndAttack();
        }
    }
}
