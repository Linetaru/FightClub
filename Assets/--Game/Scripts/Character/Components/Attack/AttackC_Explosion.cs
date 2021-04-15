using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Explosion : AttackComponent
{
    [SerializeField]
    private SphereCollider collider;

    [HorizontalGroup("1")]
    [HideLabel]
    public GameObject explosionVFX;
    [HorizontalGroup("1")]
    public float timeBeforeDestroying;

    [SerializeField]
	private float explosionRadius = 5.0f;
	[SerializeField]
	private float timeBeforeExplosion = 8.0f;
    private float timerExplosion = 0f;

    private bool hasHit;

	// Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
		
    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
        // On gère l'explosion automatiquement après 'timeBeforeExplosion'
        timerExplosion += Time.deltaTime;
        if (timerExplosion >= timeBeforeExplosion)
        {
            StartCoroutine(AutoExplode());
        }
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        hasHit = true;
        HitVFX();
    }

    // Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		
    }
    private IEnumerator AutoExplode()
    {
        collider.radius = explosionRadius;
        yield return new WaitForSeconds(0.5f); // Pour qu'il prenne en compte le hit
        HitVFX();
        if (!hasHit)
            Destroy(transform.root.gameObject);
    }

    public void HitVFX()
    {
        GameObject go = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(go, timeBeforeDestroying);
    }
}
