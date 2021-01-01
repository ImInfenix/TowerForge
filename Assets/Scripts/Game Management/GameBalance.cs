//Script destine a simplifier l'equilibrage du jeu en centralisant toutes les caracteristiques chiffrees

public static class GameBalance
{
    public static class Player
    {
        public const float startHealth = 100.0f; // Vie du joueur
#if UNITY_EDITOR
        public const float startMoney = 200.0f; //Argent de depart du joueur
#else
        public const float startMoney = 200.0f; //Argent de depart du joueur
#endif
    }

    public static class Mortar //Mortier
    {
        public const float range = 3.0f; //Portee
        public const float fireDelay = 1.5f; //Delai minimum entre deux tires
        public const float cost = 125.0f; //Cout d'achat
    }

    public static class Canonball //Boulet du mortier
    {
        public const float transitTime = 1.0f; //Duree du trajet avant d'atteindre la cible
        public const float impactRange = 1.0f; //Rayon de l'explosion
        public const float damages = 1.0f; //Degats infliges a l'impact
    }

    public static class Ballista //Balliste
    {
        public const float range = 5.0f; //Portee
        public const float fireDelay = 1.0f; //Delai minimum entre deux tires
        public const float cost = 75.0f; //Cout d'achat
    }

    public static class BallistaProjectile //Carreau de la balliste
    {
        public const float projectileSpeed = 700.0f; //Vitesse du carreau au lancement
        public const float projectileTimeout = 5.0f; //Duree ecoulee avant que le projectile se detruise par lui meme (pour eviter d'aller vers l'infini et l'au dela)
        public const float damages = 1.8f; //Degats infliges a l'impact
    }

    public static class Forge //Forge
    {
        public const float cost = 100.0f; //Cout d'achat
        public const uint burningTicks = 5; //Nombre de ticks de degats infliges
        public const float burningTick = 0.2f; //Delai entre deux ticks de degats
        public const float burningDamage = 0.2f; //Degats infliges par un tick de feu
    }

    public static class Temple //Temple
    {
        public const float cost = 125; //Cout d'achat
        public const float critProbability = 0.7f; //Probabilite d'infliger les degats critiques
    }

    public static class Laboratory //Laboratory
    {
        public const float cost = 200.0f; //Cout d'achat
        public const float poisonTick = 0.5f; //Duree ecoulee entre deux ticks de poison
        public const float poisonDamage = 0.1f; //Degats infliges par un tick de poison
    }

    public static class Tavern //Taverne
    {
        public const float cost = 50; //Cout d'achat
        public const float rewardDuration = 2.0f;
        public const float bountyIncreaseAmount = 1.3f;
    }

    public static class InsectAlien1 //Ennemi n°1
    {
        public const float speed = 1.1f; //Vitesse de deplacement
        public const float health = 4.0f; //Vie
        public const float damages = 3.0f; //Degats infliges si la fin du chemin est atteinte
        public const float moneyEarnedOnDeath = 8.0f; //Argent donne au joueur si tue
    }

    public static class InsectAlien2 //Ennemi n°2
    {
        public const float speed = 0.5f; //Vitesse de deplacement
        public const float health = 40.0f; //Vie
        public const float damages = 10.0f; //Degats infliges si la fin du chemin est atteinte
        public const float moneyEarnedOnDeath = 10.0f;//Argent donne au joueur si tue
    }

    public static class Queen //Ennemi n°3
    {
        public const float speed = 0.2f; //Vitesse de deplacement
        public const float health = 100.0f; //Vie
        public const float damages = 25.0f; //Degats infliges si la fin du chemin est atteinte
        public const float moneyEarnedOnDeath = 15.0f; //Argent donne au joueur si tue
        public const float incubationDuration = 5.0f; //Duree d'incubation
    }

    public static class Egg //Oeuf
    {
        public const float speed = 0.0f; //Vitesse de deplacement
        public const float health = 2.5f; //Vie
        public const float damages = 0.0f; //Degats infliges si la fin du chemin est atteinte
        public const float moneyEarnedOnDeath = 0.0f; //Argent donne au joueur si tue
        public const float incubationDuration = 3.0f; //Duree d'incubation
    }
}
