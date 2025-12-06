namespace Pr11.Models.Bosses
{
    public class Pestov : Skeleton
    {
        public Pestov()
        {
            Name = "Пестов С--";
            HP = (int)(HP * 1.3);
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }
    }
}