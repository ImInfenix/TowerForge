//Script definissant une vague d'ennemis

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave : MonoBehaviour
{
    public Track track; //Chemin a suivre
    public List<WaveElement> waveComposition; //Composition de la vague
    public int waveIndex; //Numero de la vague

    int listIndex; //Etape a laquelle la vague est rendue
    int spawnedEnnemies; //nombre d'ennemis apparus dans l'etape en cours

    bool spawnFinished; //Si la vague a fini de generer des ennemis

    List<Ennemy> ennemiesAlive; //Ennemis actuellemnt en vie

    //Initialisation de la vague
    public void Launch()
    {
#if UNITY_EDITOR
        if(waveComposition.Count == 0)
        {
            Debug.LogError($"La vague {name} n'a pas ete initialisee /!\\");
        }
        else
        {
            for(int i = 0; i < waveComposition.Count; i++)
            {
                if(waveComposition[i].quantity == 0)
                {
                    Debug.LogError($"La vague {name} n'a pas ete initialisee correctemnt /!\\\nL'element {i} a une quantite de 0");
                }
                if(waveComposition[i].timeSpacing == 0)
                {
                    Debug.LogError($"La vague {name} n'a pas ete initialisee correctemnt /!\\\nL'element {i} a un espacement de 0");
                }
            }
        }
#endif
        //Intialisation
        ennemiesAlive = new List<Ennemy>();
        listIndex = 0;
        spawnedEnnemies = 0;
        spawnFinished = false;

        PlaySoundOnEvent.Instance.PlayNewWave(); //On joue le son de nouvelle vague

        Spawn(); //On commence a faire apparaitre des ennemis
    }

    void Spawn() //Appel repete pour instancier le prochaine ennemi
    {
        SpawnEnnemy(waveComposition[listIndex].gameObject); //On cree un nouvel ennemi
        spawnedEnnemies++; //On incremente le nombre d'ennemi crees dans l'etape
        if(waveComposition[listIndex].quantity == spawnedEnnemies) //Si on a fini une etape
        {
            if (listIndex < waveComposition.Count - 1) //S'il en reste au moinsune, on passe a l'etape suivante
            {
                listIndex++;
                spawnedEnnemies = 0;
            }
            else //Sinon, on declare la vague comme terminee pour ce qui est de faire apparaitre des ennemis
            {
                spawnFinished = true;
                return;
            }
        }

        Invoke("Spawn", waveComposition[listIndex].timeSpacing); //On refait apparaitre un enemi apres le delai specifie
    }

    //Appel pour instancier un ennemi lors d'une ecclosion
    public void SpawnFromEnnemy(Ennemy spawnerEnnemy, GameObject newEnnemyPrefab, bool destroySourceAtSpawn)
    {
        //On cree le nouvel ennemi
        Ennemy newEnnemy = Instantiate(newEnnemyPrefab, WavesManager._Instance.aliveEnnemies.transform).GetComponent<Ennemy>();

        //On l'initialise par rapport a l'ennemi qui l'a cree
        newEnnemy.transform.position = spawnerEnnemy.transform.position;
        newEnnemy.SetFollowedTrack(track, this);
        newEnnemy.isSpawnedByEnnemy = true; //On specifie qu'on l'a cree via un ennemi
        newEnnemy.TargetIndex = spawnerEnnemy.TargetIndex;

        newEnnemy.moneyEarnedOnDeath = 0.0f; //Comme il n'est pas genere naturellement, il ne donne pas d'argent

#if UNITY_EDITOR
        newEnnemy.name = $"{spawnerEnnemy.name} Spawned";
#endif

        ennemiesAlive.Add(newEnnemy); //On ajoute le nouvel ennemi a la vague

        if(destroySourceAtSpawn) //Si l'ancien ennemi doit etre detruit au moment de la creation du nouvel ennemi
        {
            ennemiesAlive.Remove(spawnerEnnemy); //On le retire des ennemis en vie
            Destroy(spawnerEnnemy.gameObject); //On le retire du jeu
        }
    }

    //Cree un nouvel ennemi
    void SpawnEnnemy(GameObject ennemyPrefab)
    {
        if(ennemyPrefab != null) //Si le gameObject est a null, on temporise simplement avant la suite de la vague
        {
            //On cree notre nouvel enemi
            GameObject ennemyGo = Instantiate(ennemyPrefab, WavesManager._Instance.aliveEnnemies.transform);
            ennemyGo.transform.position = track.spawnPoint.position;
            Ennemy ennemy = ennemyGo.GetComponent<Ennemy>();
            ennemy.SetFollowedTrack(track, this);

#if UNITY_EDITOR
            ennemy.name = $"Wave {waveIndex + 1} Phase {listIndex} Number {spawnedEnnemies}";
#endif

            ennemiesAlive.Add(ennemy); //On l'ajoute a la liste des ennemis en vie
        }
    }

    //Retire un ennemi des ennemis en vie
    public void RemoveEnnemy(Ennemy ennemy)
    {
        ennemiesAlive.Remove(ennemy); //On le retire de la vague
        
        //On verifie si la vague est terminee
        if (spawnFinished && ennemiesAlive.Count == 0) WavesManager._Instance.EndWave(waveIndex);
    }
}
