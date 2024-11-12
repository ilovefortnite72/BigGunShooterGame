using UnityEngine;

namespace WeaponSystem {
    public class SimpleHealthScript : MonoBehaviour {
        [SerializeField] private int health;
        [SerializeField] private AudioSource audioSource;

        //Remove health
        public void RemoveHealth(int damage) {
            health -= damage;

            if (health <= 0) {
                OnDeath();
            }
        }

        //Destroy object
        private void OnDeath() {
            if (audioSource != null) {
                audioSource.Play();

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                Destroy(gameObject, audioSource.clip.length);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}