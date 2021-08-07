using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType 
{ 
    Left,Right,Straight,Fork
}
public class PlatformHandler : MonoBehaviour
{
    public static int pickupsBetweenGems = 0;

    public TurnType turnType;

    [SerializeField] List<GameObject> pickups;
    [SerializeField] List<GameObject> gems;
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] [Range(0, 1)] float gemSpawnProbability;
    [SerializeField] [Range(0, 1)] float initialObstacleProbability;
    [SerializeField] [Range(0, 1)] float hardObstacleProbability;
    [SerializeField] [Range(0, 0.5f)] float obstacleProbabilityAcceleration;
    [SerializeField] int minPickupsBetweenGems;

    static readonly System.Random random = new System.Random();
    static int lastPickupPosition = (int)(random.NextDouble() * 3);
    static int pickupsInRow = 0;
    static float obstacleProbability = -1;
    int minPickupsInRow = 5;
    void Start()
    {
        if(obstacleProbability<0) obstacleProbability = initialObstacleProbability;
        if (PlayerControls.playerMovementDistance == 0 && GetComponent<BoxCollider>() != null) PlayerControls.playerMovementDistance = GetComponent<BoxCollider>().size.z / 1.5f;
    }
    public void RemovePlatform()
    {
        Destroy(gameObject);
    }
    public void PlaceObjectOnPath(float angleY,bool increasObstacleProbability)
    {
        if (increasObstacleProbability&&obstacleProbability<hardObstacleProbability)
        {
            obstacleProbability += obstacleProbabilityAcceleration;
        }
        int maxNum = 100;
        int randomInt = random.Next(0, maxNum);
        if (randomInt < maxNum * obstacleProbability)
        {
            GenerateObject(obstacles, angleY);
        }
        else if (randomInt < maxNum * (obstacleProbability + gemSpawnProbability)&&pickupsBetweenGems>=minPickupsBetweenGems)
        {
            GenerateObject(gems, angleY);
            pickupsBetweenGems = 0;
        }
        else
        {
            GenerateObject(pickups, angleY);
            pickupsBetweenGems++;
        }
    }
    public void GenerateObject(List<GameObject> gameObjects, float angleY)
    {
        if (gameObjects.Count > 0)
        {
            int index = (int)(random.NextDouble() * gameObjects.Count);
            int placement = (int)(random.NextDouble() * 3);

            if(gameObjects == obstacles)
            {
                pickupsInRow = minPickupsInRow;
            }
            else if (gameObjects==pickups)
            {
                if (random.NextDouble()*100 >70 || pickupsInRow < minPickupsInRow)
                {
                    placement = lastPickupPosition;
                    pickupsInRow++;
                }
                else
                {
                    if (lastPickupPosition != placement) pickupsInRow = 0;

                    lastPickupPosition = placement;
                }
            }

            Vector3 position = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

            GameObject newObject = Instantiate(gameObjects[index], position, Quaternion.Euler(0, angleY, 0), transform);
            newObject.GetComponent<Transform>().localPosition = new Vector3((placement-1)*PlayerControls.playerMovementDistance, 0, 0);
        }
    }
}