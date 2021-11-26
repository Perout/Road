using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Human _startHuman;//���� ��������� ������
    [SerializeField] private Transform _distanceChecker;//��������� ������ �� ���
    [SerializeField] private float _fixationMaxDistance;//����������� ���������� ��������� 
    [SerializeField] private BoxCollider _checkCollider;//���� ����������� � ��� ���� �������� ����� ���� ��������

    private List<Human> _humans;//���� ����� ���� ����� �����

    public event UnityAction<int> HumanAdded;

    private void Start()
        {
        _humans = new List<Human>();
        Vector3 spawnPoint = transform.position;//������� ������� ������
        _humans.Add(Instantiate(_startHuman, spawnPoint, Quaternion.identity, transform));
        _humans[0].Run();
        HumanAdded?.Invoke(_humans.Count);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Human human))//���� �� ����������� � �������� �� ������� ���� ��������� Human
        {
          
            Tower collisionTower = human.GetComponentInParent<Tower>();//������� ��������� � ��������

            if (collisionTower !=null)
            {
                // ���������� � ������� ������ � �������� ���� �����
                List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChecker, _fixationMaxDistance);
                if (collectedHumans != null)//���� �� ������ �� �������� �� ���� � ������ 
                {
                    _humans[0].StopRun();
                    for (int i = collectedHumans.Count - 1; i >= 0; i--)//���������� ������ �� ���������� � ������� ����� 0 ������ ����� ������ ������� � ���� ����� ��������� �������� ���� ������ ��������
                    {
                        //�������� Human �� �������
                        Human insertHuman = collectedHumans[i];
                        InsertHuman(insertHuman);
                        _humans.Insert(0, insertHuman);//��������� � ������ �� 0 �������
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
            _humans.Insert(0, collectedHumans);//��������� � ������ �� 0 �������
            SetHumanPosition(collectedHumans);
    }
    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;//��� �������� ��� ��,� �� �����
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);//�������� ������� � ��������� x � z
        human.transform.localRotation = Quaternion.identity;//������ ������������ � 0 ���� �����
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
