using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Human[] _humansTemplate;
    [SerializeField] private Vector2Int _himanInTowerRange;
    private List<Human> _humanInTower;
    private void Start()
    {
        _humanInTower = new List<Human>();
        int humanInToTowerCount = Random.Range(_himanInTowerRange.x, _himanInTowerRange.y);
        SpawnHumans(humanInToTowerCount);
    }
    private void SpawnHumans(int humanCount)
    {
        Vector3 spawnPoint=transform.position;
        for (int i = 0; i < humanCount; i++)
        {
            Human spawnHuman = _humansTemplate[Random.Range(0, _humansTemplate.Length)];
            _humanInTower.Add(Instantiate(spawnHuman, spawnPoint, Quaternion.identity, transform));
            _humanInTower[i].transform.localPosition = new Vector3(0, _humanInTower[i].transform.localPosition.y, 0);

            spawnPoint = _humanInTower[i].FixationPoint.position;
        }
        
    }
    public List<Human> CollectHuman(Transform distanceChecker,float fixationMaxDistantion)//есть пуличный метод который вернет нам людей 
    {

        for (int i = 0; i < _humanInTower.Count; i++)
        {
            //Мы берем этого человека и всех ниже добавляем к нам в список
            float distanceBeetweenPoints = CheckDistanceY(distanceChecker, _humanInTower[i].FixationPoint.transform);
            //Debug.Log(distanceBeetweenPoints);
            if (distanceBeetweenPoints<fixationMaxDistantion)
            {
                //создаем новый лист людей 
                List<Human> collectedHumans = _humanInTower.GetRange(0, i + 1);//мы собираем людей в тавер 
                _humanInTower.RemoveRange(0, i + 1);//нам надо забрать теперь всех из списка людей 
                return collectedHumans;//возвращаем нашу колекцию
            }
        }
        return null;
    }
    private float CheckDistanceY(Transform distanceChecker,Transform humanFixationPoint)
    {
        Vector3 distanceCheckerY = new Vector3 (0, distanceChecker.position.y, 0);//нам нужна позиция только y,обнуляем другие позиции и мы заводим 2 вектора 
        Vector3 humanFixationPointY = new Vector3(0, humanFixationPoint.position.y ,0);//ветор будет отвечать за нашего Humana
        return Vector3.Distance(distanceCheckerY, humanFixationPointY);//метод который дает дистанцию между двумя векторами
    }
    public void Break()
    {
        Destroy(gameObject);
    }
}
