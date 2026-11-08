using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    List<RoomLoader> roomLoaders = new();
    List<EnemyPositionPair> enemies = new();
    struct EnemyPositionPair
    {
        public Vector3 position;
        public Enemy enemy;

        public EnemyPositionPair(Vector3 position, Enemy enemy)
        {
            this.position = position;
            this.enemy = enemy;
        }
    }

    public bool _isRoomActive = false;
    void Start()
    {
        if(!TryGetComponent<Collider2D>(out Collider2D col))
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
        

        foreach (RoomLoader loader in gameObject.GetComponentsInChildren<RoomLoader>())
        {
            roomLoaders.Add(loader);
            loader.Room = this;
        }

        foreach(Enemy enemy in gameObject.GetComponentsInChildren<Enemy>())
        {
            enemies.Add(new(enemy.transform.position,enemy));
            enemy.Room = this;
        }
        if (_isRoomActive)
        {
            ActivateRoom();
        }
        else
        {
            DeActivateRoom();
        }

        
    }
    public void ReloadRoom()
    {
        foreach(EnemyPositionPair enemyp in enemies)
        {
            var Enemy = enemyp.enemy;
            Enemy.transform.position = enemyp.position;
            Enemy.currentPoint = 0;
            Enemy.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (_isRoomActive)
        {
            //oda mantığı
            //düşman kontrolü
            //başka şeyler
            //başka mekankler
        }
    }
    public void ActivateRoom()
    {
        _isRoomActive = true;
        
        
        foreach(EnemyPositionPair enp in enemies)
        {
            Enemy en = enp.enemy;
            en.gameObject.SetActive(true);
        }
        //oda ile ilgli başka kod da olablir
    }
    public void DeActivateRoom()
    {
        _isRoomActive = false;
        foreach(EnemyPositionPair enp in enemies)
        {
            Enemy en = enp.enemy;
            en.gameObject.SetActive(false);
        }
    }

    

}
