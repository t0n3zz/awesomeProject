namespace Pr11.Models
{
    public class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            HP = 25;
            Attack = 7;
            Defense = 2;
        }

        public override void AttackPlayer(Player player)
        {
            player.TakeDamage(Attack);
        }
    }
}