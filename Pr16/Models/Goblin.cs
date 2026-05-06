namespace Pr16.Models
{
    public class Goblin : Enemy
    {
        public Goblin()
        {
            Name = "Гоблин";
            HP = 30;
            Attack = 3;
            Defense = 2;
        }

        public override int CritChance => 20;
    }
}
