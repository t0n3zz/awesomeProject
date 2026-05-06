namespace Pr16.Models
{
    public class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            HP = 40;
            Attack = 7;
            Defense = 5;
        }

        public override bool IgnoresArmor => true;
    }
}
