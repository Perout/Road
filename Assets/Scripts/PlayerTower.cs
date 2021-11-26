using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Human _startHuman;//сами заспавним хумана
    [SerializeField] private Transform _distanceChecker;//насколько далеко от шеи
    [SerializeField] private float _fixationMaxDistance;//максимально допустимая дистанция 
    [SerializeField] private BoxCollider _checkCollider;//люди добавляются и нам надо опускать будет бокс колайдер

    private List<Human> _humans;//надо знать всех наших челов

    public event UnityAction<int> HumanAdded;

    private void Start()
        {
        _humans = new List<Human>();
        Vector3 spawnPoint = transform.position;//создаем первого игрока
        _humans.Add(Instantiate(_startHuman, spawnPoint, Quaternion.identity, transform));
        _humans[0].Run();
        HumanAdded?.Invoke(_humans.Count);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Human human))//если мы столкнулись с обьектом на котором есть компонент Human
        {
          
            Tower collisionTower = human.GetComponentInParent<Tower>();//полчаем компонент у родителя

            if (collisionTower !=null)
            {
                // обращаемся к колизию тавера и получаем лист людей
                List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChecker, _fixationMaxDistance);
                if (collectedHumans != null)//если не пустой то добавлем их себе в список 
                {
                    _humans[0].StopRun();
                    for (int i = collectedHumans.Count - 1; i >= 0; i--)//перебераем список от последнего к первому самый 0 индекс самый нижний человек и надо будет запускать анимацию бега одного чеговека
                    {
                        //получаем Human из массива
                        Human insertHuman = collectedHumans[i];
                        InsertHuman(insertHuman);
                        _humans.Insert(0, insertHuman);//вставляем в массив на 0 позицию
                        DisplaceChecker(human);

                    }
                    HumanAdded?.Invoke(_humans.Count);
                    _humans[0].Run();
                }
                collisionTower.Break();
            }

        }
    }
    private void InsertHuman (Human collectedHumans)
    {   
            _humans.Insert(0, collectedHumans);//вставляем в массив на 0 позицию
            SetHumanPosition(collectedHumans);
    }
    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;//его родитель это мы,а не башня
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);//изменяем позицию и сбрасывем x и z
        human.transform.localRotation = Quaternion.identity;//просто поворочиваем в 0 всех людей
    }
    private void DisplaceChecker(Human human)
    {
        float displaceScale = 1.3f;
        Vector3 distanceChekerNewPosition = _distanceChecker.position;
        distanceChekerNewPosition.y -= human.transform.localScale.y * displaceScale;
        _distanceChecker.position = distanceChekerNewPosition;
        _checkCollider.center = _distanceChecker.localPosition;

    }
}
