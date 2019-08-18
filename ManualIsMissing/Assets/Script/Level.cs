using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CameraOrthoSize = 50f;
    private const float TamCastle=8f;
    private const float TamTopCastle = 4f;
    private const float speed = 30f;
    private const float destroyPosition=-100f;
    private const float spawnPosition = +100f;

    private List<Castle> castleList;
    private float castleSpawnTimer;
    private float castleSpawnTimerMax;
    private float gapSize;

    private void Awake()
    {
        castleList = new List<Castle>();
        castleSpawnTimerMax = 1.5f;
        gapSize = 50f;
    }

    private void Start()
    {
        //CreateGapCAstles(75f, 50f, 20f);
    }

    private void Update()
    {
        HandleCastleMovement();
        HandleCastleSpawning();
    }
    private void HandleCastleSpawning()
    {
        castleSpawnTimer -= Time.deltaTime;
        if (castleSpawnTimer < 0)
        {
            castleSpawnTimer += castleSpawnTimerMax;

            float heightEdgeLimit = 10f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CameraOrthoSize * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            CreateGapCAstles(height, gapSize, spawnPosition);
        }
    }
    private void HandleCastleMovement()
    {
        for (int i = 0; i < castleList.Count; i++)
        {
            Castle castle = castleList[i];
            castle.Move();
            if (castle.GetXposition() < destroyPosition)
            {
                castle.DestroySelf();
                castleList.Remove(castle);
                i--;
            }

        }
    }
    private void CreateGapCAstles(float gapy, float gapSize, float xposition)
    {
        CreateCastle(gapy - gapSize * .5f, xposition, true);
        CreateCastle(CameraOrthoSize * 2f - gapy - gapSize * .5f, xposition, false);
    }
    private void CreateCastle(float height, float xPosition,bool createBottom)
    {
        //CastleTop
        Transform head = Instantiate(GameAssets.GetInstance().pfTopCastle);
        float CastleTopPosition;
        if(createBottom)
        {
            CastleTopPosition = -CameraOrthoSize + height - TamTopCastle*.5f;
        }
        else
        {
            CastleTopPosition = +CameraOrthoSize - height - TamTopCastle*.5f;
        }
        head.position = new Vector3(xPosition,CastleTopPosition);

        //CastleBody
        Transform body = Instantiate(GameAssets.GetInstance().pfCastleBody);
        float CastleBodyPosition;
        if(createBottom)
        {
            CastleBodyPosition = -CameraOrthoSize;
        }
        else
        {
            CastleBodyPosition = +CameraOrthoSize;
            body.localScale = new Vector3(1, -1, 1);
        }
        body.position = new Vector3(xPosition, CastleBodyPosition);

        SpriteRenderer CastleBodySprite = body.GetComponent<SpriteRenderer>();
        CastleBodySprite.size = new Vector2(TamCastle, height);

        BoxCollider2D boxcolliderbody = body.GetComponent<BoxCollider2D>();
        boxcolliderbody.size = new Vector2(TamCastle, height);
        boxcolliderbody.offset = new Vector2(0f, height*0.5f);

        Castle castle = new Castle(head, body);
        castleList.Add(castle);
    }

    private class Castle
    {
        private Transform headTransform;
        private Transform bodyTransform;

        public Castle(Transform headTransform,Transform bodyTransform)
        {
            this.headTransform = headTransform;
            this.bodyTransform = bodyTransform;
        }
        public void Move()
        {
            headTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
            bodyTransform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        }
        public float GetXposition()
        {
            return headTransform.position.x;
        }
        public void DestroySelf()
        {
            Destroy(headTransform.gameObject);
            Destroy(bodyTransform.gameObject);
        }
    }
}
