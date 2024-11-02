using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform player;            // �÷��̾� ��ġ
    public float moveDistance = 3f;     // ���� �̵� �Ÿ�
    public float moveSpeed = 1f;        // ���� �⺻ �̵� �ӵ�
    public float approachSpeed = 2f;    // �÷��̾� ���� �� �ӵ�
    public float approachRange = 5f;    // ���� ���� �Ÿ�
    public float approachDuration = 1f; // ���� ���� �ð�
    public Color hitColor = Color.red; // �ǰ� �� ����
    private Color originalColor;         // ���� ����
    private float fixedY;

    private Vector3 startPosition;      // ���� ��ġ
    private bool movingLeft = true;     // �̵� ���� üũ
    private bool isApproaching = false; // �÷��̾� ���� �� ����
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������
    private FadeManager fadeManager;    // ���̵� �Ŵ���

    private int health = 3;             // ���� ü��
    private bool isHit = false;          // �ǰ� ���� üũ

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fadeManager = FindObjectOfType<FadeManager>(); // ���̵� �Ŵ��� ã��
        originalColor = spriteRenderer.color; // ���� ���� ����
        fixedY = transform.position.y;
        StartCoroutine(MovePattern()); // �̵� ���� ����
    }

    void Update()
    {
        // �÷��̾���� �Ÿ� üũ
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= approachRange && !isApproaching)
        {
            StopAllCoroutines();
            StartCoroutine(ApproachPlayer());
        }

        // ���콺 ���� Ŭ�� ����
        if (Input.GetMouseButtonDown(0) && !isHit)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) // �� �±� Ȯ��
            {
                TakeDamage(1);
            }
        }

        transform.position = new Vector3(transform.position.x, fixedY,transform.position.z);
    }

    // �÷��̾�� �浹 ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadeManager.FadeOutAndRestart(); // ���̵� �ƿ� �� �� �����
        }
    }

    // ���� ���� ó��
    private void TakeDamage(int damage)
    {
        spriteRenderer.color = hitColor;
        health -= damage; // ü�� ����
        isHit = true; // �ǰ� ���� ����
        StopAllCoroutines(); // ���� ��� �ڷ�ƾ ����
        StartCoroutine(HandleHit()); // �ǰ� ó�� ����

        if (health <= 0)
        {
            Destroy(gameObject); // ü���� 0 ������ ��� ������Ʈ �ı�
        }
    }

    IEnumerator HandleHit()
    {
        // 1�� ���� ���� �̵��� ����
        yield return new WaitForSeconds(1f);
        spriteRenderer.color = originalColor; // ���� �������� ����
        isHit = false; // �ǰ� ���� ����
        StartCoroutine(MovePattern()); // �̵� ���� �簳
    }

    IEnumerator MovePattern()
    {
        while (true)
        {
            float targetX = movingLeft ? startPosition.x - moveDistance : startPosition.x + moveDistance;
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            movingLeft = !movingLeft;
            spriteRenderer.flipX = movingLeft;
        }
    }

    IEnumerator ApproachPlayer()
    {
        isApproaching = true;
        float elapsedTime = 0f;

        float fixedY = transform.position.y;

        while (elapsedTime < approachDuration)
        {
            Vector3 targetPosition = new Vector3(player.position.x, fixedY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, player.position, approachSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isApproaching = false;
        StartCoroutine(MovePattern());
    }
}
