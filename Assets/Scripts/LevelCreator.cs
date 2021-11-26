using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;// знать путь куда идти
    [SerializeField] private Tower _towerTemplate;
    [SerializeField] private int _humanTowerCount;
    private void Start()
    {
        GenetationLevel();
    }
    private void GenetationLevel()
    {
        float roadLenght = _pathCreator.path.length;//получить длинну дороги
        float distanceBetweenTower = roadLenght/_humanTowerCount;//дистанция между башнями

        float distanceTravelled = 0;//переменная для учета сколько мы сгенерировали дистации
        Vector3 spawnPoint;//где спавнить будем

        for (int i = 0; i < _humanTowerCount; i++)
        {
            distanceTravelled += distanceBetweenTower;
            spawnPoint = _pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);

            Instantiate(_towerTemplate, spawnPoint, Quaternion.identity);
        }

    }
}
