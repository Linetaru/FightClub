﻿using System.Collections;
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
    private bool clearAfterExplode;

    private CharacterBase character;

	// Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
        character = user;
    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
        // On gère l'explosion automatiquement après 'timeBeforeExplosion'
        timerExplosion += Time.deltaTime;
        if (timerExplosion >= timeBeforeExplosion)
        {
            TriggerExplosion();
        }

        if(clearAfterExplode)
        {
            clearAfterExplode = false;
            user.Projectile.ClearAll();
        }
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        user.Projectile.ClearAll();
        hasHit = true;
        collider.radius = explosionRadius;
        ExplosionVFX();
    }

    public override void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {

    }
    public override void OnClash(CharacterBase user, CharacterBase target)
    {

    }

    // Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		
    }
    public IEnumerator Explode()
    {
        collider.radius = explosionRadius;
        yield return new WaitForSeconds(0.5f); // Pour qu'il prenne en compte le hit
        ExplosionVFX();
        if (!hasHit)
        {
            character.Projectile.ClearAll();
            Destroy(transform.root.gameObject);
        }
    }

    public void ExplosionVFX()
    {
        if(explosionVFX != null)
        {
            GameObject go = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(go, timeBeforeDestroying);
        }
    }

    public void TriggerExplosion()
    {
        StartCoroutine(Explode());
    }
}