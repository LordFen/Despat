using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CreaturePattern
{
    public class GameController : MonoBehaviour {

        public static AudioSource audioSource;
        //hit sound
        public AudioClip hit;

        //Text life and game over
        public Text end;
        public Text life;

        
        //spawn position
        public Transform[] tabSpawn;

        //Prefab
        public GameObject[] tabFriends;
        public GameObject[] tabEnemies;
        public GameObject[] blood;
        public AudioClip spider;
        //sound monsters
        public AudioClip[] monsters;
        //Player
        public GameObject player;
        private UnityStandardAssets.Copy._2D.Platformer2DUserControl playerOri;

        //Magazine
        public Transform enemyParent;
        public Transform friendParent;

        public GameObject explosion;

        //list of all creatures
        List<Creature> listFriends = new List<Creature>();
        List<Creature> listEnemies = new List<Creature>();


        Creature playerCreatureTrans;
        float timer = 0.0f;
        float timerRound = 5;
        float timerSpawn = 0;
        bool stopSpawn = false;
        

        void Start() {
            //init grid
            timer = 0.0f;
            playerCreatureTrans = new Creature();
            playerOri = player.GetComponent<UnityStandardAssets.Copy._2D.Platformer2DUserControl>();
            stopSpawn = false;
            audioSource = GetComponent<AudioSource>();

        
        }

        private void Update()
        {
            //Show Hp and game over
            if (!stopSpawn)
            {
                if (playerOri.GetDeath())
                {
                    stopSpawn = true;
                    end.gameObject.SetActive(true);
                }

                life.text = playerOri.hp.ToString();
            }
        }


        private void FixedUpdate()
        {
            Creature close = null;
            playerCreatureTrans.creatureTrans = player.transform;
            //Enemies Action
            for (int i=0;i<listEnemies.Count;i++)
            {
                if (!listEnemies[i].die)
                {
                    close = FindClosestObjectForEnemy(listEnemies[i]);
                    if (close == null)
                    {
                        listEnemies[i].AddClosestObject(playerCreatureTrans);
                    }
                    else
                    {
                        float distance = Vector3.Distance(close.creatureTrans.position, listEnemies[i].creatureTrans.position);
                        if (distance <= 0.8f)
                        {
                            listEnemies[i].AddClosestObject(close);
                        }
                        else
                        {
                            listEnemies[i].AddClosestObject(playerCreatureTrans);
                        }
                    }
                    listEnemies[i].Action();
                }
                else
                {
                   //Destory Enemy and create blood

                    GameObject destroyG = listEnemies[i].creatureTrans.gameObject;
                    listEnemies.RemoveAt(i);
                    int random = Random.Range(0, 5);
                    GameObject imageBlood = Lean.LeanPool.Spawn(blood[random], destroyG.transform.position, Quaternion.identity);
                    Lean.LeanPool.Despawn(imageBlood, 10);
                    Lean.LeanPool.Despawn(destroyG);
                    
                }
            }

            //Friend Action
            for(int i=0;i<listFriends.Count;i++)
            {
                if (!listFriends[i].die)
                {
                    close = FindClosestObjectForFriend(listFriends[i]);
                    listFriends[i].AddClosestObject(close);
                    listFriends[i].Action();
                }
                else
                {
                    //Destory Friend and create blood

                    GameObject destroyG = listFriends[i].creatureTrans.gameObject;
                    listFriends.RemoveAt(i);
                    int random = Random.Range(0, 5);
                    GameObject imageBlood = Lean.LeanPool.Spawn(blood[random], destroyG.transform.position, Quaternion.identity);
                    Lean.LeanPool.Despawn(imageBlood, 10);
                    Lean.LeanPool.Despawn(destroyG);
                }
            }

            //Creating enemies with frequency
            timer += Time.fixedDeltaTime;
            timerSpawn += Time.fixedDeltaTime;

            //Fast Spawning
            if(timerSpawn>=30)
            {

                timerRound /= timerRound;
                timerSpawn = 0.0f;
               
            }

            if (timer>=timerRound)
            {
                if(!stopSpawn)
                CreateEnemy();
            }
        }

        //create enemy - random
        void CreateEnemy()
        {
                audioSource.PlayOneShot(spider, 1);
                timer = 0.0f;
                int randomEnemy = Random.Range(0, 3);
                int randomPosition = Random.Range(0, 8);
                GameObject g=Lean.LeanPool.Spawn(tabEnemies[randomEnemy], new Vector2(tabSpawn[randomPosition].position.x, tabSpawn[randomPosition].position.y), Quaternion.identity);
                Animator animator = g.GetComponent<Animator>();
                listEnemies.Add(new Enemy(g,player,randomEnemy,animator,explosion,hit));
                g.transform.parent = enemyParent;
            
        }

        //create friends - buttons
        public void CreateFriend(int i)
        {
            if (!stopSpawn)
            {
                int random = Random.Range(0, 2);
                audioSource.PlayOneShot(monsters[random]);
                GameObject g = Lean.LeanPool.Spawn(tabFriends[i], new Vector2(player.transform.position.x, player.transform.position.y), Quaternion.identity);
                Animator animator = g.GetComponent<Animator>();
                listFriends.Add(new Friend(g, i, animator,explosion,hit));
                g.transform.parent = friendParent;
            }
        }

        //Find Object
        Creature FindClosestObjectForFriend(Creature c)
        {
            Creature findCreature = null;

            float bestDistance = Mathf.Infinity;
            
            for(int i=0;i<listEnemies.Count;i++)
            {
                float distance = (c.creatureTrans.position - listEnemies[i].creatureTrans.position).sqrMagnitude;
                if(distance<bestDistance)
                {
                    bestDistance = distance;
                    findCreature = listEnemies[i];
                }
            }
            

            return findCreature;
        }

        //Find Object
        Creature FindClosestObjectForEnemy(Creature c)
        {
            Creature findCreature = null;

            float bestDistance = Mathf.Infinity;

            for (int i = 0; i < listFriends.Count; i++)
            {
                float distance = (c.creatureTrans.position - listFriends[i].creatureTrans.position).sqrMagnitude;
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    findCreature = listFriends[i];
                }
            }


            return findCreature;
        }

        public void Exit()
        {
            SceneManager.LoadScene(0);
        }
    }


}
