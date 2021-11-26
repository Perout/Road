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
    public List<Human> CollectHuman(Transform distanceChecker,float fixationMaxDistantion)//���� �������� ����� ������� ������ ��� ����� 
    {

        for (int i = 0; i < _humanInTower.Count; i++)
        {
            //�� ����� ����� �������� � ���� ���� ��������� � ��� � ������
            float distanceBeetweenPoints = CheckDistanceY(distanceChecker, _humanInTower[i].FixationPoint.transform);
            //Debug.Log(distanceBeetweenPoints);
            if (distanceBeetweenPoints<fixationMaxDistantion)
            {
                //������� ����� ���� ����� 
                List<Human> collectedHumans = _humanInTower.GetRange(0, i + 1);//�� �������� ����� � ����� 
                _humanInTower.RemoveRange(0, i + 1);//��� ���� ������� ������ ���� �� ������ ����� 
                return collectedHumans;//���������� ���� ��������
            }
        }
        return null;
    }
    private float CheckDistanceY(Transform distanceChecker,Transform humanFixationPoint)
    {
        Vector3 distanceCheckerY = new Vector3 (0, distanceChecker.position.y, 0);//��� ����� ������� ������ y,�������� ������ ������� � �� ������� 2 ������� 
        Vector3 humanFixationPointY = new Vector3(0, humanFixationPoint.position.y ,0);//����� ����� �������� �� ������ Humana
        return Vector3.Distance(distanceCheckerY, humanFixationPointY);//����� ������� ���� ��������� ����� ����� ���������
    }
    public void Break()
    {
        Destroy(gameObject);
    }
}
