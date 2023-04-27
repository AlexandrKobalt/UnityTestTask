using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GOSpawner : MonoBehaviour
{
    public GameObject goToSpawn;
    public Transform spawnPoint;

    #region InputGameObjects
    public GameObject spawnPeriodGO;
    public GameObject objectSpeedGO;
    public GameObject distanceGO;
    #endregion

    #region InputFields
    private TMP_InputField spawnPeriodInput;
    private TMP_InputField objectSpeedInput;
    private TMP_InputField distanceInput;
    #endregion

    #region Values
    private int _spawnPeriod;
    private int _objectSpeed;
    private int _distance;
    #endregion

    private void Start() 
    {
        // Получаем InputField компонент с GameObject
        spawnPeriodInput = spawnPeriodGO.GetComponent<TMP_InputField>();
        objectSpeedInput = objectSpeedGO.GetComponent<TMP_InputField>();
        distanceInput = distanceGO.GetComponent<TMP_InputField>();

        // Валидация - ввод только целых чисел
        spawnPeriodInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
        objectSpeedInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
        distanceInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
    }

    public void OnApply()
    {
        // Собираем заданные пользователем значения
        int.TryParse(spawnPeriodInput.text, out _spawnPeriod);
        int.TryParse(objectSpeedInput.text, out _objectSpeed);
        int.TryParse(distanceInput.text, out _distance);

        // Перезапускаем Invoke
        CancelInvoke();
        InvokeRepeating("Spawner", 0f, _spawnPeriod);
    }

    public void Spawner()
    {
        GameObject spawnedObject = Instantiate(goToSpawn, spawnPoint.transform.position, Quaternion.identity);
        StartCoroutine(MoveObject(spawnedObject));
    }

    private IEnumerator MoveObject(GameObject gObject)
    {
        Vector3 startPosition = gObject.transform.position;
        Vector3 endPosition = startPosition + spawnPoint.transform.forward * _distance;
        float distanceMoved = 0f;

        while (distanceMoved < _distance)
        {
            distanceMoved += _objectSpeed * Time.deltaTime;
            gObject.transform.position = Vector3.Lerp(startPosition, endPosition, distanceMoved / _distance);
            yield return null;
        }

        Destroy(gObject);
    }
}