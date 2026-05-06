namespace Pr16.Models
{
    public class Weapon : Item
    {
        public int Attack { get; set; }

        public override string ToString()
        {
            return $"{Name} (+{Attack} атаки)";
        }
    }
}
