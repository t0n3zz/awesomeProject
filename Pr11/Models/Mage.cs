namespace Pr11.Models
{
    public class Mage : Enemy
    {
        public Mage()
        {
            Name = "Маг";
            HP = 20;
            Attack = 10;
            Defense = 1;
        }

        public override void AttackPlayer(Player player)
        {
            player.TakeDamage(Math.Max(0, Attack - player.Defense));
        }
    }
}