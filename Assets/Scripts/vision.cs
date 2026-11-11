using UnityEngine;

public class vision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().RevealEnemy();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError(other.gameObject);
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().HideEnemy();

        }
    }
}
