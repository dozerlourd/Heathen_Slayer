using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necro_DeathBolt : MonoBehaviour
{
    #region Variable

    [SerializeField] float expRadius = 0.5f;
    [SerializeField] float damage = 7;
    [SerializeField] float gracePariod = 0.2f;

    [Space(15)]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotSpeed = 30;
    [SerializeField] float Duration = 10;

    Vector3 forwardDir;

    [SerializeField] GameObject BloodEffect;

    SpriteRenderer spriteRenderer;

    #endregion

    #region Unity Life Cycle

    void OnEnable()
    {
        StartCoroutine(FlipCheck());
        StartCoroutine(ExpAfterSetTime());
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GuidedMove();
    }

    #endregion

    #region Implementation Place

    #region Explosion Related Method

    IEnumerator FlipCheck()
    {
        yield return new WaitForEndOfFrame();
        forwardDir = spriteRenderer.flipX ? -transform.right : transform.right;
    }

    IEnumerator ExpAfterSetTime()
    {
        yield return new WaitForSeconds(Duration);
        PlayerDetecting();
    }

    void PlayerDetecting()
    {
        GameObject Player = null;
        
        RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, expRadius, Vector2.zero, 0, LayerMask.GetMask("L_Player"));
        Instantiate(BloodEffect, transform.position, Quaternion.identity);

        if (hitInfo)
        Player = hitInfo.collider.gameObject;

        if (Player != null)
        {
            PlayerStat playerStat = Player.GetComponent<PlayerStat>();
            Explosion(playerStat);
        }

        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    void Explosion(PlayerStat _playerStat)
    {
        print(_playerStat.gameObject.name);
        _playerStat.SetHP(damage, gracePariod);
        gameObject.SetActive(false);
    }

    #endregion

    #region Movement Related Method

    void GuidedMove()
    {

        Vector3 targetPos = PlayerSystem.Instance.Player.transform.position;
        Vector3 targerDir = (targetPos - transform.position).normalized;

        float angle = Mathf.Atan2(targerDir.y, targerDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotSpeed * Time.deltaTime);
        spriteRenderer.flipY = Mathf.Abs(transform.eulerAngles.z) >= 90 && Mathf.Abs(transform.eulerAngles.z) <= 270;

        forwardDir.y = 0;
        transform.Translate(forwardDir * moveSpeed * Time.deltaTime);

        #region junk code
        //Vector3 targetPos = PlayerSystem.Instance.Player.transform.position;
        //Vector3 moveDir = (targetPos - transform.position).normalized;
        //float angleToTarget = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        //Quaternion rotTarget = Quaternion.AngleAxis(angleToTarget, Vector3.forward);

        //Vector3 rotDir = Vector3.Lerp(forwardDir, targerDir, rotSpeed * Time.deltaTime).normalized;

        //transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, rotSpeed * Time.deltaTime);
        //transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        //transform.right = forwardDir == transform.right ? -rotDir : rotDir;
        //Quaternion.Slerp(transform.rotation, Quaternion.Euler(targerDir), rotSpeed * Time.deltaTime);
        //transform.eulerAngles = rotDir * 180;
        #endregion
    }

    void AccelerationGuidedMove()
    {

    }

    #endregion

    #region Setter

    /// <summary>
    /// For Setting Death Bolt's Flip Value
    /// </summary>
    /// <param name="val"> Death Bolt's Flip Value </param>
    public void SetFlip(bool val) => spriteRenderer.flipX = val;

    #endregion

    #region Callback Method

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Explosion(col.GetComponent<PlayerStat>());
        }
    }

    #endregion

    #endregion
}
