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
    private const float DragonPosition=0f;

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    private List<Castle> castleList;
    private int castlePassedCount=0;
    private int castleSpawned=0;
    private float castleSpawnTimer;
    private float castleSpawnTimerMax;
    private float gapSize;
    private State state;
    private enum State
    {
        WaitingToStart,
        Playing,
        DragonDead,
    }
    private void Awake()
    {
        instance = this;
        castleList = new List<Castle>();
        castleSpawnTimerMax = 1.5f;
        gapSize = 50f;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Player.GetInstance().Ondied += Dragon_OnDied;
        Player.GetInstance().OnstartedPlaying += Dragon_OnStartedPlaying;
    }

    private void Dragon_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Dragon_OnDied(object sender, System.EventArgs e)
    {
        Debug.Log("Dead");
        state = State.DragonDead;
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            HandleCastleMovement();
            HandleCastleSpawning();

        }
     
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
            bool isToTheRightOfDragon = castle.GetXposition() > DragonPosition;
            castle.Move();
            if(isToTheRightOfDragon && castle.GetXposition() <= DragonPosition && castle.IsBottom())
            {
                castlePassedCount++;
            }
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
        castleSpawned++;
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

        Castle castle = new Castle(head, body, createBottom);
        castleList.Add(castle);
    }

    public int GetCastleSpawned()
    {
        return castleSpawned;
    }

    public int GetCastlePassedCount()
    {
        return castlePassedCount;
    }

    private class Castle
    {
        private Transform headTransform;
        private Transform bodyTransform;
        private bool isBottom;

        public Castle(Transform headTransform,Transform bodyTransform,bool isBottom)
        {
            this.headTransform = headTransform;
            this.bodyTransform = bodyTransform;
            this.isBottom = isBottom;
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
        public bool IsBottom()
        {
            return isBottom;
        }
        public void DestroySelf()
        {
            Destroy(headTransform.gameObject);
            Destroy(bodyTransform.gameObject);
        }
    }
}
