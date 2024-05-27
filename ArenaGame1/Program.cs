using ArenaGame.Heroes;
using ArenaGame.Weapons;
using ArenaGame;
using System;

namespace ArenaGame
{
    public class GameEngine
    {
        public IHero HeroA { get; set; }
        public IHero HeroB { get; set; }
        public Action<NotificationArgs> NotificationsCallBack { get; set; }
        public IHero Winner { get; private set; }

        public void Fight()
        {
            while (HeroA.Health > 0 && HeroB.Health > 0)
            {
                ExecuteAttack(HeroA, HeroB);
                if (HeroB.Health <= 0)
                {
                    Winner = HeroA;
                    break;
                }

                ExecuteAttack(HeroB, HeroA);
                if (HeroA.Health <= 0)
                {
                    Winner = HeroB;
                    break;
                }
            }
        }

        private void ExecuteAttack(IHero attacker, IHero defender)
        {
            double attack = attacker.Weapon.AttackPower;
            double damage = Math.Max(0, attack - defender.Defense);
            defender.Health -= damage;

            NotificationsCallBack?.Invoke(new NotificationArgs(attacker, defender, attack, damage));
        }

        public class NotificationArgs
        {
            public IHero Attacker { get; }
            public IHero Defender { get; }
            public double Attack { get; }
            public double Damage { get; }

            public NotificationArgs(IHero attacker, IHero defender, double attack, double damage)
            {
                Attacker = attacker;
                Defender = defender;
                Attack = attack;
                Damage = damage;
            }
        }
    }

    public interface IHero
    {
        string Name { get; }
        double Health { get; set; }
        double Defense { get; }
        IWeapon Weapon { get; }
    }
}

namespace ArenaGame.Heroes
{
    public class Barbarian : IHero
    {
        public string Name { get; }
        public double Health { get; set; }
        public double Defense { get; }
        public IWeapon Weapon { get; }

        public Barbarian(string name, double health, double defense, IWeapon weapon)
        {
            Name = name;
            Health = health;
            Defense = defense;
            Weapon = weapon;
        }
    }

    public class Sorcerer : IHero
    {
        public string Name { get; }
        public double Health { get; set; }
        public double Defense { get; }
        public IWeapon Weapon { get; }

        public Sorcerer(string name, double health, double defense, IWeapon weapon)
        {
            Name = name;
            Health = health;
            Defense = defense;
            Weapon = weapon;
        }
    }
}

namespace ArenaGame.Weapons
{
    public interface IWeapon
    {
        string Name { get; }
        double AttackPower { get; }
    }

    public class Axe : IWeapon
    {
        public string Name { get; }
        public double AttackPower { get; }

        public Axe(string name, double attackPower = 15)
        {
            Name = name;
            AttackPower = attackPower;
        }
    }

    public class Wand : IWeapon
    {
        public string Name { get; }
        public double AttackPower { get; }

        public Wand(string name, double attackPower = 10)
        {
            Name = name;
            AttackPower = attackPower;
        }
    }
}

namespace ConsoleArenaGame
{
    class Program
    {
        static void ConsoleNotification(GameEngine.NotificationArgs args)
        {
            Console.WriteLine($"{args.Attacker.Name} attacked {args.Defender.Name} with {Math.Round(args.Attack, 2)} and caused {Math.Round(args.Damage, 2)} damage.");
            Console.WriteLine($"Attacker: {args.Attacker.Name}, Health: {Math.Round(args.Attacker.Health, 2)}");
            Console.WriteLine($"Defender: {args.Defender.Name}, Health: {Math.Round(args.Defender.Health, 2)}");
        }

        static void Main(string[] args)
        {
            GameEngine gameEngine = new GameEngine()
            {
                HeroA = new Barbarian("Barbarian", 120, 20, new Axe("Battle Axe")),
                HeroB = new Sorcerer("Sorcerer", 80, 10, new Wand("Magic Wand")),
                NotificationsCallBack = ConsoleNotification
            };

            gameEngine.Fight();

            Console.WriteLine();
            Console.WriteLine($"And the winner is {gameEngine.Winner.Name}");
        }
    }
}

