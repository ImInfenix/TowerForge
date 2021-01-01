//Script gerant les statistiques du joueur

public class PlayerStats
{
    float health; //Vide
    float money; //Argent
    readonly GameManager gameManager; //Reference vers le GameManager

    //Constructeur
    public PlayerStats(GameManager gameManager)
    {
        this.gameManager = gameManager; //On reference le GameManager
        health = GameBalance.Player.startHealth; //On intialise la vie
        money = GameBalance.Player.startMoney; //On intialise l'argent

        gameManager.healthText.text = health.ToString(); //On met a jour l'affichage de la vie
        gameManager.moneyText.text = money.ToString(); //On met a jour l'affichage de l'argent
    }

    //Applique des degats au joueur
    public void TakeDamages(float damages)
    {
        health -= damages; //On retire la vie
        if(health <= 0)    //Si on passe a 0
        {
            health = 0;
            gameManager.LoseGame(); //On declenche la defaite
        }
        if(gameManager.healthAnimator.isActiveAndEnabled)
            gameManager.healthAnimator.Play("HeartBreak"); //On joue l'animation de perte de vie
        gameManager.healthText.text = health.ToString(); //On me a jour l'affichage de la vie
    }

    //Modifie le montant d'argent du joueur
    public void AddMoney(float amount)
    {
        money += amount; //Modification
        gameManager.moneyText.text = money.ToString(); //Mise a jour visuelle
    }

    //Fait payer un certain montant au joueur, renvoie true si le payement a eu lieu, false sinon
    public bool Pay(float amount)
    {
        if (amount > money) return false; //Si le payement n'est pas faisable on renvoie false
        AddMoney(-amount); //On retire le montant a l'argent du joueur
        return true;
    }
}
