using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Feedbacks;

public class BallExplosionCharacterState : CharacterState
{
    [SerializeField]
    CharacterState ballKickOffState;

    [SerializeField]
    GameObject explosionParticlePrefab;

    GameObject explosionParticle;

    [SerializeField]
    GameObject ballModel;

    [SerializeField]
    Transform ballRespawnPosition;

    [SerializeField]
    float respawnDuration = 3.0f;

    float timer = 0f;

    float limitTimescale;
    //bool explosionUnactive = false;

    private void Start()
    {
        limitTimescale = Time.timeScale;
    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        Camera.main.GetComponentInParent<ScreenShake>().StartScreenShake(0.5f, .5f);

        timer = respawnDuration;
        explosionParticle = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        character.Knockback.Launch(new Vector2(0, 0), 0);
        ballModel.SetActive(false);
        //character.transform.position = new Vector3(0f, 0f, 500f);
        character.Knockback.IsInvulnerable = true;
        character.Movement.SetSpeed(0f, 0f);
        character.Movement.ResetAcceleration();

        //explosionUnactive = false;
        //GetComponentInChildren<AttackSubManager>().InitAttack(character);
        //GetComponentInChildren<AttackSubManager>().ActionActive();

        Time.timeScale = 0.1f;
        //StartCoroutine(DestroyExplosionHitbox());
    }

    public override void UpdateState(CharacterBase character)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            character.SetState(ballKickOffState);
        }

        if (Time.timeScale < limitTimescale)
        {
            Time.timeScale += Time.unscaledDeltaTime/* * 0.3f*/;
        }
    }

    //IEnumerator DestroyExplosionHitbox()
    //{
    //    yield return new WaitForSeconds(.005f);
    //    Debug.Log("ExplosionUnactive");
    //    GetComponentInChildren<AttackSubManager>().ActionUnactive();
    //}


    //public override void LateUpdateState(CharacterBase character)
    //{

    //}

    public override void EndState(CharacterBase character, CharacterState newState)
    {
        character.Knockback.IsInvulnerable = false;
        Destroy(explosionParticle.gameObject);
        character.transform.position = ballRespawnPosition.position;
        ballModel.SetActive(true);
    }
}