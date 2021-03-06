using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerContact : MonoBehaviour
{
    public bool dies, bumpOnTop;
    public float knockbackForce, bumpOnTopForce;
    public float timeUntilRecoveringControl; //tiempo que dura el knockBack
    private MakeDamage mDamage;
    private DestroyBullet killThis;

    private void Start()
    {
        killThis = GetComponent<DestroyBullet>();
        mDamage = GetComponent<MakeDamage>();
    }

    //cuando el jugador salte sobre este objeto, lo destruirá
    private void OnCollisionEnter2D(Collision2D other)
    {
        // si colisionas con el jugador
        if (other.gameObject.CompareTag("Player"))
        {
            //buscamos la normal y calculamos el ángulo entre la normal y el vector Up
            ContactPoint2D contact = other.contacts[0];
            KnockBack kb = other.gameObject.GetComponentInParent<KnockBack>();
            float angle = Vector2.Angle(contact.normal, transform.up);

            //si el ángulo es de unos 180º (el jugador cae verticalmente), destruye al enemigo
            if (Mathf.Abs(angle - 180) < 0.1f && bumpOnTop)
            {
                if (kb != null) kb.BumpOnTop(-contact.normal, bumpOnTopForce);
                if (dies)
                {
                    KillMe();
                }
            }

            else
            {
                if (mDamage != null) mDamage.Damage(other);

                // impulsa al jugador en el sentido contrario a la colision
                if (kb != null) kb.KnockGO(-contact.normal, knockbackForce, timeUntilRecoveringControl);
            }
        }
        if (killThis != null) killThis.GetDestroyed();
    }
    public void KillMe()
    {

        try
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled =false;
            this.gameObject.GetComponent<Collider2D>().enabled =false;
            Collider2D[] cols = this.gameObject.GetComponentsInChildren<Collider2D>();
            foreach(Collider2D c in cols)
            {
                c.enabled = false;
            }
            AudioManager.instance.PlayClip(this.gameObject);
        }
        catch (System.Exception e)
        {
            Destroy(this.gameObject);
            print(e);
        }
        GameManager.instance.KillEnemy();
    }
}