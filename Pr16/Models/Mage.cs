namespace Pr16.Models
{
    public class Mage : Enemy
    {
        public Mage()
        {
            Name = "Маг";
            HP = 25;
            Attack = 5;
            Defense = 2;
        }

        public override int FreezeChance => 15;
    }
}
