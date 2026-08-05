using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientPooler : MonoBehaviour
{
    [SerializeField] private List<Pool> ingredientsList;
    [SerializeField] private Dictionary<string, Queue<GameObject>> ingredientsDictionary;

    public static IngredientPooler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        ingredientsDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool ingredient in ingredientsList)
        {
            Queue<GameObject> ingredientPool = new Queue<GameObject>();

            for (int i = 0; i < ingredient.size; i++)
            {
                GameObject ingr = Instantiate(ingredient.prefab, this.transform);
                ingr.SetActive(false);
                ingredientPool.Enqueue(ingr);
            }

            ingredientsDictionary.Add(ingredient.ingredientTag, ingredientPool);
        }
    }

    public GameObject SpawnIngredientsFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!ingredientsDictionary.ContainsKey(tag))
        {
            Debug.Log($"The tag {tag} does not exist");
            return null;
        }
        GameObject ingredientToSpawn = ingredientsDictionary[tag].Dequeue();

        ingredientToSpawn.SetActive(true);
        ingredientToSpawn.transform.position = position;
        ingredientToSpawn.transform.rotation = rotation;

        ingredientsDictionary[tag].Enqueue(ingredientToSpawn);

        return ingredientToSpawn;
    }
}
