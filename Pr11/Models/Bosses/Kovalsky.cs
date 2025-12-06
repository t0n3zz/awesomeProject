namespace Pr11.Models.Bosses
{
    public class Kovalsky : Skeleton
    {
        public Kovalsky()
        {
            Name = "Ковальский";
            HP = (int)(HP * 2.5);
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }
}